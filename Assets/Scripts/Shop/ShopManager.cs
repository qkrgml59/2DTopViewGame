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
}
