using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI ����")]
    public GameObject inventoryPanel;
    public List<GameObject> inventorySlots = new List<GameObject>(); // ���� ������Ʈ ���� ����
    public TextMeshProUGUI goldText;

    [Header("�۹� ������")]
    public Sprite carrotIcon;
    public Sprite potatoIcon;
    public Sprite cornIcon;
    public Sprite sweetPotatoIcon;
    public Sprite appleIcon;
    public Sprite peachIcon;
    public Sprite grapeIcon;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private Dictionary<string, Sprite> cropIcons = new Dictionary<string, Sprite>();

    private int playerGold = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // �۹� ������ ����
        cropIcons.Add("���", carrotIcon);
        cropIcons.Add("����", potatoIcon);
        cropIcons.Add("������", cornIcon);
        cropIcons.Add("����", sweetPotatoIcon);
        cropIcons.Add("���", appleIcon);
        cropIcons.Add("������", peachIcon);
        cropIcons.Add("����", grapeIcon);
    }

    private void Update()
    {
        // I Ű�� �κ��丮 ����/�ݱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // �۹� �߰�
    public void AddItem(string cropName)
    {
        if (inventory.ContainsKey(cropName))
            inventory[cropName]++;
        else
            inventory.Add(cropName, 1);

        UpdateInventoryUI();
    }

    // �κ��丮 UI ������Ʈ
    private void UpdateInventoryUI()
    {
        // ��� ���� �ʱ�ȭ
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount >= 2)
            {
                Image iconImage = slot.transform.GetChild(0).GetComponent<Image>();
                TMP_Text countText = slot.transform.GetChild(1).GetComponent<TMP_Text>();

                iconImage.sprite = null;
                iconImage.color = new Color(1, 1, 1, 0); // ����
                countText.text = "";
            }
        }

        int index = 0;
        foreach (var item in inventory)
        {
            if (index >= inventorySlots.Count)
                break;

            var slot = inventorySlots[index];
            Image iconImage = slot.transform.GetChild(0).GetComponent<Image>();
            TMP_Text countText = slot.transform.GetChild(1).GetComponent<TMP_Text>();

            if (cropIcons.ContainsKey(item.Key))
            {
                iconImage.sprite = cropIcons[item.Key];
                iconImage.color = new Color(1, 1, 1, 1); // ������ ���̰�
                countText.text = item.Value.ToString();
            }
            else
            {
                Debug.LogWarning($"�������� ��ϵ��� ���� �۹�: {item.Key}");
            }

            index++;
        }

        goldText.text = $"Gold: {playerGold}G";
    }

    // �۹� ����
    private int GetCropPrice(string cropName)
    {
        switch (cropName)
        {
            case "���": return 5;
            case "����": return 7;
            case "������": return 12;
            case "����": return 10;
            case "���": return 12;
            case "������": return 15;
            case "����": return 18;
            default: return 0;
        }
    }

    // �۹� ��ü �Ǹ�
    public void SellAllItems()
    {
        int totalGold = 0;

        foreach (var item in inventory)
        {
            int price = GetCropPrice(item.Key);
            totalGold += price * item.Value;
        }

        playerGold += totalGold;
        inventory.Clear();
        UpdateInventoryUI();

        Debug.Log($"�� {totalGold}G�� �������ϴ�!");
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return inventory;
    }
}