using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Random=UnityEngine.Random;
using System.Collections;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public float spawnForce = 10f;

    [Header("Resource Prefabs")]
    public GameObject ionResoucePrefab;
    public GameObject coreResourcePrefab;
    public GameObject upgradeResourcePrefab;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnResource(Resource.ResourceType resourceType, Vector3 position)
    {
        Debug.Log($"Spawning resource of type {resourceType} at position {position}");


        // use dotween to spawn resouce and make it burst out in a random x and z position near the position
        GameObject resourcePrefab = null;
        switch (resourceType)
        {
            case Resource.ResourceType.ION:
                resourcePrefab = ionResoucePrefab;
                break;
            case Resource.ResourceType.ORB:
                resourcePrefab = coreResourcePrefab;
                break;
            case Resource.ResourceType.UPGRADE:
                resourcePrefab = upgradeResourcePrefab;
                break;
        }

        Debug.Log("Spawing resource prefab: " + resourcePrefab);

        if (resourcePrefab != null)
        {
            GameObject resourceInstance = Instantiate(resourcePrefab, position, Quaternion.identity);
            // Add a random force to the resource to make it burst out
            Rigidbody rb = resourceInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                StartCoroutine(FollowPlayer(Random.Range(7f, 10f)));
            }
        }
        else
        {
            Debug.LogError("Resource prefab not found for type: " + resourceType);
        }
    }


    private IEnumerator FollowPlayer(float followSpeed)
    {
        Transform target = GlobalDataStore.Instance.Player.transform;

        while (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                followSpeed * Time.deltaTime
            );
            yield return null; // wait until next frame
        }
    }


    Vector3 GetRandomPointAround(Vector3 center, float radius)
    {
        Vector2 offset = Random.insideUnitCircle * radius;
        return new Vector3(center.x + offset.x, center.y, center.z + offset.y);
    }
    
}