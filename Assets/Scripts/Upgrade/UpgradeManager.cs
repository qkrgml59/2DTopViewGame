using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public GameObject upgradePanel;

    public int moveSpeedPrice = 50;
    public int harvestRangePrice = 50;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
    }

    public void UpgradeMoveSpeed()
    {
        if (InventoryManager.Instance.GetPlayerGold() >= moveSpeedPrice)
        {
            InventoryManager.Instance.SpendGold(moveSpeedPrice);
            PlayerPrefs.SetFloat("MoveSpeed", PlayerPrefs.GetFloat("MoveSpeed", 2f) + 0.5f);
            Debug.Log("�̵��ӵ� ��ȭ��!");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            UIManager.Instance.ShowGoldWarning();
        }
    }

    public void UpgradeHarvestRange()
    {
        if (InventoryManager.Instance.GetPlayerGold() >= harvestRangePrice)
        {
            InventoryManager.Instance.SpendGold(harvestRangePrice);
            PlayerPrefs.SetFloat("HarvestRange", PlayerPrefs.GetFloat("HarvestRange", 0.5f) + 0.1f);
            Debug.Log("ä�� ���� ��ȭ��!");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            UIManager.Instance.ShowGoldWarning();
        }
    }
}
