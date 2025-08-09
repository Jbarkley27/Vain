using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Surge Skill")]
public class Surge_Skill : Skill
{

    public override void Execute(MonoBehaviour monoBehaviour)
    {
        Debug.Log("Executing Surge Skill");
    }
}
