using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using static RankingManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [Header("�ؽ�Ʈ UI")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI score;


    [Header("��ŷ UI")]
    public TMP_InputField inputField;
    public Button gameStartButton;
    public GameObject rankingPanel;
    public TextMeshProUGUI rankingText;
    public Transform rankingContent;
    public GameObject rankingItemPrefab;

    [Header("���� �� UI")]
    public Image dayNightImage;
    public Sprite daySprite;
    public Sprite nightSprite;
    public GameObject nightOverlay;

    [Header("���� Ŭ���� UI")]
    public GameObject gameClearPanel;


    public Button rankingOpenButton;
    public Button rankingCloseButton;

    public RankingManager rankingManager;


    public GameObject helpUI;
    public GameObject RankingUI;

    public GameObject shopPanel;

    public GameObject goldWarningUI;


    private void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }

        if (rankingOpenButton != null)
            rankingOpenButton.onClick.AddListener(OpenRankingPanel);

        if (rankingCloseButton != null)
            rankingCloseButton.onClick.AddListener(CloseRankingPanel);

    }

    public void OpenRankingPanel()
    {
        rankingPanel.SetActive(true);

        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        List<RankingEntry> rankings = rankingManager.currentData.rankings;

        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingContent);
            TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"{i + 1}.  {rankings[i].playerName} - {rankings[i].score}��";
        }
    }

    public void ShowGoldWarning()
    {
        goldWarningUI.SetActive(true);
        Invoke("HideGoldWarning", 1.5f); // 1.5�� �� �ڵ� ����
    }

    void HideGoldWarning()
    {
        goldWarningUI.SetActive(false);
    }

    public void CloseRankingPanel()
    {
        rankingPanel.SetActive(false);
    }

    public void GameStart()
    {
        
        SceneManager.LoadScene("PlayScene");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Level_0"); // ���� ȭ�� �� �̸� �ֱ�
    }

    public void OnGameStartButtonClicked()
    {

        string playerName = inputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("�÷��̾� �̸��� �Է��ϼ���");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("�÷��̾� �̸� ���� �� : " + playerName);

        SceneManager.LoadScene("PlayScene");
    }

    public void ShowRanking(List<RankingEntry> rankings)
    {
        
        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        // �� ��ŷ �׸� �߰�
        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingContent);
            TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
            text.text = $"{i + 1}. {rankings[i].playerName} - {rankings[i].score}��";
        }
    }

    public void ShowGameClearUI()
    {
        if (gameClearPanel !=null)
        {
            gameClearPanel.SetActive(true);
        }
    }


    public void GameExit()
    {
        Application.Quit();
    }

    
   public void ShowShopUI(bool isOpen)
    {
        shopPanel.SetActive(isOpen);
    }





    public void UpdateDay(int day)
    {
        dayText.text = "Day " + day;
    }

    public void UpdateScore(int score)
    {
        Debug.Log("UIManager ���� ������Ʈ ȣ�� ��: " + score);
        scoreText.text = "Score :" + score;
    }

    public void UpdateTimer(int seconds)
    {
        timerText.text = "Time: " + seconds + "s";
    }

    public void UpdateDayNightUI(bool isNight)
    {
        Debug.Log("���� UI ������Ʈ: " + (isNight ? "��" : "��"));


        if (dayNightImage == null || daySprite == null || nightSprite == null)
        {
            Debug.LogWarning("���� UI �̹����� ����Ǿ� ���� �ʾƿ�!");
            return;
        }

        dayNightImage.sprite = isNight ? nightSprite : daySprite;

        if (nightOverlay != null)
        {
            nightOverlay.SetActive(isNight);
        }
    }




}
