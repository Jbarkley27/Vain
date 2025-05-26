using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetFormation", menuName = "Planet/Formation")]
public class PlanetFormation : ScriptableObject
{
    public List<Vector3> formation = new List<Vector3>();
    public List<Vector3> availableEventLocations = new List<Vector3>();
    public List<Vector3> availableLandmarkLocations = new List<Vector3>();
}