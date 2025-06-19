using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetManager : MonoBehaviour
{
    // Planet Formations
    [System.Serializable]
    public struct PlanetFormation
    {
        public Transform ParentPlacementParent;
        public Transform SmallPOIListParent;
        public Transform MediumPOIListParent;
        public Transform LargePOIListParent;
        public List<GameObject> planetFormation;
        public List<GameObject> smallPOIList;
        public List<GameObject> mediumPOIList;
        public List<GameObject> largePOIList;
    }

    public List<PlanetFormation> planetaryFormations = new List<PlanetFormation>();
    public List<Planet> planets = new List<Planet>();
    public Vector3 PlayerSpawnPosition = new Vector3(-2500, 0, -70);



    void Start()
    {
        CreateVeil();
    }



    public void CreateVeil()
    {
        Debug.Log("Creating Veil");

        PlanetFormation newFormation = planetaryFormations[Random.Range(0, planetaryFormations.Count)];

        // Get All Locations of POI from Selected Formation
        foreach (Transform poi in newFormation.ParentPlacementParent)
        {
            newFormation.planetFormation.Add(poi.gameObject);
        }

        foreach (Transform poi in newFormation.SmallPOIListParent)
        {
            poi.AddComponent<POIState>();
            newFormation.smallPOIList.Add(poi.gameObject);
        }
        
        foreach (Transform poi in newFormation.MediumPOIListParent)
        {
            poi.AddComponent<POIState>();
            newFormation.mediumPOIList.Add(poi.gameObject);
        }

        foreach (Transform poi in newFormation.LargePOIListParent)
        {
            poi.AddComponent<POIState>();
            newFormation.largePOIList.Add(poi.gameObject);
        }




        for (int i = 0; i < planets.Count; i++)
        {
            planets[i].CreatePlanet(newFormation.planetFormation[i].transform.position);
        }

        // copy the available event and landmark locations from the new formation

        // List<Vector3> landmarkLocations = newFormation.availableLandmarkLocations.ToList();



        // GlobalDataStore.Instance.Player.GetComponent<Rigidbody>().position = PlayerSpawnPosition;

        Debug.Log("Veil Creation Completed");
    }

}