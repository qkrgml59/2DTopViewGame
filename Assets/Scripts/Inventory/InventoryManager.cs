using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI 연결")]
    public GameObject inventoryPanel;
    public List<GameObject> inventorySlots = new List<GameObject>(); 
    public TextMeshProUGUI goldText;

    [Header("작물 아이콘")]
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

        // 작물 아이콘 세팅
        cropIcons.Add("당근", carrotIcon);
        cropIcons.Add("감자", potatoIcon);
        cropIcons.Add("옥수수", cornIcon);
        cropIcons.Add("고구마", sweetPotatoIcon);
        cropIcons.Add("사과", appleIcon);
        cropIcons.Add("복숭아", peachIcon);
        cropIcons.Add("포도", grapeIcon);

        LoadGold();
    }

    private void Update()
    {
        // I 키로 인벤토리 열기/닫기
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // 작물 추가
    public void AddItem(string cropName)
    {
        if (inventory.ContainsKey(cropName))
            inventory[cropName]++;
        else
            inventory.Add(cropName, 1);

        UpdateInventoryUI();
    }

    // 인벤토리 UI 업데이트
    private void UpdateInventoryUI()
    {
        foreach (var slot in inventorySlots)
        {
            Transform iconTransform = slot.transform.Find("Icon");
            Transform countTransform = slot.transform.Find("Count");

            if (iconTransform == null || countTransform == null)
            {
                Debug.LogError($"[InventoryManager] 슬롯 '{slot.name}'에서 Icon 또는 Count 오브젝트를 찾을 수 없습니다.");
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
                Debug.LogError($"[InventoryManager] 슬롯 '{slot.name}'에서 Icon 또는 Count 오브젝트를 찾을 수 없습니다.");
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
                Debug.LogWarning($"아이콘이 등록되지 않은 작물: {item.Key}");
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
            case "당근": return 5;
            case "감자": return 7;
            case "옥수수": return 12;
            case "고구마": return 10;
            case "사과": return 12;
            case "복숭아": return 15;
            case "포도": return 18;
            default: return 0;
        }
    }

    // 작물 가격
    private int GetCropPrice(string cropName)
    {
        switch (cropName)
        {
            case "당근": return 5;
            case "감자": return 7;
            case "옥수수": return 12;
            case "고구마": return 10;
            case "사과": return 12;
            case "복숭아": return 15;
            case "포도": return 18;
            default: return 0;
        }
    }

    // 작물 전체 판매
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

        // 골드를 여기서 더하고 UI 갱신
        playerGold += totalGold;

        // UI 즉시 업데이트
        goldText.text = $"Gold: {playerGold}G";

        // 점수는 플레이어에 반영
        Player player = FindObjectOfType<Player>();
        player.AddScore(totalScoreToAdd);

        inventory.Clear();
        UpdateInventoryUI();

        Debug.Log($"총 {totalGold}G를 벌었습니다! 점수 {totalScoreToAdd}점 추가!");
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return inventory;
    }
}