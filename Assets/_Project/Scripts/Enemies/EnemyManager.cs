using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public Transform playerScentNodeParent;
    public Transform enemyWanderNodeParent;
    public List<GameObject> PlayerScentNodes;
    public List<GameObject> EnemyWanderNodes;


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

        // Get all Nodes
        EnemyWanderNodes.Clear();
        PlayerScentNodes.Clear();
        foreach (Transform scentNode in playerScentNodeParent)
        {
            PlayerScentNodes.Add(scentNode.gameObject);
        }

        foreach (Transform scentNode in enemyWanderNodeParent)
        {
            EnemyWanderNodes.Add(scentNode.gameObject);
        }
    }

    public float GetStatMultiplier()
    {
        return 1f;
    }

    public GameObject GetRandomWanderNodePosition()
    {
        return EnemyWanderNodes[Random.Range(0, EnemyWanderNodes.Count)];
    }

    public Transform GetRandomPlayerScentNode()
    {
        return PlayerScentNodes[Random.Range(0, PlayerScentNodes.Count)].transform;
    }
}