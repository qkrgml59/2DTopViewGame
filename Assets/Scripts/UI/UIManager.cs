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


    public Button rankingOpenButton;
    public Button rankingCloseButton;

    public RankingManager rankingManager;


    public GameObject helpUI;
    public GameObject RankingUI;

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

    public void CloseRankingPanel()
    {
        rankingPanel.SetActive(false);
    }

    public void GameStart()
    {
        // gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        SceneManager.LoadScene("PlayScene");
    }

    private void OnGameStartButtonClicked()
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
        // ���� ��ŷ �׸� ����
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




    public void GameExit()
    {
        Application.Quit();
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
