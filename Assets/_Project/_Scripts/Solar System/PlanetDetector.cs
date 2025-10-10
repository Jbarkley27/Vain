using System;
using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    public string CurrentPlanet;
    [SerializeField] private PlanetZoneUI _planetZoneUI;
    public PlanetSector CurrentPlanetSector;
    public StarfieldColorManager starfieldColorManager;
    public string DefaultPlanetName = "Void"; // Default planet name when not in a specific zone


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlanetSector"))
        {
            PlanetSector planetSector = other.GetComponent<PlanetSector>();
            if (planetSector == null) return;

            CurrentPlanetSector = planetSector;
            CurrentPlanet = planetSector.PlanetName;

            _planetZoneUI.EnterNewZone(planetSector.PlanetName);
            Debug.Log($"Player entered {planetSector.PlanetName}'s zone.");


            if (planetSector.PlanetStarfieldColor == null) return;

            starfieldColorManager.ChangeColor(planetSector.PlanetStarfieldColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlanetSector"))
        {
            PlanetSector planetSector = other.GetComponent<PlanetSector>();
            if (planetSector == null) return;


            _planetZoneUI.EnterNewZone(starfieldColorManager.OrbitName, false);

            CurrentPlanetSector = null;
            CurrentPlanet = DefaultPlanetName;
            Debug.Log($"Player exited {planetSector.PlanetName}'s zone.");

            starfieldColorManager.ChangeColor(starfieldColorManager.defaultVoidColor, true);
        }
    }

    
}
