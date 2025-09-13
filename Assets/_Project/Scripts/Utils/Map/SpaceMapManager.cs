using System.Collections.Generic;
using UnityEngine;

public class SpaceMapManager : MonoBehaviour
{
    public static SpaceMapManager Instance;
    public List<POIState> ActivePOIs = new List<POIState>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Map Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } 
    
    public void RegisterPOI(POIState poi)
    {
        if (!ActivePOIs.Contains(poi))
        {
            ActivePOIs.Add(poi);
        }
    }
}