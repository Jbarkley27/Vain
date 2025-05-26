using UnityEngine;

public class PlanetForm : MonoBehaviour
{
    public float orbitalRate; // how fast the planet will spin

    void Start()
    {
        orbitalRate = Random.Range(orbitalRate, orbitalRate + 2);
    }

    void Update()
    {
        transform.Rotate(0f, orbitalRate * Time.deltaTime, 0f);
    }

}