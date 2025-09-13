using UnityEngine;
using System.Collections.Generic;

public class SectorManager : MonoBehaviour
{
    public enum SectorType
    {
        SECTOR_A,
        SECTOR_B,
        SECTOR_C,
        SECTOR_D,
        SECTOR_E,
    }

    public SectorType CurrentSector;

    [System.Serializable]
    public struct SectorData
    {
        public SectorType sectorType;
        public int planetCount;
        public List<Planet> possiblePlanets;
        public List<Planet> generatedPlanets;
    }



    public List<SectorData> AllSectorData = new List<SectorData>();
    public GameObject SectorBounds;
    public PlanetFormationBase PlanetFormationBase;




    void Awake()
    {
        // Hide all mesh renderes of SectorBounds children
        // leave on during development to see the sector bounds
        foreach (MeshRenderer meshRenderer in SectorBounds.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    public void CreateAllSectors()
    {
        GlobalDataStore.Instance.PlanetFormationBase.InitiateFormationFramework();
        foreach (SectorData sectorData in AllSectorData)
        {
            CreateSector(sectorData.sectorType);
        }
    }


    public void CreateSector(SectorType sectorType)
    {
        SectorData sectorData = AllSectorData.Find(s => s.sectorType == sectorType);
        if (sectorData.possiblePlanets.Count == 0)
        {
            Debug.LogWarning($"No possible planets found for sector {sectorType}");
            return;
        }

        sectorData.generatedPlanets.Clear();


        for (int i = 0; i < sectorData.planetCount; i++)
        {
            int randomIndex = Random.Range(0, sectorData.possiblePlanets.Count);
            Planet planetToAdd = sectorData.possiblePlanets[randomIndex];
            Debug.Log("Initializing Planet " + planetToAdd.gameObject.name + " for " + sectorType);
            planetToAdd.InitializePlanet(sectorType);
            sectorData.generatedPlanets.Add(planetToAdd);
            sectorData.possiblePlanets.Remove(planetToAdd);
        }

        if (sectorData.planetCount == sectorData.generatedPlanets.Count)
        {
            Debug.Log("All planets generated for " + sectorType);
        }

    }
}