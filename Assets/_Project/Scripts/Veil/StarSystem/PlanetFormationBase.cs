using UnityEngine;
using System.Collections.Generic;

public class PlanetFormationBase : MonoBehaviour
{
    public Transform PlanetPlacement;
    public Transform SmallPOIListParent;
    public Transform MediumPOIListParent;
    public Transform LargePOIListParent;
    public List<GameObject> smallPOIList;
    public List<GameObject> mediumPOIList;
    public List<GameObject> largePOIList;
    public Transform PlayerSpawnPosition;
    public Planet Planet = null;


    void Start()
    {
        InitiateFormationFramework();
    }


    public void ClearFormation()
    {
        if (PlanetPlacement.childCount > 0) Destroy(PlanetPlacement.GetChild(0).gameObject);
        Planet = null;
    }


    public void CreatePlanetarySystem(Planet planet)
    {
        Debug.Log($"Creating Planetary System for {planet.Name}");
        // Clear previous formation
        ClearFormation();

        Planet newPlanet = Instantiate(planet, PlanetPlacement.transform);

        // update planet formation position so that planet is at the center relative to its size
        Vector3 offset = new Vector3(newPlanet.Size, -newPlanet.Size, newPlanet.Size);
        PlanetPlacement.position = offset;

        // Set the planet reference
        Planet = planet;

        // Set the planet's position to the placement position
        if (PlanetPlacement != null)
        {
            planet.transform.position = PlanetPlacement.position;
        }

        GlobalDataStore.Instance.StarfieldColorManager.ChangeColor(planet.PlanetStarfieldColor);
    }


    public void InitiateFormationFramework()
    {
        smallPOIList = new List<GameObject>();
        mediumPOIList = new List<GameObject>();
        largePOIList = new List<GameObject>();

        // Populate the lists with the children of the respective parent transforms
        foreach (Transform poi in SmallPOIListParent)
        {
            poi.gameObject.AddComponent<POIState>();
            smallPOIList.Add(poi.gameObject);
        }

        foreach (Transform poi in MediumPOIListParent)
        {
            poi.gameObject.AddComponent<POIState>();
            mediumPOIList.Add(poi.gameObject);
        }

        foreach (Transform poi in LargePOIListParent)
        {
            poi.gameObject.AddComponent<POIState>();
            largePOIList.Add(poi.gameObject);
        }

        // ClearFormation();
    }
}