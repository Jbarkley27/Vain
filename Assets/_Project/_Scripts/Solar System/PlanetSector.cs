using UnityEngine;

public class PlanetSector : MonoBehaviour
{
    public GameObject planetBounds;
    public string PlanetName;
    public Color PlanetStarfieldColor = Color.black;


    void Start()
    {
        planetBounds.SetActive(false);
    }
}
