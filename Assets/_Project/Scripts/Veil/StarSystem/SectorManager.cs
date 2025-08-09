using UnityEngine;
using System.Collections.Generic;

public class SectorManager : MonoBehaviour
{
    public enum SectorType
    {
        Center,
        Inner,
        Middle,
        Outer
    }

    public SectorType CurrentSector;
    public GameObject SectorBounds;

    public PlanetFormationBase PlanetFormationBase;

    [Header("Outer Sector")]
    public int OuterSectorPlanetCount = 6;
    public List<PlanetFormationData> SectorOuterPossiblePlanetFormationDatas = new List<PlanetFormationData>();
    public List<Planet> OuterSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormationData> GeneratedOuterSectorPlanetFormationDatas = new List<PlanetFormationData>();



    [Header("Middle Sector")]
    public int MiddleSectorPlanetCount = 7;
    public List<PlanetFormationData> SectorMiddlePossiblePlanetFormationDatas = new List<PlanetFormationData>();
    public List<Planet> MiddleSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormationData> GeneratedMiddleSectorPlanetFormationDatas = new List<PlanetFormationData>();



    [Header("Inner Sector")]
    public int InnerSectorPlanetCount = 8;
    public List<PlanetFormationData> SectorInnerPossiblePlanetFormationDatas = new List<PlanetFormationData>();
    public List<Planet> InnerSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormationData> GeneratedInnerSectorPlanetFormationDatas = new List<PlanetFormationData>();


    [Header("Center Sector")]
    public int CenterSectorPlanetCount = 1;




    void Start()
    {
        CreateOuterSector();

        // Hide all mesh renderes of SectorBounds children
        // leave on during development to see the sector bounds
        foreach (MeshRenderer meshRenderer in SectorBounds.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    public void CreateOuterSector()
    {
        CurrentSector = SectorType.Outer;

        Debug.Log("Creating Outer Sector");

        // Get random planet formations for the outer sector
        for (int i = 0; i < OuterSectorPlanetCount; i++)
        {

            // Get a random formation from the possible formations list
            PlanetFormationData randomFormation = SectorOuterPossiblePlanetFormationDatas[Random.Range(0, SectorOuterPossiblePlanetFormationDatas.Count)];
            GeneratedOuterSectorPlanetFormationDatas.Add(Instantiate(randomFormation, Vector3.zero, Quaternion.identity));


            // Get a random planet from the possible planets list
            SectorOuterPossiblePlanetFormationDatas.Remove(randomFormation);
            Planet newRandomPlanet = OuterSectorPossiblePlanets[Random.Range(0, OuterSectorPossiblePlanets.Count)];
            OuterSectorPossiblePlanets.Remove(newRandomPlanet);


            // setup formation base
            PlanetFormationBase.CreateFormation(newRandomPlanet, randomFormation);
        }
    }

    public void CreateMiddleSector()
    {
        CurrentSector = SectorType.Middle;

        Debug.Log("Creating Middle Sector");

        // Logic to create the middle sector can be added here
        // This could involve instantiating planet formations and setting up planets similar to the outer sector.
    }


    public void CreateInnerSector()
    {
        CurrentSector = SectorType.Inner;

        Debug.Log("Creating Inner Sector");

        // Logic to create the inner sector can be added here
        // This could involve instantiating planet formations and setting up planets similar to the outer sector.
    }
    

    public void CreateCenterSector()
    {
        CurrentSector = SectorType.Center;

        Debug.Log("Creating Center Sector");

        // Logic to create the center sector can be added here
        // This could involve instantiating planet formations and setting up planets similar to the outer sector.
    }
}