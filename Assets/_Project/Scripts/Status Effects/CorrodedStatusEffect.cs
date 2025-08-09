
using System.Collections;
using UnityEngine;

public class CorrodedStatusEffect : StatusEffectBase
{
    public float tickFrequency;
    public int tickDamage;

    public override void ApplyStatus(bool IsPlayer, EnemyBase enemy = null)
    {
        FlashCanvasGroup();
        enemyTarget = enemy;
        StartCoroutine(Corrode());
        StartCoroutine(RemoveEffect());
        this.IsPlayer = IsPlayer;
        Debug.Log("Starting Status " + statusEffectType);
    }

    public IEnumerator Corrode()
    {
        while (this != null && gameObject != null) // Safe check
        {
            ApplyCorrosionDamage(); // Your custom logic

            yield return new WaitForSeconds(tickFrequency); // Or however often you want to apply it
        }
    }

    public void ApplyCorrosionDamage()
    {
        if (IsPlayer)
        {
            PlayerHealth.Instance.TakeDamage(tickDamage);
        }
        else
        {
            if (!enemyTarget) return;
            enemyTarget.TakeDamage(tickDamage);
        }

        FlashCanvasGroup();
    }

    public IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(LifeTime);

        if (IsPlayer)
        {
            // This is on the player
            StatusEffectPlayerManager.Instance.RemoveStatus(statusEffectType);
        }
        else
        {
            enemyTarget.statusEffectEnemyManager.RemoveStatus(statusEffectType);
        }
    }
}