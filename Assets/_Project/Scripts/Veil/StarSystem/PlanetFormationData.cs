using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetFormationData", menuName = "New PlanetFormationData", order = 1)]
public class PlanetFormationData : ScriptableObject
{
    public string FormationName;
    public SectorManager.SectorType SectorType;
    public List<POIState.POIGradeCType> AvailablePOIGradeCTypes;
    public List<POIState.POIGradeBType> AvailablePOIGradeBTypes;
    public List<POIState.POIGradeAType> AvailablePOIGradeATypes;

    [Header("Grade C POI Settings")]
    public int MaxGradeCPOICount;

    [Header("Grade B POI Settings")]
    public int MaxGradeBPOICount;

    [Header("Grade A POI Settings")]
    public int MaxGradeAPOICount;
}