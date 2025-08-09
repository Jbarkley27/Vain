using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    public static StatManager Instance;

    public enum StatType { SPEED, BARRIER, HEALTH, ATTACK, DASH_COOLDOWN, SKILL_COOLDOWN, PLASMA_DAMAGE, SCORCH_DAMAGE, JOLT_DAMAGE, CORROSIVE_DAMAGE };

    [Header("Health")]
    public int BaseHealth = 50;
    public int HealthUpgradeStep = 5;
    public int MaxHealth = 100;


    [Header("Dash Regen")]
    public float BaseCooldown = 5;
    public float ReductionStep = .3f;


    [Header("Thrust")]
    public float BaseThrustSpeed = 140;
    public float ThrustUpgradeStep = 10;
    public float BaseBoostSpeed = 200f;
    public float BoostUpgradeStep = 6f;

    [Header("Scorch")]
    public int BaseElementalDamageBoost = 0;
    public int ElementalDaamgeStep = 0;
    public int ScorchDamage = 0;
    public int JoltDamage = 0;
    public int AcidDamage = 0;
    public int PlasmaDamage = 0;



    [Serializable]
    public struct Stat
    {
        public int CurrentValue;
        public TMP_Text NameText;
        public TMP_Text CurrentValueText;
        public StatType statType;
        public string Name;

        public void UpdateStatValue(int amount)
        {
            CurrentValue += amount;
            CurrentValueText.text = CurrentValue + "";
        }

        public void InitiateStatUI()
        {
            NameText.text = Name;
            UpdateStatValue(0);
        }

    }

    public List<Stat> AllStats = new List<Stat>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a StatManager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Stat stat in AllStats)
        {
            stat.InitiateStatUI();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetStat(StatType type)
    {
        foreach (Stat stat in AllStats)
        {
            if (stat.statType == type)
            {
                return stat.CurrentValue;
            }
        }

        Debug.Log("No stat was found");
        return 0;
    }



    public void UpdateStatValue(StatType type, int amount)
    {
        foreach (Stat stat in AllStats)
        {
            if (stat.statType == type)
            {
                stat.UpdateStatValue(amount);
            }
        }

        Debug.Log("No stat was found");
    }


    public float GetDashRegenStat()
    {
        return Mathf.Clamp(BaseCooldown - (GetStat(StatType.DASH_COOLDOWN) * ReductionStep), 1, 40);
    }

    public float GetBoostValue()
    {
        return Mathf.Clamp(BaseBoostSpeed + (GetStat(StatType.SPEED) * BoostUpgradeStep), 100f, 300f);
    }

    public float GetThrustValue()
    {
        return Mathf.Clamp(BaseThrustSpeed + (GetStat(StatType.SPEED) * ThrustUpgradeStep), 60f, 220f);
    }

    public int GetHealthValue()
    {
        return Mathf.Clamp(BaseHealth + (GetStat(StatType.HEALTH) * HealthUpgradeStep), 0, MaxHealth);
    }

    public int GetElementDamageValue(WeaponTypes.ElementType types)
    {
        switch (types)
        {
            case WeaponTypes.ElementType.PLASMA:
                return Mathf.Clamp(BaseElementalDamageBoost + (GetStat(StatType.PLASMA_DAMAGE) * ElementalDaamgeStep), 0, 10);
            case WeaponTypes.ElementType.SCORCH:
                return Mathf.Clamp(BaseElementalDamageBoost + (GetStat(StatType.SCORCH_DAMAGE) * ElementalDaamgeStep), 0, 10);
            case WeaponTypes.ElementType.CORROSION:
                return Mathf.Clamp(BaseElementalDamageBoost + (GetStat(StatType.CORROSIVE_DAMAGE) * ElementalDaamgeStep), 0, 10);
            case WeaponTypes.ElementType.JOLT:
                return Mathf.Clamp(BaseElementalDamageBoost + (GetStat(StatType.JOLT_DAMAGE) * ElementalDaamgeStep), 0, 10);
        }

        Debug.Log("Couldn't find elemental value");
        return 0;
    }



}
