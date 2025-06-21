using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StatusEffectPlayerManager : MonoBehaviour
{
    public static StatusEffectPlayerManager Instance;
    public GameObject statusEffectRoot;
    public Dictionary<StatusEffectBase.StatusEffectType, GameObject> AppliedStatuses = new Dictionary<StatusEffectBase.StatusEffectType, GameObject>();
    public bool CanAddEffect = true;

    [Header("All Status Prefabs")]
    public GameObject CorrodedPrefab;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Status Effect Player Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Transform transform in statusEffectRoot.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(ApplyPoison());
    }


    public IEnumerator ApplyPoison()
    {
        yield return new WaitForSeconds(3);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(7);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(5);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(3);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(7);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(5);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(3);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(7);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

        yield return new WaitForSeconds(5);

        AddStatus(StatusEffectBase.StatusEffectType.CORRODED);

    }



    public void AddStatus(StatusEffectBase.StatusEffectType statusEffectType)
    {
        if (!CanAddEffect) return;
        if (AppliedStatuses.ContainsKey(statusEffectType))
        {

            // check if that status effect is already applied
            if (AppliedStatuses[statusEffectType] != null)
            {
                Debug.Log(statusEffectType + " is already applied");
                return;
            }


            // apply status
            StatusEffectBase newStatusEffect = Instantiate(GetStatusPrefab(statusEffectType), statusEffectRoot.transform).GetComponent<StatusEffectBase>();

            AppliedStatuses[statusEffectType] = newStatusEffect.gameObject;
            newStatusEffect.ApplyStatus(true);
        }
        else
        {
            // add new status type and apply status
            StatusEffectBase newStatusEffect = Instantiate(GetStatusPrefab(statusEffectType), statusEffectRoot.transform).GetComponent<StatusEffectBase>();


            AppliedStatuses.Add(statusEffectType, newStatusEffect.gameObject);
            newStatusEffect.ApplyStatus(true);
        }

    }

    public GameObject GetStatusPrefab(StatusEffectBase.StatusEffectType statusEffectType)
    {
        switch (statusEffectType)
        {
            case StatusEffectBase.StatusEffectType.CORRODED:
                return CorrodedPrefab;
            default:
                return CorrodedPrefab;
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
            Debug.Log("Removing " + statusEffectType);
            StatusEffectBase statusEffect = statusEffectObj.GetComponent<StatusEffectBase>();
            statusEffect.canvasGroup.DOFade(0, .2f)
            .OnComplete(() =>
            {
                Destroy(statusEffectObj); // Clean up the effect GameObject
                // keep the key but mark it as null
                AppliedStatuses[statusEffectType] = null;
            });
        }
    }

}