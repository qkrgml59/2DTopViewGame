using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string cropName)
    {
        if (inventory.ContainsKey(cropName))
            inventory[cropName]++;
        else
            inventory[cropName] = 1;

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        inventoryText.text = "";
        foreach (var item in inventory)
        {
            inventoryText.text += $"{item.Key} : {item.Value}°³\n";
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
