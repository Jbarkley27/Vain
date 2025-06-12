using System.Collections;
using UnityEngine;

public abstract class AttackDataBase : ScriptableObject
{
    public EnemyAttackLibrary.AttackType AttackType;
    public GameObject ProjectilePrefab;
    public float Lifetime = 3f;
    public float BaseSpeed = 10f;
    public int Damage;
    public float RateOfFire;
    





    // All attacks must implement this
    public abstract IEnumerator Execute(EnemyBase enemy);
}

