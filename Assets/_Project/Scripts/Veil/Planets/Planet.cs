using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string Name;
    [SerializeField] private List<PlanetForm> _planetForms = new List<PlanetForm>();
    [SerializeField] private PlanetZone _planetZone;

    public void CreatePlanet(Vector3 planetPosition)
    {
        Destroy(transform.GetChild(0).gameObject);

        int randomPlanetForm = Random.Range(0, _planetForms.Count);

        PlanetForm planetFormPrefab = Instantiate(_planetForms[randomPlanetForm], planetPosition, Quaternion.identity, transform);

        planetFormPrefab.name = Name;
        _planetZone.SetPlanetName(Name);

        _planetZone.transform.position = planetPosition;
        _planetZone.transform.localScale = new Vector3(Random.Range(500, 600), Random.Range(10, 20), Random.Range(500, 600));

        Debug.Log("Created " + Name);
    }
}