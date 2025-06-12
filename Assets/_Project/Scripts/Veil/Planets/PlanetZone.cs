using UnityEngine;

public class PlanetZone : MonoBehaviour
{
    public string PlanetName;
    public Planet Planet;

    public void SetPlanetName(string name, Planet planet)
    {
        PlanetName = name;
        Planet = planet;
    }
}
