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

    public void LoadIntoPlanetarySystem(int i)
    {
        if (sectorManager == null || sectorManager.PlanetFormationBase == null)
        {
            Debug.LogError("SectorManager or PlanetFormationBase not found!");
            return;
        }

        Debug.Log("Initiating Planetary Entry");
        sectorManager.PlanetFormationBase.CreatePlanetarySystem(
            sectorManager.OuterSectorPlanets[i]
        );
    }
}