using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetManager : MonoBehaviour
{
    // // Planet Formations
    // [System.Serializable]
    // public struct PlanetFormation
    // {
    //     public Transform ParentPlacementParent;
    //     public Transform SmallPOIListParent;
    //     public Transform MediumPOIListParent;
    //     public Transform LargePOIListParent;
    //     public List<GameObject> planetFormation;
    //     public List<GameObject> smallPOIList;
    //     public List<GameObject> mediumPOIList;
    //     public List<GameObject> largePOIList;
    // }

    public List<PlanetFormation> planetaryFormations = new List<PlanetFormation>();
    public List<Planet> planets = new List<Planet>();
    // public Vector3 PlayerSpawnPosition = new Vector3(-2500, 0, -70);



    void Start()
    {
        CreateVeil();
    }



    public void CreateVeil()
    {
        Debug.Log("Creating Veil");

        PlanetFormation newFormation = Instantiate(planetaryFormations[Random.Range(0, planetaryFormations.Count)]
            , Vector3.zero, Quaternion.identity);

        newFormation.SetupFormation();

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




        GlobalDataStore.Instance.Player.GetComponent<Rigidbody>().position = newFormation.PlayerSpawnPosition.transform.position;

        GlobalDataStore.Instance.SpaceTimeSystem.StartAtZero();

        Debug.Log("Veil Creation Completed");
    }

}