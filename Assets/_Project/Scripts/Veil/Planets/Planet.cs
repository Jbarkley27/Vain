using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public string Name;
    [SerializeField] private List<PlanetForm> _planetForms = new List<PlanetForm>();
    [SerializeField] private PlanetZone _planetZone;
    [SerializeField] private GameObject zoneRoot;
    [SerializeField] private GameObject minimapMarker;

    public void CreatePlanet(Vector3 planetPosition)
    {
        // remove the debug planet object
        Destroy(transform.GetChild(0).gameObject);

        int randomPlanetForm = Random.Range(0, _planetForms.Count);

        PlanetForm planetFormPrefab = Instantiate(_planetForms[randomPlanetForm], planetPosition, Quaternion.identity, transform);

        // setup planet
        planetFormPrefab.name = Name;
        _planetZone.SetPlanetName(Name, this);

        zoneRoot.transform.position = new Vector3(planetPosition.x, 0, planetPosition.z);
        minimapMarker.transform.position = new Vector3(planetPosition.x, 0, planetPosition.z);
        // minimapMarker.transform.localScale = minimapMarker.transform.localScale * Random.Range(.6f, .9f);

        Debug.Log("Created " + Name);
    }
}