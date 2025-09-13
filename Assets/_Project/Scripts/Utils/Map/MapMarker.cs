using UnityEngine;

public class MapMarker : MonoBehaviour
{
    public enum MarkerType
    {
        Planet,
        SpaceStation,
        Anomaly,
        Other
    }

    public MarkerType markerType;
    public string markerName;
    public string description;

    void Start()
    {
        // Optionally, initialize or register the marker with a map manager here
    }

    void Update()
    {
        // Update marker position or state if necessary
    }
}