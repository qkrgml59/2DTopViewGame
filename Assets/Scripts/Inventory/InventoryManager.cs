using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI ����")]
    public GameObject inventoryPanel;
    public List<GameObject> inventorySlots = new List<GameObject>(); 
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
    
    // int totalScoreFromSelling = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        ResetAllData();


        // �۹� ������ ����
        cropIcons.Add("���", carrotIcon);
        cropIcons.Add("����", potatoIcon);
        cropIcons.Add("������", cornIcon);
        cropIcons.Add("����", sweetPotatoIcon);
        cropIcons.Add("���", appleIcon);
        cropIcons.Add("������", peachIcon);
        cropIcons.Add("����", grapeIcon);

        LoadGold();
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
        foreach (var slot in inventorySlots)
        {
            Transform iconTransform = slot.transform.Find("Icon");
            Transform countTransform = slot.transform.Find("Count");

            if (iconTransform == null || countTransform == null)
            {
                Debug.LogError($"[InventoryManager] ���� '{slot.name}'���� Icon �Ǵ� Count ������Ʈ�� ã�� �� �����ϴ�.");
                continue;
            }

            Image iconImage = iconTransform.GetComponent<Image>();
            TMP_Text countText = countTransform.GetComponent<TMP_Text>();

            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
            countText.text = "";
        }

        int index = 0;
        foreach (var item in inventory)
        {
            if (index >= inventorySlots.Count)
                break;

            var slot = inventorySlots[index];
            Transform iconTransform = slot.transform.Find("Icon");
            Transform countTransform = slot.transform.Find("Count");

            if (iconTransform == null || countTransform == null)
            {
                Debug.LogError($"[InventoryManager] ���� '{slot.name}'���� Icon �Ǵ� Count ������Ʈ�� ã�� �� �����ϴ�.");
                continue;
            }

            Image iconImage = iconTransform.GetComponent<Image>();
            TMP_Text countText = countTransform.GetComponent<TMP_Text>();

            if (cropIcons.ContainsKey(item.Key))
            {
                iconImage.sprite = cropIcons[item.Key];
                iconImage.color = new Color(1, 1, 1, 1); 
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

    public void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", playerGold);
        PlayerPrefs.Save();
    }
    public void ResetAllData()
    {
        // ��� �ʱ�ȭ
        PlayerPrefs.SetInt("PlayerGold", 0);
        PlayerPrefs.Save();
        Debug.Log("��� �ʱ�ȭ �Ϸ�");

        // ��ŷ �ʱ�ȭ
        string rankingPath = Path.Combine(Application.persistentDataPath, "ranking.json");
        if (File.Exists(rankingPath))
        {
            File.Delete(rankingPath);
            Debug.Log("��ŷ ������ ���� �Ϸ�");
        }
        else
        {
            Debug.Log("��ŷ ������ ������ �������� �ʽ��ϴ�.");
        }
    }

    public void LoadGold()
    {
        playerGold = PlayerPrefs.GetInt("PlayerGold", 0);
    }

    public int GetPlayerGold()
    {
        return playerGold;
    }

    public void SpendGold(int amount)
    {
        playerGold -= amount;
        if (playerGold < 0) playerGold = 0;

        goldText.text = $"Gold: {playerGold}G";
    }

    private int GetCropScore(string cropName)
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
        int totalScoreToAdd = 0;

        foreach (var item in inventory)
        {
            int price = GetCropPrice(item.Key);
            totalGold += price * item.Value;

            int cropScore = GetCropScore(item.Key);
            totalScoreToAdd += cropScore * item.Value;
        }

        playerGold += totalGold;

       
        goldText.text = $"Gold: {playerGold}G";

     
        Player player = FindObjectOfType<Player>();
        player.AddScore(totalScoreToAdd);

        inventory.Clear();
        UpdateInventoryUI();

        Debug.Log($"�� {totalGold}G�� �������ϴ�! ���� {totalScoreToAdd}�� �߰�!");
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return inventory;
    }
}