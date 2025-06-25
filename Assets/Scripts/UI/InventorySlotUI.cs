using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    public void SetItem(Sprite icon, int count)
    {
        iconImage.sprite = icon;
        iconImage.enabled = true;
        countText.text = count.ToString();
    }

    public void Clear()
    {
        iconImage.enabled = false;
        countText.text = "";
    }

    public void UpdateCount(int count)
    {
        countText.text = count.ToString();
    }
}
