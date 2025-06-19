using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string Name;
    [SerializeField] private List<PlanetForm> _planetForms = new List<PlanetForm>();
    [SerializeField] private PlanetZone _planetZone;
    [SerializeField] private GameObject zoneRoot;
    [SerializeField] private GameObject minimapMarker;
    public Color PlanetColor;

    public void CreatePlanet(Vector3 planetPosition)
    {
        Destroy(transform.GetChild(0).gameObject);

        int randomPlanetForm = Random.Range(0, _planetForms.Count);

        PlanetForm planetFormPrefab = Instantiate(_planetForms[randomPlanetForm], planetPosition, Quaternion.identity, transform);

        planetFormPrefab.name = Name;
        _planetZone.SetPlanetName(Name, this);

        zoneRoot.transform.position = new Vector3(planetPosition.x, 0, planetPosition.z);
        minimapMarker.transform.position = new Vector3(planetPosition.x, 0, planetPosition.z);
        // minimapMarker.transform.localScale = minimapMarker.transform.localScale * Random.Range(.6f, .9f);

        Debug.Log("Created " + Name);
    }
}