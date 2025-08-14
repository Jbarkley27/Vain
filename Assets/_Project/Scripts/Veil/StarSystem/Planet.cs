using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string Name;
    public float Size;

    public List<POIState.POIGradeCType> AvailablePOIGradeCTypes;
    public List<POIState.POIGradeBType> AvailablePOIGradeBTypes;
    public List<POIState.POIGradeAType> AvailablePOIGradeATypes;

    [Header("Grade C POI Settings")]
    public int MaxGradeCPOICount;

    [Header("Grade B POI Settings")]
    public int MaxGradeBPOICount;

    [Header("Grade A POI Settings")]
    public int MaxGradeAPOICount;

    public Color PlanetStarfieldColor;
    public float RotationSpeed;

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

}