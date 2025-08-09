using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpaceTimeSystem : MonoBehaviour
{
    [Header("Time Settings")]
    public int minutesPerDay = 100;      // How many "minutes" in a day
    public int daysPerYear = 364;        // How many days in a year
    public float realSecondsPerMinute = 1f; // Base speed of a minute in real seconds

    [Header("Current Time")]
    public int currentMinute = 0;
    public int currentDay = 1;
    public int currentYear = 1;

    [Header("Runtime Controls")]
    public bool isPaused = false;
    [Range(0.1f, 100f)]
    public float timeSpeedMultiplier = 1f; // 1 = normal, 2 = twice as fast, 0.5 = half speed

    private float minuteTimer = 0f;


    [Header("UI")]
    // public Slider daySlider;
    public TMP_Text timeText;

    public

    void Update()
    {
        // Ensure clamped values in case they're edited at runtime
        ClampTimeValues();

        // Skip if paused
        if (isPaused) return;

        // Update UI
        // if (daySlider)
        //     daySlider.value = GetDayProgress();
        if (timeText)
            timeText.text = GetTimeString();    

        // Count real time (scaled by multiplier)
        minuteTimer += Time.deltaTime * timeSpeedMultiplier;

        if (minuteTimer >= realSecondsPerMinute)
        {
            minuteTimer = 0f;
            AdvanceMinute();
        }
    }

    // --- Core Time Advancement ---
    void AdvanceMinute()
    {
        currentMinute++;

        if (currentMinute >= minutesPerDay)
        {
            currentMinute = 0;
            currentDay++;

            if (currentDay > daysPerYear)
            {
                currentDay = 1;
                currentYear++;
            }
        }
    }

    // --- Start Conditions ---
    public void StartAtZero()
    {
        currentMinute = 0;
        currentDay = 0;
        currentYear = 1;
        minuteTimer = 0f;
    }

    public void StartAtRandom()
    {
        currentMinute = Random.Range(0, minutesPerDay);
        currentDay = Random.Range(1, daysPerYear + 1);
        currentYear = 1;
        minuteTimer = 0f;
    }

    public void StartAtSpecific(int year, int day, int minute)
    {
        currentYear = Mathf.Max(1, year);
        currentDay = day;
        currentMinute = minute;
        minuteTimer = 0f;
        ClampTimeValues();
    }

    // --- Reset Function ---
    public void ResetTime()
    {
        StartAtZero();
    }

    // --- Manual Time Change ---
    public void SetTime(int year, int day, int minute)
    {
        StartAtSpecific(year, day, minute);
    }

    // --- Pause & Speed Control ---
    public void PauseTime() => isPaused = true;
    public void ResumeTime() => isPaused = false;
    public void TogglePause() => isPaused = !isPaused;
    public void SetSpeedMultiplier(float multiplier) => timeSpeedMultiplier = Mathf.Max(0.1f, multiplier);

    // --- Utility ---
    void ClampTimeValues()
    {
        currentMinute = Mathf.Clamp(currentMinute, 0, minutesPerDay - 1);
        currentDay = Mathf.Clamp(currentDay, 1, daysPerYear);
        currentYear = Mathf.Max(1, currentYear);
    }

    // --- HUD Helpers ---
    public string GetTimeString()
    {
        return $"VEIL | {currentYear}:{currentDay}:{currentMinute} | {GlobalDataStore.Instance.PlanetDetector.CurrentPlanet}";
    }

    public float GetDayProgress()
    {
        return (float)currentMinute / minutesPerDay;
    }

    public float GetYearProgress()
    {
        float totalMinutesPassed = ((currentDay - 1) * minutesPerDay) + currentMinute;
        float totalMinutesInYear = daysPerYear * minutesPerDay;
        return totalMinutesPassed / totalMinutesInYear;
    }
}
