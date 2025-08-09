using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random=UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public Transform playerScentNodeParent;
    public List<ScentNode> PlayerScentNodes;


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

        PlayerScentNodes.Clear();
        foreach (Transform scentNode in playerScentNodeParent)
        {
            scentNode.AddComponent<ScentNode>();
            PlayerScentNodes.Add(scentNode.GetComponent<ScentNode>());
        }
    }



    public float GetEnemyTierStatMultiplier()
    {
        return 1f;
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