using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Transform minimapCamera;
    public Transform fullScreenView;
    public RectTransform mapBackground;
    public float _uiScaleFullscreen = 4;
    public float _scaleSpeed;
    public float _uiScaleMinimap = 1;
    public float _camSizeMinimap = 100;
    public float _camSizeFullscreen = 7000;
    public Camera minimapCam;
    public RectTransform maskUI;
    public GameObject playerMarker;
    public List<MiniMapKey> fullScreenOnlyElements = new List<MiniMapKey>();
    public List<MiniMapKey> miniMapOnlyElements = new List<MiniMapKey>();

    public bool isFullscreen = false;
    public static bool IsFullscreen;
    public float _camFollowSpeed = 30;
    public GameObject mapKeyUIRoot;
    public CanvasGroup mapKeyCG;

    public static List<MiniMapKey> miniMapKeys = new List<MiniMapKey>();
    public static List<MinimapElement> minimapElements = new List<MinimapElement>();

    [Header("Panel")]
    public GameObject panel;
    public CanvasGroup canvasCG;
    public TMP_Text titleText;
    public TMP_Text infoText;
    public MiniMapKey.MinimapIconType currentIconTypeFiler;

    [System.Serializable]
    public struct PanelKeyData
    {
        public MiniMapKey.MinimapIconType keyType;
        public string description;
    }

    public List<PanelKeyData> allKeyItems = new List<PanelKeyData>();

    private void Start()
    {
        canvasCG.alpha = 1;
        Minimize();
    }

    void Update()
    {
        if (!isFullscreen)
        {
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Time.deltaTime * _camFollowSpeed);
        }
        else
        {
            minimapCamera.transform.DOMove(fullScreenView.position, _scaleSpeed).SetEase(Ease.Linear);
        }

        IsFullscreen = isFullscreen;

        panel.SetActive(IsAnythingHovered());
        FlashMapElements();
    }



    public bool IsAnythingHovered()
    {
        for (int i = 0; i < miniMapKeys.Count; i++)
        {
            if (miniMapKeys[i].Hovered) return true;
        }

        return false;
    }


    public static void AddMiniMapKey(MiniMapKey marker)
    {
        if (!miniMapKeys.Contains(marker))
        {
            miniMapKeys.Add(marker);
        }
    }



    public void ToggleMinimapScale()
    {
        if (isFullscreen)
        {
            Minimize();
        }
        else
        {
            Maximize();
        }

        isFullscreen = !isFullscreen;

    }

    public void Minimize()
    {
        // HidePanel();
        mapKeyCG.DOFade(0, .1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            mapKeyUIRoot.SetActive(false);
        });
        maskUI.DOSizeDelta(new Vector2(_uiScaleMinimap, _uiScaleMinimap), _scaleSpeed).SetEase(Ease.InOutSine);
        mapBackground.DOSizeDelta(new Vector2(_uiScaleMinimap + 10, _uiScaleMinimap + 10), _scaleSpeed).SetEase(Ease.InOutSine);
        minimapCam.DOOrthoSize(_camSizeMinimap, _scaleSpeed).SetEase(Ease.InOutSine);
        playerMarker.transform.DOScale(.2f, _scaleSpeed).SetEase(Ease.InOutSine);
        minimapCamera.transform.DOMove(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), _scaleSpeed).SetEase(Ease.InOutSine);




        for (int i = 0; i < fullScreenOnlyElements.Count; i++)
        {
            fullScreenOnlyElements[i].iconVisual.SetActive(false);
        }

        for (int i = 0; i < miniMapOnlyElements.Count; i++)
        {
            miniMapOnlyElements[i].iconVisual.SetActive(true);
        }

    }

    public void Maximize()
    {
        // maximizing
        // HidePanel();
        maskUI.DOSizeDelta(new Vector2(_uiScaleFullscreen, _uiScaleFullscreen), _scaleSpeed).SetEase(Ease.InOutSine);
        mapBackground.DOSizeDelta(new Vector2(_uiScaleFullscreen + 10, _uiScaleFullscreen + 10), _scaleSpeed).SetEase(Ease.InOutSine);
        mapKeyUIRoot.SetActive(true);
        mapKeyCG.DOFade(.9f, .5f).SetEase(Ease.InSine);
        minimapCam.DOOrthoSize(_camSizeFullscreen, _scaleSpeed).SetEase(Ease.InOutSine);
        playerMarker.transform.DOScale(4, _scaleSpeed).SetEase(Ease.InOutSine);



        for (int i = 0; i < fullScreenOnlyElements.Count; i++)
        {
            fullScreenOnlyElements[i].iconVisual.SetActive(true);
        }

        for (int i = 0; i < miniMapOnlyElements.Count; i++)
        {
            miniMapOnlyElements[i].iconVisual.SetActive(false);
        }

    }



    public void FlashMapElements()
    {
        if (!IsAnythingHovered())
        {
            Debug.Log("Nothing Hovered");
            foreach (MinimapElement minimapElement in minimapElements)
            {
                minimapElement.canvasGroupFlasher.StopFlashing();
            }
            return;
        }
        

        foreach (MinimapElement minimapElement in minimapElements)
        {
            if (minimapElement.minimapIconType == currentIconTypeFiler)
            {
                minimapElement.canvasGroupFlasher.StartFlashing();
            }
            else
            {
                minimapElement.canvasGroupFlasher.StopFlashing();
            }
        }
    }


    public void UpdatePanelData(MiniMapKey.MinimapIconType keyType)
    {
        Debug.Log("Showing Panel");
        currentIconTypeFiler = keyType;

        PanelKeyData panelKeyData = allKeyItems.Find(item => item.keyType == keyType);

        titleText.text = panelKeyData.keyType.ToString();
        infoText.text = panelKeyData.description;
    }

    public void HidePanel()
    {
        Debug.Log("Hiding Panel");
        foreach (MinimapElement minimapElement in minimapElements)
        {
            minimapElement.canvasGroupFlasher.StopFlashing();
        }

        titleText.text = "";
        infoText.text = "";
    }
}