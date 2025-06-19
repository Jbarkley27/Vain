using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Currency
    public int Ions;
    public int Cores;
    public int Upgrades;
    public static Inventory Instance;

    [Header("UI")]
    public TMP_Text ionText;
    public TMP_Text coreText;
    public TMP_Text upgradeText;

    // List of items in the inventory
    public Dictionary<Resource.ResourceType, int> resources = new Dictionary<Resource.ResourceType, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Inventory object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the inventory with some default resources
        Ions = 1000; // Starting Ions
        Cores = 10; // Starting Cores
        Upgrades = 5; // Starting Upgrades
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReduceResource(Resource.ResourceType resourceType, int amount)
    {
        if (resourceType == Resource.ResourceType.ION)
        {
            Ions -= amount;
        }
        else if (resourceType == Resource.ResourceType.ORB)
        {
            Cores -= amount;
        }
        else if (resourceType == Resource.ResourceType.UPGRADE)
        {
            Upgrades -= amount;
        }
        else if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] -= amount;
            if (resources[resourceType] <= 0)
            {
                resources.Remove(resourceType);
            }
        }

        UpdateUI();
    }

    public int GetResourceAmount(Resource.ResourceType resourceType)
    {
        if (resourceType == Resource.ResourceType.ION)
        {
            return Ions;
        }

        if (resourceType == Resource.ResourceType.ORB)
        {
            return Cores;
        }

        if (resourceType == Resource.ResourceType.UPGRADE)
        {
            return Upgrades;
        }

        if (resources.ContainsKey(resourceType))
        {
            return resources[resourceType];
        }
        return 0; // Return 0 if the resource type does not exist
    }

    public void AddResource(Resource.ResourceType resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] += amount;
        }
        else
        {
            resources[resourceType] = amount;
        }

        UpdateUI();
    }

    public void RemoveResource(Resource.ResourceType resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] -= amount;
            if (resources[resourceType] <= 0)
            {
                resources.Remove(resourceType);
            }
        }
        else
        {
            Debug.LogWarning($"Attempted to remove {amount} of {resourceType}, but it does not exist in the inventory.");
        }

        UpdateUI();
    }
    

    public void UpdateUI()
    {
        ionText.text = Ions.ToString();
        coreText.text = Cores.ToString();
        upgradeText.text = Upgrades.ToString();

        // Update resource amounts in the UI
        foreach (var resource in resources)
        {
            switch (resource.Key)
            {
                case Resource.ResourceType.ION:
                    ionText.text = resource.Value.ToString();
                    break;
                case Resource.ResourceType.ORB:
                    coreText.text = resource.Value.ToString();
                    break;
                case Resource.ResourceType.UPGRADE:
                    upgradeText.text = resource.Value.ToString();
                    break;
            }
        }
    }

}
