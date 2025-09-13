using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;


    public enum MapState { MINIMIZED, MAXIMIZED };
    public MapState mapState;


    public GameObject mapUIRoot;
    public CanvasGroup mapRootCG;
    public float _uiScaleMinimized = 1;
    public float _uiScaleMaximized = 4;
    public float _scaleSpeed = 1;
    public List<CanvasGroup> planetIcons = new List<CanvasGroup>();
    public Color minimizedIconColor = Color.white;
    public Color maximizedIconColor = Color.grey;
    public Image imageMinimapBackground;
    public TMP_Text timeText;
    public List<MinimapElement> SectorMapElements = new List<MinimapElement>();
    public List<MinimapElement> A_SubSectorPlanetMapElements = new List<MinimapElement>();
    public List<MinimapElement> B_SubSectorPlanetMapElements = new List<MinimapElement>();
    public List<MinimapElement> C_SubSectorPlanetMapElements = new List<MinimapElement>();
    public List<MinimapElement> D_SubSectorPlanetMapElements = new List<MinimapElement>();
    public List<MinimapElement> E_SubSectorPlanetMapElements = new List<MinimapElement>();
    public List<GameObject> planetAvailablePositions = new List<GameObject>();
    public Transform sectorElementsRoot;
    public GameObject planetIconElementPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Map Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Minimize();
    }

    public void Minimize()
    {
        mapUIRoot.transform.DOScale(_uiScaleMinimized, _scaleSpeed).SetEase(Ease.InOutSine);
        mapRootCG.DOFade(0.5f, _scaleSpeed)
        .SetEase(Ease.InOutSine);
        mapState = MapState.MINIMIZED;
        imageMinimapBackground.color = minimizedIconColor;
        timeText.gameObject.SetActive(false);

        foreach (MinimapElement element in SectorMapElements)
        {
            element.gameObject.SetActive(false);
        }
    }

    public void Maximize()
    {
        mapUIRoot.transform.DOScale(_uiScaleMaximized, _scaleSpeed).SetEase(Ease.InOutSine);
        mapRootCG.DOFade(1f, _scaleSpeed).SetEase(Ease.InOutSine);
        mapState = MapState.MAXIMIZED;
        imageMinimapBackground.color = maximizedIconColor;
        timeText.gameObject.SetActive(true);

        foreach (MinimapElement element in SectorMapElements)
        {
            element.gameObject.SetActive(true);
        }
    }

    public void ToggleMinimapScale()
    {
        if (mapState == MapState.MINIMIZED)
        {
            Maximize();
        }
        else
        {
            Minimize();
        }
    }

    public void CreateMap()
    {
        foreach (CanvasGroup icon in planetIcons)
        {
            icon.gameObject.transform.localScale = Vector3.one * Random.Range(.7f, 1f);
            icon.alpha = 0;
        }

        // connect map ui to sector
        int index = 0;
        foreach (SectorManager.SectorData sectorData in GlobalDataStore.Instance.SectorManager.AllSectorData)
        {
            SectorMapElements[index].sectorType = sectorData.sectorType;
            index++;
        }
    }


    public void MinimizeElement(CanvasGroup CG)
    {
        CG.DOFade(0, _scaleSpeed)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            CG.gameObject.SetActive(false);
        });
    }

    public void HideSectorUI()
    {
        foreach (MinimapElement minimapElement in SectorMapElements)
        {
            minimapElement.gameObject.SetActive(false);
        }
    }

    public void ShowSectorUI()
    {
        foreach (MinimapElement minimapElement in SectorMapElements)
        {
            minimapElement.gameObject.SetActive(true);
        }
    }


    public void LoadPlanetUI()
    {
        HideSectorUI();

        List<MinimapElement> tempList = new List<MinimapElement>();
        List<GameObject> tempPosList = new List<GameObject>(planetAvailablePositions);

        switch (GlobalDataStore.Instance.SectorUIPanel.CurrentData.sectorType)
        {
            case SectorManager.SectorType.SECTOR_A:
                tempList = A_SubSectorPlanetMapElements;
                break;
            case SectorManager.SectorType.SECTOR_B:
                tempList = B_SubSectorPlanetMapElements;
                break;
            case SectorManager.SectorType.SECTOR_C:
                tempList = C_SubSectorPlanetMapElements;
                break;
            case SectorManager.SectorType.SECTOR_D:
                tempList = D_SubSectorPlanetMapElements;
                break;
            case SectorManager.SectorType.SECTOR_E:
                tempList = E_SubSectorPlanetMapElements;
                break;
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i].gameObject.transform.position = tempPosList[i].transform.position;
            tempList[i].gameObject.SetActive(true);
        }
    }





    // UI Mapping Tools
    public Vector2 worldMin = new Vector2(-50, -50);
    public Vector2 worldMax = new Vector2(50, 50);
    public RectTransform mapContainerRect;

    public void MapWorldToUI(Transform worldObj, RectTransform uiMarker,
                          RectTransform canvasRect)
    {
        Vector3 worldPos = worldObj.position;

        float normX = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float normY = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z);

        Vector2 canvasSize = canvasRect.rect.size;

        float uiX = (normX * canvasSize.x) - (canvasSize.x / 2f);
        float uiY = (normY * canvasSize.y) - (canvasSize.y / 2f);

        uiMarker.localPosition = new Vector2(uiX, uiY);
    }

    public void AddMarkerElement(MinimapElement minimapElement)
    {
        switch (minimapElement.sectorType)
        {
            case SectorManager.SectorType.SECTOR_A:
                A_SubSectorPlanetMapElements.Add(minimapElement);
                break;
            case SectorManager.SectorType.SECTOR_B:
                B_SubSectorPlanetMapElements.Add(minimapElement);
                break;
            case SectorManager.SectorType.SECTOR_C:
                C_SubSectorPlanetMapElements.Add(minimapElement);
                break;
            case SectorManager.SectorType.SECTOR_D:
                D_SubSectorPlanetMapElements.Add(minimapElement);
                break;
            case SectorManager.SectorType.SECTOR_E:
                E_SubSectorPlanetMapElements.Add(minimapElement);
                break;
        }
    }
}
