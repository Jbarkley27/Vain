using UnityEngine;

public class StarSystemManager : MonoBehaviour
{
    public SectorManager sectorManager;

    void Start()
    {
        CreateVeil();
    }

    public void CreateVeil()
    {
        Debug.Log("Creating Veil");

        // Initialize SectorManager
        if (sectorManager == null)
        {
            Debug.LogError("SectorManager not found in the scene!");
            return;
        }

        sectorManager.CreateOuterSector();

        GlobalDataStore.Instance.SpaceTimeSystem.StartAtZero();

        Debug.Log("Veil Creation Completed");
    }


    public void CreateNextSector()
    {
        // Logic to create the next sector can be added here
        Debug.Log("Creating Next Sector");
        
        // Example: You might want to call sectorManager.CreateOuterSector() or similar methods
        // based on the current sector type or other conditions.
    }

}