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

    // ���� UI ������Ʈ
    public void UpdateShopUI()
    {
        if (shopInventoryText == null)
            return;

        shopInventoryText.text = "�Ǹ� ������ �۹�:\n";

        foreach (var item in InventoryManager.Instance.GetInventoryItems())
        {
            shopInventoryText.text += $"{item.Key} : {item.Value}��\n";
        }
    }

    // �Ǹ� ��ư
    public void OnSellAllButtonClicked()
    {
        InventoryManager.Instance.SellAllItems();
        UpdateShopUI(); // �Ǹ� �� �ٷ� ����
        CloseShop();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

}