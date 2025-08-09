using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class StatusEffectBase : MonoBehaviour
{
    public enum StatusEffectType { NONE, SCORCHED, JOLTED, CORRODED, SLOWED };
    public string EffectName;
    public float LifeTime;
    public StatusEffectType statusEffectType;
    public Sprite Icon;
    public Color mainColor;
    public Image borderColor;
    public float elapsedTime;

    [Header("UI")]
    public CanvasGroup canvasGroup;
    public float flashDuration = 0.2f;
    public int flashCount = 3;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public bool IsPlayer;
    public EnemyBase enemyTarget;


    public virtual void ApplyStatus(bool IsPlayer, EnemyBase enemy = null) { }
    public virtual void OnUpdate(float deltaTime)
    {
        elapsedTime += deltaTime;
    }
    public virtual void OnRemove() { }

    public bool IsFinished => elapsedTime >= LifeTime;

    public void FlashCanvasGroup()
    {
        if (!canvasGroup) return;
        // Kill any existing tween on the canvas group
        canvasGroup.DOKill();

        // Flash = fade out, then in = 1 cycle
        Sequence flashSequence = DOTween.Sequence();

        for (int i = 0; i < flashCount; i++)
        {
            if (!canvasGroup) return;
            flashSequence.Append(canvasGroup.DOFade(minAlpha, flashDuration));
            flashSequence.Append(canvasGroup.DOFade(maxAlpha, flashDuration));
        }

        if (!canvasGroup) return;
        flashSequence.SetUpdate(true); // Optional: works in TimeScale 0 (e.g., UI during pause)
    }
}