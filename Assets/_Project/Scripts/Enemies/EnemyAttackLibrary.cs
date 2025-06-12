using UnityEngine;

public class EnemyAttackLibrary : MonoBehaviour
{
    public enum AttackType { RADIAL };
    public AttackDataBase queuedAttack;
    public EnemyBase enemyBase;

    public void Fire()
    {
        StartCoroutine(queuedAttack.Execute(enemyBase));
    }
}