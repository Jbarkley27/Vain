using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random=UnityEngine.Random;


public class PlayerInfoUI : MonoBehaviour
{
    public bool IsOpen;
    public MiniMap miniMap;
    public List<CanvasGroup> allUIElementGroupCGList = new List<CanvasGroup>();
    public GameObject playerInfoRoot;
    public CanvasGroup rootCanvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsOpen = true; // just to bypass the false check in Close function
        StartCoroutine(ClosePlayerInfo());
        // rootCanvasGroup.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePlayerInfo()
    {
        StartCoroutine(IsOpen ? ClosePlayerInfo() : OpenPlayerInfo());
    }

    public IEnumerator OpenPlayerInfo()
    {
        if (IsOpen) yield break;
        miniMap.Minimize();
        IsOpen = true;
        rootCanvasGroup.alpha = 0;

        foreach (CanvasGroup infoElement in allUIElementGroupCGList)
        {
            infoElement.alpha = 0;
        }

        playerInfoRoot.SetActive(true);

        rootCanvasGroup.DOFade(1, .3f);

        foreach (CanvasGroup infoElement in allUIElementGroupCGList)
        {
            infoElement.DOFade(1, Random.Range(.1f, .3f));
        }
    }

    public IEnumerator ClosePlayerInfo()
    {
        if (!IsOpen) yield break;
        IsOpen = false;
        foreach (CanvasGroup infoElement in allUIElementGroupCGList)
        {
            infoElement.DOFade(0, Random.Range(.1f, .3f));
        }

        rootCanvasGroup.DOFade(0, .3f)
        .OnComplete(() =>
        {
            playerInfoRoot.SetActive(false);
        });
    }
}
