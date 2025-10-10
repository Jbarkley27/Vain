using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public GameObject mapUIRoot;
    public CanvasGroup mapRootCG;
    public List<MinimapElement> minimapElements;
    public Camera minimapCam;


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
    }

    public void AddToMinimapList(MinimapElement minimapElement)
    {
        if (!minimapElements.Contains(minimapElement))
        {
            minimapElements.Add(minimapElement);
        }
    }
}
