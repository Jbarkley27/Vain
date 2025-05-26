using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    public string CurrentPlanet;
    [SerializeField] private PlanetZoneUI _planetZoneUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlanetZone"))
        {
            PlanetZone planetZone = other.GetComponent<PlanetZone>();
            if (planetZone == null) return;

            CurrentPlanet = planetZone.name;

            StartCoroutine(_planetZoneUI.EnterNewZone(CurrentPlanet));
            Debug.Log($"Player entered {planetZone.name}'s zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlanetZone"))
        {
            PlanetZone planetZone = other.GetComponent<PlanetZone>();
            if (planetZone == null) return;

            CurrentPlanet = "Void";
            Debug.Log($"Player exited {planetZone.name}'s zone.");
        }
    }
}
