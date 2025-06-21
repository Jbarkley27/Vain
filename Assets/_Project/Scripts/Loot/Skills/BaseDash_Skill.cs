using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Basic Dash Skill")]
public class BaseDash_Skill : Skill
{
    public override void Execute(MonoBehaviour monoBehaviour)
    {
        Debug.Log("Dash Cooldown " + StatManager.Instance.GetDashRegenStat());
        GlobalDataStore.Instance.PlayerMovement.StartDash(StatManager.Instance.GetDashRegenStat());
    }
}
