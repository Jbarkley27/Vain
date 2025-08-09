using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball Skill")]
public class Fireball_Skill : Skill
{
    public GameObject fireballPrefab;
    public float speed = 10f;

    public override void Execute(MonoBehaviour monoBehaviour)
    {
        Debug.Log("Executing Fireball Skill");
    }
}
