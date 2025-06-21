using System.Collections.Generic;
using UnityEngine;

public class StatusEffectEnemyManager : MonoBehaviour
{
    public GameObject statusEffectRoot;
    public Dictionary<StatusEffectBase.StatusEffectType, GameObject> AppliedStatuses = new Dictionary<StatusEffectBase.StatusEffectType, GameObject>();
    public bool CanAddEffect = true;
    public EnemyBase enemy;
    public int statusEffectMax = 2;



    public void AddStatus(StatusEffectBase.StatusEffectType statusEffectType, GameObject statusEffectPrefab)
    {
        if (!CanAddEffect) return;
        if (AppliedStatuses.Count >= 2)
        {
            Debug.Log("Max Status Effects already applied");
            return;
        }
        if (AppliedStatuses.ContainsKey(statusEffectType))
        {
            // check if that status effect is already applied
            if (AppliedStatuses[statusEffectType] != null)
            {
                Debug.Log(statusEffectType + " is already applied");
                return;
            }


            // apply status
            StatusEffectBase newStatusEffect = Instantiate(statusEffectPrefab, statusEffectRoot.transform).GetComponent<StatusEffectBase>();

            AppliedStatuses[statusEffectType] = newStatusEffect.gameObject;
            newStatusEffect.ApplyStatus(false, enemy);
        }
        else
        {
            // add new status type and apply status
            StatusEffectBase newStatusEffect = Instantiate(statusEffectPrefab, statusEffectRoot.transform).GetComponent<StatusEffectBase>();


            AppliedStatuses.Add(statusEffectType, newStatusEffect.gameObject);
            newStatusEffect.ApplyStatus(false, enemy);
        }
    }


   public void RemoveStatus(StatusEffectBase.StatusEffectType statusEffectType)
    {
        if (!AppliedStatuses.ContainsKey(statusEffectType))
        {
            Debug.LogWarning($"Tried to remove {statusEffectType}, but it wasn't applied.");
            return;
        }

        GameObject statusEffectObj = AppliedStatuses[statusEffectType];
        
        if (statusEffectObj != null)
        {
            StatusEffectBase statusEffect = statusEffectObj.GetComponent<StatusEffectBase>();
            Destroy(statusEffectObj); // Clean up the effect GameObject
        }

        // keep the key but mark it as null
        AppliedStatuses[statusEffectType] = null;
    }

}