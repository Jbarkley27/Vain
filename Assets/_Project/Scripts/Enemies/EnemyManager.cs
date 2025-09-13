using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random=UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public bool ShowEnemyDebugRays = true;
    public int MaxEnemyAttackPoints = 3;
    public int CurrentEnemyAttackPoints = 0;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Enemy Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    public float GetEnemyTierStatMultiplier()
    {
        return 1f;
    }



    public bool attackLock = false;
    public bool TryToAttack(int attackCost)
    {
        if (attackLock) return false;
        attackLock = true;
        if (CurrentEnemyAttackPoints + attackCost <= MaxEnemyAttackPoints)
        {
            CurrentEnemyAttackPoints += attackCost;
            attackLock = false;
            return true;
        }

        attackLock = false;
        return false;
    }

    public void EnemyFinishedAttack(int attackCost)
    {
        CurrentEnemyAttackPoints -= attackCost;
        if (CurrentEnemyAttackPoints < 0) CurrentEnemyAttackPoints = 0;
    }
}