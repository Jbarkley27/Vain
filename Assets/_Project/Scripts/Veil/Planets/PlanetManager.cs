using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public List<PlanetFormation> planetaryFormations = new List<PlanetFormation>();
    public List<Planet> planets = new List<Planet>();

    void Start()
    {
        CreateVeil();
    }

    public void CreateVeil()
    {
        Debug.Log("Creating Veil");

        PlanetFormation newFormation = planetaryFormations[Random.Range(0, planetaryFormations.Count)];

        for (int i = 0; i < planets.Count; i++)
        {
            planets[i].CreatePlanet(newFormation.formation[i]);
        }

        Debug.Log("Veil Creation Completed");
    }
}