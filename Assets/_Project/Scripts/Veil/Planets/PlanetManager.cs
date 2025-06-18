using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetManager : MonoBehaviour
{
    public List<PlanetFormation> planetaryFormations = new List<PlanetFormation>();
    public List<Planet> planets = new List<Planet>();
    public GameObject portalPrefab;
    public List<GameObject> portalGameObjects = new List<GameObject>();

    public bool StartPlayerInVeil = true;

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

        // copy the available event and landmark locations from the new formation

        List<Vector3> eventLocations = newFormation.availableEventLocations.ToList();
        List<Vector3> landmarkLocations = newFormation.availableLandmarkLocations.ToList();

        // spawn 2 portals
        for (int i = 0; i < 2; i++)
        {
            Vector3 portalPosition = eventLocations[Random.Range(0, eventLocations.Count)];
            GameObject portal = Instantiate(portalPrefab, portalPosition, Quaternion.identity);
            eventLocations.Remove(portalPosition); // Remove the position to avoid duplicates
            portalGameObjects.Add(portal);
        }

        Debug.Log("Veil Creation Completed");

        // HandleWherePlayerSpawns();
    }

}