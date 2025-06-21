using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public TextMeshProUGUI shopText;
    public TextMeshProUGUI GoldText;

    public Dictionary<string, int> cropPrices = new Dictionary<string, int>()
    {
        { "���", 5 },
        { "����", 7 },
        { "����", 10 },
        { "������", 12 },
        { "���", 13 },
        { "������", 15 },
        { "����", 18 },
    };

    private void Start()
    {
        shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateShopUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void UpdateShopUI()
    {
        shopText.text = "";
        foreach(var item in InventoryManager.Instance.inventory)
        {
            string cropName = item.Key;
            int amount = item.Value;
            int price = cropPrices.ContainsKey(cropName) ? cropPrices[cropName] : 5;

            shopText.text += $"{cropName} ({amount}��) - ���� {price}���\n";

        }

        GoldText.text = "Gold : " + GameManager.Instance.Gold;

    }

    public void SellItem(string cropName)
    {
       
    }

}
        
