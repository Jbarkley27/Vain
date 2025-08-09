using UnityEngine;
using System.Collections.Generic;

public class PlanetFormation : MonoBehaviour
{
    public Transform ParentPlacementParent;
    public Transform SmallPOIListParent;
    public Transform MediumPOIListParent;
    public Transform LargePOIListParent;
    public List<GameObject> planetFormation;
    public List<GameObject> smallPOIList;
    public List<GameObject> mediumPOIList;
    public List<GameObject> largePOIList;
    public Transform PlayerSpawnPosition;

    public void SetupFormation()
    {
        planetFormation = new List<GameObject>();
        smallPOIList = new List<GameObject>();
        mediumPOIList = new List<GameObject>();
        largePOIList = new List<GameObject>();

        // Populate the lists with the children of the respective parent transforms
        foreach (Transform poi in ParentPlacementParent)
        {
            planetFormation.Add(poi.gameObject);
        }

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
    }
}