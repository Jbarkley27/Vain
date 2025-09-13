using UnityEngine;

public class Event : MonoBehaviour
{
    public string EventName;
    public string Description;
    public POIState.POIGradeCType EventType;
    public bool IsCompleted = false;
    public bool IsActive = false;

    public void TriggerEvent()
    {
        Debug.Log($"Event Triggered: {EventName} - {Description}");
        // Add event-specific logic here
    }

    public void SaveEventData()
    {
        Debug.Log($"Event Data Saved for: {EventName}");
        // Add event data saving logic here
    }

    public void CompleteEvent()
    {
        Debug.Log($"Event Completed: {EventName}");
        // Add event completion logic here
    }

    // on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsCompleted && !IsActive)
        {
            TriggerEvent();
            // Optionally mark as completed immediately or after some conditions
            // IsCompleted = true;
        }
    }
}