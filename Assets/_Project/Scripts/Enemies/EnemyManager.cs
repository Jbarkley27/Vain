using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public Transform playerScentNodeParent;
    public Transform enemyWanderNodeParent;
    public List<ScentNode> PlayerScentNodes;
    public List<GameObject> EnemyWanderNodes;
    public List<GameObject> EnemyRepositionNodes;


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
            scentNode.AddComponent<ScentNode>();
            PlayerScentNodes.Add(scentNode.GetComponent<ScentNode>());
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

    public Transform GetRandomPlayerScentNode(ScentNode usedNode = null)
    {
        for (int i = 0; i < PlayerScentNodes.Count; i++)
        {
            if (!PlayerScentNodes[i].IsTaken)
            {
                PlayerScentNodes[i].IsTaken = true;
                return PlayerScentNodes[i].gameObject.transform;
            }
        }

        if (usedNode) usedNode.IsTaken = false;

        return PlayerScentNodes[Random.Range(0, PlayerScentNodes.Count)].transform;
    }
}