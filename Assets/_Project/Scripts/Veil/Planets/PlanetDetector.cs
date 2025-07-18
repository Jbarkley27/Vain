using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    public string CurrentPlanet;
    [SerializeField] private PlanetZoneUI _planetZoneUI;
    public Planet CurrentPlanetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlanetRegion"))
        {
            Debug.Log("Planet Region");
            PlanetZone planetZone = other.GetComponent<PlanetZone>();
            if (planetZone == null) return;

            CurrentPlanet = planetZone.PlanetName;
            CurrentPlanetObject = planetZone.Planet;

            StartCoroutine(_planetZoneUI.EnterNewZone(CurrentPlanet));
            Debug.Log($"Player entered {planetZone.PlanetName}'s zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlanetRegion"))
        {
            PlanetZone planetZone = other.GetComponent<PlanetZone>();
            if (planetZone == null) return;

            CurrentPlanet = "Void";
            CurrentPlanetObject = null;
            Debug.Log($"Player exited {planetZone.PlanetName}'s zone.");
        }
    }

    
}
