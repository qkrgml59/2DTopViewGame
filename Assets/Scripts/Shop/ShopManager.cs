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
        { "당근", 5 },
        { "감자", 7 },
        { "고구마", 10 },
        { "옥수수", 12 },
        { "사과", 13 },
        { "복숭아", 15 },
        { "포도", 18 },
    };
}
