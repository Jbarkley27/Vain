using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public WaveConfig waveConfig;
    public List<Transform> spawnPoints;
    public Transform spawnParent;
    public EnemyPooler pooler;
    public Transform player;
    public int currentTier = 1;
    public Planet planet;
    public float despawnDelay = 10f;
    private Coroutine despawnCoroutine;
    private bool isPlayerInZone = false;
    private bool enemiesSpawned = false;
    public List<EnemyBase> activeEnemies = new List<EnemyBase>();

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
            isPlayerInZone = true;

            // If player re-enters the planet region, cancel the despawn coroutine
            if (despawnCoroutine != null)
            {
                StopCoroutine(despawnCoroutine);
                despawnCoroutine = null;
            }



            if (!enemiesSpawned)
            {
                SpawnWave(); // First time only
                enemiesSpawned = true;
                Debug.Log("Spawning Enemies for the first time");
                return;
            }
            else
            {
                Debug.Log("Player still in combat, enemy state reamins");
            }

            // SpawnWave(); // this was causing double enemies to spawn
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            if (despawnCoroutine == null)
            {
                despawnCoroutine = StartCoroutine(DespawnTimer());
            }
        }
    }

    private IEnumerator DespawnTimer()
    {
        Debug.Log("Player left zone, countdown started.");

        float timer = 0f;

        while (timer < despawnDelay)
        {
            if (isPlayerInZone)
            {
                Debug.Log("Player returned, despawn canceled.");
                yield break; // Cancel despawn
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Despawn delay reached. Resetting battle.");
        DespawnWave();
        enemiesSpawned = false;
        despawnCoroutine = null;
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
                if (!enemy.IsSetup) enemy.Setup(entry.tier, player, this);
                activeEnemies.Add(enemy);
            }
        }
    }

    public void DespawnEnemy(GameObject enemyObj)
    {
        if (enemyObj == null)
        {
            Debug.LogWarning("Enemy object is null.");
            return;
        }

        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            pooler.Despawn(enemyObj, enemy.EnemyID);
            Debug.Log("Despawned enemy: " + enemy.EnemyID);
        }
        else
        {
            Debug.LogWarning("Enemy not found in active list: " + enemy.EnemyID);
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
            // Debug.Log("Position + " + hit.position);
            return hit.position;
        }

        // Debug.Log("Position Outside --  " + point);
        return point; // fallback
    }
}
