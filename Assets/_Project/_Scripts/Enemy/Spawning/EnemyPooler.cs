using UnityEngine;
using System.Collections.Generic;

public class EnemyPooler : MonoBehaviour {
    // Reference to a scriptable object or container that holds all enemy prefabs
    public EnemyDatabase library;

    // Where inactive enemies will be stored in the hierarchy
    public Transform pooledParent;

    // Where active enemies will be placed when spawned
    public Transform activeParent;

    // Dictionary that holds queues of enemies by their EnemyID
    private Dictionary<EnemyID, Queue<GameObject>> _pools = new();





    /// <summary>
    /// Spawns (activates) an enemy of a specific type at a given position/rotation.
    /// </summary>
    public GameObject Spawn(EnemyID id, Vector3 position, Quaternion rotation)
    {
        // Make sure we have a pool queue for this enemy type
        if (!_pools.ContainsKey(id))
        {
            _pools[id] = new Queue<GameObject>();
        }

        GameObject enemy;

        // If there's an available enemy in the pool, reuse it
        if (_pools[id].Count > 0)
        {
            enemy = _pools[id].Dequeue();
        }
        else
        {
            // Otherwise, instantiate a new one from the prefab library
            GameObject prefab = library.GetPrefab(id);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found for EnemyID: {id}");
                return null;
            }

            // Parent new enemy to pooledParent by default
            enemy = Instantiate(prefab, pooledParent);
        }

        // Position and rotate the enemy
        enemy.transform.SetPositionAndRotation(position, rotation);

        // Move the enemy under the activeParent in the hierarchy
        enemy.transform.SetParent(activeParent);

        // Enable the enemy so it becomes active in the game
        enemy.SetActive(true);

        // Let the enemy know it has been spawned from the pool
        if (enemy.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnSpawned();
        }

        return enemy;
    }




    /// <summary>
    /// Deactivates and returns an enemy to the pool.
    /// </summary>
    public void Despawn(GameObject enemy, EnemyID id)
    {
        // Disable the enemy so it stops updating/animating/etc.
        enemy.SetActive(false);

        // Re-parent the enemy back to pooledParent
        enemy.transform.SetParent(pooledParent);

        // Notify the enemy it is being returned to the pool
        if (enemy.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnDespawned();
        }

        // Make sure the queue for this enemy type exists
        if (!_pools.ContainsKey(id))
        {
            _pools[id] = new Queue<GameObject>();
        }

        // Add it back to the pool queue
        _pools[id].Enqueue(enemy);
    }
    

    

    /// <summary>
    /// Pre-creates and pools a set number of enemies before gameplay starts.
    /// </summary>
    public void Prewarm(EnemyID id, int count)
    {
        // Ensure there's a queue for this enemy type
        if (!_pools.ContainsKey(id))
        {
            _pools[id] = new Queue<GameObject>();
        }

        for (int i = 0; i < count; i++)
        {
            // Get the prefab for this enemy type
            GameObject prefab = library.GetPrefab(id);
            if (prefab == null) continue;

            // Instantiate the enemy under pooledParent (inactive pool)
            GameObject enemy = Instantiate(prefab, pooledParent);
            enemy.SetActive(false); // Disable the object so it's pooled

            // Call the OnDespawned hook to reset any internal state
            if (enemy.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnDespawned();
            }

            // Add the enemy to the pool
            _pools[id].Enqueue(enemy);
        }
    }
}
