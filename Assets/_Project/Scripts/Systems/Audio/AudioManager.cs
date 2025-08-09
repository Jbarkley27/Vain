using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;

public enum MusicCategory
{
    Ambient,
    Battle,
    Boss
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Music queues by category
    private Dictionary<MusicCategory, Queue<string>> musicQueues = new Dictionary<MusicCategory, Queue<string>>();

    private EventInstance currentMusic;
    private EventInstance nextMusic;

    private MusicCategory currentCategory = MusicCategory.Ambient;
    private bool isSwitching = false;

    [Header("Crossfade Settings")]
    public float crossfadeTime = 2f; // seconds

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize category queues
        foreach (MusicCategory category in System.Enum.GetValues(typeof(MusicCategory)))
        {
            musicQueues[category] = new Queue<string>();
        }
    }

    // --- Public API ---

    public void PlayOneShot(string eventPath)
    {
        RuntimeManager.PlayOneShot(eventPath);
    }

    public void AddToMusicQueue(MusicCategory category, string eventPath)
    {
        musicQueues[category].Enqueue(eventPath);
    }

    public void StartMusic(MusicCategory category)
    {
        currentCategory = category;
        PlayNextTrack();
    }

    public void SwitchMusicCategory(MusicCategory newCategory)
    {
        if (isSwitching || musicQueues[newCategory].Count == 0)
            return;

        StartCoroutine(CrossfadeToCategory(newCategory));
    }

    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }
    }

    // --- Internal Logic ---

    private void PlayNextTrack()
    {
        if (musicQueues[currentCategory].Count == 0) return;

        string nextTrackPath = musicQueues[currentCategory].Dequeue();
        musicQueues[currentCategory].Enqueue(nextTrackPath); // cycle

        currentMusic = RuntimeManager.CreateInstance(nextTrackPath);
        currentMusic.start();

        StartCoroutine(CheckTrackEnd());
    }

    private IEnumerator CheckTrackEnd()
    {
        PLAYBACK_STATE state;
        do
        {
            yield return null;
            if (currentMusic.isValid())
                currentMusic.getPlaybackState(out state);
            else
                yield break;
        } while (state != PLAYBACK_STATE.STOPPED && state != PLAYBACK_STATE.STOPPING);

        PlayNextTrack();
    }

    private IEnumerator CrossfadeToCategory(MusicCategory newCategory)
    {
        isSwitching = true;

        // Create next track instance
        string nextTrackPath = musicQueues[newCategory].Dequeue();
        musicQueues[newCategory].Enqueue(nextTrackPath);
        nextMusic = RuntimeManager.CreateInstance(nextTrackPath);

        // Start next music at low volume
        nextMusic.setVolume(0f);
        nextMusic.start();

        // Fade out current, fade in next
        float timer = 0f;
        float currentVol = 1f;

        while (timer < crossfadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeTime;

            if (currentMusic.isValid())
                currentMusic.setVolume(Mathf.Lerp(currentVol, 0f, t));

            if (nextMusic.isValid())
                nextMusic.setVolume(Mathf.Lerp(0f, 1f, t));

            yield return null;
        }

        // Stop old track
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentMusic.release();
        }

        currentMusic = nextMusic;
        currentCategory = newCategory;

        // Keep cycling in the new category
        StartCoroutine(CheckTrackEnd());

        isSwitching = false;
    }
}
