using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.IO.Compression;

public class MinimapElement : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float hoverAlpha = .9f;
    public float normalAlpha = .4f;
    public CanvasGroupFlasher flasher;
    public Vector3 screenPos;
    public GameObject iconUI;

    void Start()
    {
        MapManager.Instance.AddToMinimapList(this);
    }


    public void Update()
    {
        screenPos = MapManager.Instance.minimapCam.WorldToScreenPoint(transform.position);

    }

}