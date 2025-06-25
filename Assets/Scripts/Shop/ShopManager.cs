using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public TextMeshProUGUI shopInventoryText;

    private void OnEnable()
    {
        UpdateShopUI();
    }

    // 상점 UI 업데이트
    public void UpdateShopUI()
    {
        if (shopInventoryText == null)
            return;

        shopInventoryText.text = "판매 가능한 작물:\n";

        foreach (var item in InventoryManager.Instance.GetInventoryItems())
        {
            shopInventoryText.text += $"{item.Key} : {item.Value}개\n";
        }
    }

    // 판매 버튼
    public void OnSellAllButtonClicked()
    {
        InventoryManager.Instance.SellAllItems();
        UpdateShopUI(); // 판매 후 바로 갱신
        CloseShop();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

}