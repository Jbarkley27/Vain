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

    [Header("Outer Sector")]
    public int OuterSectorPlanetCount = 6;
    public List<PlanetFormation> SectorOuterPossiblePlanetFormations = new List<PlanetFormation>();
    public List<Planet> OuterSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormation> GeneratedOuterSectorPlanetFormations = new List<PlanetFormation>();



    [Header("Middle Sector")]
    public int MiddleSectorPlanetCount = 7;
    public List<PlanetFormation> SectorMiddlePossiblePlanetFormations = new List<PlanetFormation>();
    public List<Planet> MiddleSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormation> GeneratedMiddleSectorPlanetFormations = new List<PlanetFormation>();



    [Header("Inner Sector")]
    public int InnerSectorPlanetCount = 8;
    public List<PlanetFormation> SectorInnerPossiblePlanetFormations = new List<PlanetFormation>();
    public List<Planet> InnerSectorPossiblePlanets = new List<Planet>();
    public List<PlanetFormation> GeneratedInnerSectorPlanetFormations = new List<PlanetFormation>();


    [Header("Center Sector")]
    public int CenterSectorPlanetCount = 1;

    


    void Start()
    {
        CreateOuterSector();
    }

    public void CreateOuterSector()
    {
        CurrentSector = SectorType.Outer;

        Debug.Log("Creating Outer Sector");

        // Get random planet formations for the outer sector
        for (int i = 0; i < OuterSectorPlanetCount; i++)
        {
            PlanetFormation randomFormation = SectorOuterPossiblePlanetFormations[Random.Range(0, SectorOuterPossiblePlanetFormations.Count)];

            Planet newRandomPlanet = OuterSectorPossiblePlanets[Random.Range(0, OuterSectorPossiblePlanets.Count)];
            OuterSectorPossiblePlanets.Remove(newRandomPlanet);

            GeneratedOuterSectorPlanetFormations.Add(Instantiate(randomFormation, Vector3.zero, Quaternion.identity));

            randomFormation.SetupFormation(newRandomPlanet);

            // after adding we need to remove it from the possible list to avoid duplicates
            SectorOuterPossiblePlanetFormations.Remove(randomFormation);
        }
    }

    public void CreateMiddleSector()
    {
        CurrentSector = SectorType.Middle;

        Debug.Log("Creating Middle Sector");

        CurrentSector = SectorType.Outer;

        Debug.Log("Creating Outer Sector");

        // Get random planet formations for the outer sector
        for (int i = 0; i < OuterSectorPlanetCount; i++)
        {
            PlanetFormation randomFormation = SectorOuterPossiblePlanetFormations[Random.Range(0, SectorOuterPossiblePlanetFormations.Count)];

            Planet newRandomPlanet = OuterSectorPossiblePlanets[Random.Range(0, OuterSectorPossiblePlanets.Count)];
            OuterSectorPossiblePlanets.Remove(newRandomPlanet);

            GeneratedOuterSectorPlanetFormations.Add(Instantiate(randomFormation, Vector3.zero, Quaternion.identity));

            randomFormation.SetupFormation(newRandomPlanet);

            // after adding we need to remove it from the possible list to avoid duplicates
            SectorOuterPossiblePlanetFormations.Remove(randomFormation);
        }
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