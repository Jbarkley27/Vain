using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFlasher : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashSpeed = 0.5f;
    [Range(0f, 1f)] public float alphaMin = 0.2f;
    [Range(0f, 1f)] public float alphaMax = 1f;

    private CanvasGroup canvasGroup;
    private Tween flashTween;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartFlashing();
    }

    void OnEnable()
    {
        StartFlashing();
    }

    public void StartFlashing()
    {
        StopFlashing(); // ensure no duplicates

        flashTween = canvasGroup.DOFade(alphaMin, flashSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopFlashing()
    {
        if (flashTween != null && flashTween.IsActive())
        {
            flashTween.Kill();
            canvasGroup.alpha = alphaMax; // reset alpha
        }
    }

    void OnDisable()
    {
        StopFlashing();
    }
}
