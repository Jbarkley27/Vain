using UnityEngine;

[CreateAssetMenu(menuName = "Affixes/StatBoost")]
public class StatBoostAffix : AffixBase
{

    // When i come back to structure diffferent affixes:
    /*
     * One Time Stat Boost
     * check if condition is true Tick, always checking
     * skill only
     * blaster only
     * on enemy death, something with their position
     * 
     */
    public StatManager.StatType stat;
    public int amount;

    public override void OnEquip()
    {
        Debug.Log("Adding " + amount + " to " + stat.ToString());
        StatManager.Instance.UpdateStatValue(stat, amount);
    }

    public override void OnUnequip()
    {
        Debug.Log("Removing " + amount + " from " + stat.ToString());
        StatManager.Instance.UpdateStatValue(stat, -amount);
    }
}
