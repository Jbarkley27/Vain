using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random=UnityEngine.Random;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    public bool ShouldTrackOnDrop = false;
    public float TrackSpeed;
    public enum ResourceType { ION, ORB, UPGRADE };
    public ResourceType resourceType;
    public int amount;

    void Start()
    {
        if (TrackSpeed != 0 ) TrackSpeed = Random.Range(TrackSpeed, TrackSpeed + 3f);
    }

    void Update()
    {
        if (ShouldTrackOnDrop)
        {
            // Smoothly track the player if ShouldTrackOnDrop is true
            Transform playerTransform = GlobalDataStore.Instance.Player.transform;
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, TrackSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming Player has a method to collect resources
            Inventory.Instance.AddResourceToInventory(resourceType, amount);

            gameObject.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}
