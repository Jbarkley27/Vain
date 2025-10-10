using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public string Name;
    public float Size;

    public List<Event> AvailablePOIGradeCTypes;
    public List<Event> AvailablePOIGradeBTypes;
    public List<Event> AvailablePOIGradeATypes;

    [Header("Grade C POI Settings")]
    public int MaxGradeCPOICount;

    [Header("Grade B POI Settings")]
    public int MaxGradeBPOICount;

    [Header("Grade A POI Settings")]
    public int MaxGradeAPOICount;

    public Color PlanetStarfieldColor;
    public float RotationSpeed;

    public List<Vector3> GradeCSelectedPositions;
    public List<Vector3> GradeBSelectedPositions;
    public List<Vector3> GradeASelectedPositions;

    public List<Event> C_POIEvents;
    public List<Event> B_POIEvents;
    public List<Event> A_POIEvents;

    public SectorManager.SectorType sectorType;

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

    public IEnumerator UnloadPlanet()
    {
        // Add any unloading logic here (e.g., play animation, effects, etc.)
        yield return null; // Placeholder for any asynchronous operations
        Destroy(gameObject);
    }

    public void InitializePlanet(SectorManager.SectorType sectorType)
    {
        this.sectorType = sectorType;

        GradeASelectedPositions = new List<Vector3>();
        GradeBSelectedPositions = new List<Vector3>();
        GradeCSelectedPositions = new List<Vector3>();


        // Getting Available Positions for events
        List<GameObject> tempLargePOIList = new List<GameObject>(GlobalDataStore.Instance.PlanetFormationBase.largePOIList);
        List<GameObject> tempMediumPOIList = new List<GameObject>(GlobalDataStore.Instance.PlanetFormationBase.mediumPOIList);
        List<GameObject> tempSmallPOIList = new List<GameObject>(GlobalDataStore.Instance.PlanetFormationBase.smallPOIList);


        for (int i = 0; i < MaxGradeAPOICount; i++)
        {
            if (i == 0)
            {
                // First get a random position
                int randomIndex = UnityEngine.Random.Range(0, tempLargePOIList.Count);
                GameObject selectedPosition = tempLargePOIList[randomIndex];
                GradeASelectedPositions.Add(selectedPosition.transform.position);
                tempLargePOIList.RemoveAt(randomIndex);
            }
            else
            {
                GradeASelectedPositions.Add(GetFurtherstPOIPosition(tempLargePOIList.ConvertAll(p => p.transform.position), GradeASelectedPositions[GradeASelectedPositions.Count - 1]));
            }
        }

        for (int i = 0; i < MaxGradeBPOICount; i++)
        {
            if (i == 0)
            {
                // First get a random position
                int randomIndex = Random.Range(0, tempMediumPOIList.Count);
                GameObject selectedPosition = tempMediumPOIList[randomIndex];
                GradeBSelectedPositions.Add(selectedPosition.transform.position);
                tempMediumPOIList.RemoveAt(randomIndex);
            }
            else
            {
                GradeBSelectedPositions.Add(GetFurtherstPOIPosition(tempMediumPOIList.ConvertAll(p => p.transform.position), GradeBSelectedPositions[GradeBSelectedPositions.Count - 1]));
            }
        }

        for (int i = 0; i < MaxGradeCPOICount; i++)
        {
            if (i == 0)
            {
                // First get a random position
                int randomIndex = Random.Range(0, tempSmallPOIList.Count);
                GameObject selectedPosition = tempSmallPOIList[randomIndex];
                GradeCSelectedPositions.Add(selectedPosition.transform.position);
                tempSmallPOIList.RemoveAt(randomIndex);
            }
            else
            {
                GradeCSelectedPositions.Add(GetFurtherstPOIPosition(tempSmallPOIList.ConvertAll(p => p.transform.position), GradeCSelectedPositions[GradeCSelectedPositions.Count - 1]));
            }
        }





        // Select Events
        C_POIEvents = new List<Event>();
        B_POIEvents = new List<Event>();
        A_POIEvents = new List<Event>();


        List<Event> tempAvailableGradeC = new List<Event>(AvailablePOIGradeCTypes);
        List<Event> tempAvailableGradeB = new List<Event>(AvailablePOIGradeBTypes);
        List<Event> tempAvailableGradeA = new List<Event>(AvailablePOIGradeATypes);



        for (int i = 0; i < MaxGradeCPOICount; i++)
        {
            int randomIndex = Random.Range(0, tempAvailableGradeC.Count);
            if (randomIndex >= 0 || randomIndex < tempAvailableGradeC.Count && tempAvailableGradeC.Count > 0)
            {
                Event selectedEvent = tempAvailableGradeC[randomIndex];
                C_POIEvents.Add(selectedEvent);

                // create minimap marker
                // MinimapElement newMapElement = Instantiate(MapManager.Instance.planetIconElementPrefab, MapManager.Instance.sectorElementsRoot).GetComponent<MinimapElement>();

                // newMapElement.sectorType = sectorType;
                // newMapElement.gameObject.SetActive(false);
                // MapManager.Instance.AddMarkerElement(newMapElement);

                // Optionally remove the selected event to avoid duplicates
                tempAvailableGradeC.Remove(selectedEvent);
            }
        }

        for (int i = 0; i < MaxGradeBPOICount; i++)
        {
            int randomIndex = Random.Range(0, tempAvailableGradeB.Count);
            if (randomIndex >= 0 || randomIndex < tempAvailableGradeB.Count && tempAvailableGradeB.Count > 0)
            {
                Event selectedEvent = tempAvailableGradeB[randomIndex];
                B_POIEvents.Add(selectedEvent);

                // create minimap marker
                // MinimapElement newMapElement = Instantiate(MapManager.Instance.planetIconElementPrefab, MapManager.Instance.sectorElementsRoot).GetComponent<MinimapElement>();

                // newMapElement.sectorType = sectorType;
                // newMapElement.gameObject.SetActive(false);
                // MapManager.Instance.AddMarkerElement(newMapElement);

                // Optionally remove the selected event to avoid duplicates
                tempAvailableGradeB.Remove(selectedEvent);
            }
        }

        for (int i = 0; i < MaxGradeAPOICount; i++)
        {
            int randomIndex = Random.Range(0, tempAvailableGradeA.Count);
            if (randomIndex >= 0 || randomIndex < tempAvailableGradeA.Count && tempAvailableGradeA.Count > 0)
            {
                Event selectedEvent = tempAvailableGradeA[randomIndex];
                A_POIEvents.Add(selectedEvent);

                // create minimap marker
                // MinimapElement newMapElement = Instantiate(MapManager.Instance.planetIconElementPrefab, MapManager.Instance.sectorElementsRoot).GetComponent<MinimapElement>();

                // newMapElement.sectorType = sectorType;
                // newMapElement.gameObject.SetActive(false);
                // MapManager.Instance.AddMarkerElement(newMapElement);


                // Optionally remove the selected event to avoid duplicates
                tempAvailableGradeA.Remove(selectedEvent);
            }
        }


        // create UI
    }


    public Vector3 GetFurtherstPOIPosition(List<Vector3> allPositions, Vector3 startPosition)
    {
        Vector3 furthestPosition = Vector3.zero;
        float maxDistance = 0f;

        foreach (Vector3 pos in allPositions)
        {
            float distance = Vector3.Distance(startPosition, pos);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPosition = pos;
            }
        }

        return furthestPosition;
    }

}