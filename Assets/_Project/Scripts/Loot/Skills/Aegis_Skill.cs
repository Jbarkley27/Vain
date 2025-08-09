using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Aegis Skill")]
public class Aegis_Skill : Skill
{
    public override void Execute(MonoBehaviour monoBehaviour)
    {
        Debug.Log("Executing Aegis Skill");
    }
}
