using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Transform minimapCamera;
    public Transform fullScreenView;
    public RectTransform mapBackground;
    public float _uiScaleFullscreen = 4;
    public float _scaleSpeed;
    public float _uiScaleMinimap = 1;
    public float _camSizeMinimap = 88;
    public float _camSizeFullscreen = 3000;
    public Camera minimapCam;
    public RectTransform maskUI;
    public GameObject playerMarker;
    public List<MinimapIcon> fullScreenOnlyElements = new List<MinimapIcon>();
    public List<MinimapIcon> miniMapOnlyElements = new List<MinimapIcon>();

    public bool isFullscreen = false;
    public static bool IsFullscreen;
    public float _camFollowSpeed = 30;
    public GameObject mapKeyUIRoot;
    public CanvasGroup mapKeyCG;

    private void Start()
    {
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
}