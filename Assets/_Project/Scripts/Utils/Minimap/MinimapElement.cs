using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class MinimapElement : MonoBehaviour
{
    // public Image icon;
    public CanvasGroup canvasGroup;
    public bool cursorHovering = false;
    public float hoverAlpha = .9f;
    public float normalAlpha = .4f;
    public CanvasGroupFlasher flasher;
    public SectorManager.SectorType sectorType;
    public bool Hovered = false;

    void Start()
    {
        canvasGroup.alpha = normalAlpha;
        flasher = GetComponent<CanvasGroupFlasher>();
    }


    public void Update()
    {
        if (MapManager.Instance.mapState == MapManager.MapState.MINIMIZED) return;

        if (cursorHovering)
        {
            // We need to show the panel for the first time
            if (!Hovered)
            {
                Hovered = true;
                StartCoroutine(GlobalDataStore.Instance.SectorUIPanel.ShowPanel(
                    new SectorUIPanel.SectorUIPanelData(
                        sectorType.ToString(),
                        sectorType
                    )
                ));
                flasher.StartFlashing();
            }
        }
        else if (!cursorHovering)
        {
            // hide panel for thie first time
            if (Hovered)
            {
                GlobalDataStore.Instance.SectorUIPanel.HidePanel();
                Hovered = false;
                flasher.StopFlashing(normalAlpha);
            }
        }
    }


    public void OnDisable()
    {
        if (flasher && flasher.IsFlashing) flasher.StopFlashing();
        cursorHovering = false;
        gameObject.transform.DOScale(Vector3.zero, Random.Range(.1f, .2f))
        .SetEase(Ease.OutSine);
    }

    public void Onable()
    {
        gameObject.transform.DOScale(Vector3.one, Random.Range(.1f, .2f))
        .SetEase(Ease.OutSine);
        canvasGroup.DOFade(normalAlpha, Random.Range(.1f, .2f));
    }

}