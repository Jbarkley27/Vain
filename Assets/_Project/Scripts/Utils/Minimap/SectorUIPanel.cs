using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class SectorUIPanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float hoverAlpha = .9f;
    public float normalAlpha = .4f;
    public bool isVisible = false;
    public List<CanvasGroup> panelElements = new List<CanvasGroup>();
    public TMP_Text sectorName;
    public Vector3 panelCursorOffset = new Vector3(40, 0, 0);
    public GameObject root;
    public SectorUIPanelData CurrentData;

    public struct SectorUIPanelData
    {
        public string sectorName;
        public SectorManager.SectorType sectorType;

        public SectorUIPanelData(string sectorName, SectorManager.SectorType sectorType)
        {
            this.sectorName = sectorName;
            this.sectorType = sectorType;
        }
    }

    void Start()
    {
        // foreach (Transform chld in transform)
        // {
        //     panelElements.Add(chld.GetComponent<CanvasGroup>());
        // }

        HidePanel();
    }


    void Update()
    {
        if (isActiveAndEnabled)
        {
            gameObject.transform.position = WorldCursor.instance.GetCursorPosition() + panelCursorOffset;
        }
    }



    public IEnumerator ShowPanel(SectorUIPanelData sectorUIPanelData)
    {
        if (MapManager.Instance.mapState == MapManager.MapState.MINIMIZED) yield break;
        Debug.Log("Showing Sector Panel");
        CurrentData = sectorUIPanelData;

        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
            root.SetActive(true);
            canvasGroup.DOFade(hoverAlpha, .1f);
        }

        sectorName.text = sectorUIPanelData.sectorName.ToString();
    }



    public void HidePanel()
    {
        foreach (CanvasGroup cg in panelElements)
        {
            cg.alpha = 0;
            cg.gameObject.SetActive(false);
        }

        canvasGroup.alpha = 0;

        root.SetActive(false);
    }

}