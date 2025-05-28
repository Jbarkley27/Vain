using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public WaveConfig waveConfig;
    public List<Transform> spawnPoints;
    public Transform spawnParent;
    public EnemyPooler pooler;
    public Transform player;
    public int currentTier = 1;
    public Planet planet;

    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    void Awake()
    {
        foreach (Transform point in spawnParent)
        {
            if (point == null) continue;
            spawnPoints.Add(point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnWave();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DespawnWave();
        }
    }

    void SpawnWave()
    {
        Debug.Log("Spawning enemies for " + planet.Name);
        foreach (var entry in waveConfig.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                Vector3 spawnPos = GetValidSpawnPosition();
                GameObject enemyObj = pooler.Spawn(entry.enemyID, spawnPos, Quaternion.identity);
                EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
                enemy.Setup(entry.tier, player);
                activeEnemies.Add(enemy);
            }
        }
    }

    void DespawnWave()
    {
        Debug.Log("Disabling enemies for " + planet.Name);
        foreach (var enemy in activeEnemies)
        {
            pooler.Despawn(enemy.gameObject, enemy.EnemyID);
        }
        
        activeEnemies.Clear();
    }

    Vector3 GetValidSpawnPosition()
    {
        // Use NavMesh.SamplePosition or spawnPoints[]
        var point = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 2f, NavMesh.AllAreas))
        {
            Debug.Log("Position + " + hit.position);
            return hit.position;
        }

        Debug.Log("Position Outside --  " + point);
        return point; // fallback
    }
}
