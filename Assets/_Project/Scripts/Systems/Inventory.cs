using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Currency
    // public int Ions;
    // public int Cores;
    // public int Upgrades;
    public static Inventory Instance;

    // [Header("UI")]
    // public TMP_Text ionText;
    // public TMP_Text coreText;
    // public TMP_Text upgradeText;

    [System.Serializable]
    public struct InventoryItem
    {
        public Resource.ResourceType resourceType;
        public string Name;
        // public TMP_Text NameText;
        public TMP_Text AmountText;
        public int Amount;

        public void UpdateItemAmount(int amount)
        {
            Amount += amount;
            AmountText.text = Amount + "";
        }

        public void InitiateStatUI()
        {
            // NameText.text = Name;
            Amount = 0;
            UpdateItemAmount(0);
        }
    }

    // List of items in the inventory
    public List<InventoryItem> InventoryList = new List<InventoryItem>();

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
        foreach (InventoryItem item in InventoryList)
        {
            item.InitiateStatUI();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public int GetItemAmount(Resource.ResourceType type)
    {
        foreach (InventoryItem item in InventoryList)
        {
            if (item.resourceType == type)
            {
                return item.Amount;
            }
        }

        Debug.Log("No item was found");
        return 0;
    }

    public void AddResourceToInventory(Resource.ResourceType type, int amount)
    {
        foreach (InventoryItem item in InventoryList)
        {
            if (item.resourceType == type)
            {
                item.UpdateItemAmount(amount);
                return;
            }
        }

        Debug.Log("No item was found");
    }
}
