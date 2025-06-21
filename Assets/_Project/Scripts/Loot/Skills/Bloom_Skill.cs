using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Bloom Skill")]
public class Bloom_Skill : Skill
{

    public override void Execute(MonoBehaviour monoBehaviour)
    {
        Debug.Log("Executing Bloom Skill");
    }
}
