using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    public string CurrentPlanet;
    [SerializeField] private PlanetZoneUI _planetZoneUI;
    public Planet CurrentPlanetObject;
    public StarfieldColorManager starfieldColorManager;
    public string DefaultPlanetName = "Void"; // Default planet name when not in a specific zone

    void Start()
    {
        CurrentPlanet = DefaultPlanetName;
        CurrentPlanetObject = null;
    }

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


        if (other.CompareTag("PlanetColorChange"))
        {
            PlanetColorZone planetZoneColorTheme = other.GetComponent<PlanetColorZone>();
            if (planetZoneColorTheme == null) return;

            starfieldColorManager.ChangeColor(planetZoneColorTheme.planetColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlanetRegion"))
        {
            PlanetZone planetZone = other.GetComponent<PlanetZone>();
            if (planetZone == null) return;

            CurrentPlanet = DefaultPlanetName;
            CurrentPlanetObject = null;
            Debug.Log($"Player exited {planetZone.PlanetName}'s zone.");
        }


        if (other.CompareTag("PlanetColorChange"))
        {
            starfieldColorManager.ChangeColor(starfieldColorManager.defaultVoidColor, true);
        }
    }

    
}
