
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager Instance;
    [SerializeField] private CinemachineCamera cmFreeCam;

    // [Header("Shake Settings")]
    // public float amplitudeGain;
    // public float frequemcyGain;
    // public float shakeDuration;

    [System.Serializable]
    public struct ShakeProfile
    {
        public float amplitudeGain;
        public float frequencyGain;
        public float shakeDuration;

        public ShakeProfile(float amp, float fre, float dur)
        {
            this.amplitudeGain = amp;
            this.frequencyGain = fre;
            this.shakeDuration = dur;
        }
    }

    public ShakeProfile JumpProfile = new ShakeProfile(3.5f, 5, .15f);
    public ShakeProfile ShootProfile = new ShakeProfile(2f, 5, .1f);
    public ShakeProfile DashProfile = new ShakeProfile(4.5f, 5, .4f);
    public ShakeProfile DamagedProfile = new ShakeProfile(2.5f, 5, .3f);


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Screen Shake Manager object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DoShake(ShakeProfile profile)
    {
        StartCoroutine(Shake(profile));
    }

    public IEnumerator Shake(ShakeProfile profile)
    {
        cmFreeCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = profile.amplitudeGain;
        cmFreeCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().FrequencyGain = profile.frequencyGain;
        
        yield return new WaitForSeconds(profile.shakeDuration);

        cmFreeCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0;
        cmFreeCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 0;
    }
}
