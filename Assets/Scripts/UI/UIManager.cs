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


    [Header("텍스트 UI")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI score;


    [Header("랭킹 UI")]
    public TMP_InputField inputField;
    public Button gameStartButton;
    public GameObject rankingPanel;
    public TextMeshProUGUI rankingText;
    public Transform rankingContent;
    public GameObject rankingItemPrefab;

    [Header("낮과 밤 UI")]
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
            text.text = $"{i + 1}.  {rankings[i].playerName} - {rankings[i].score}점";
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
            Debug.Log("플레이어 이름을 입력하세요");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("플레이어 이름 저장 됨 : " + playerName);

        SceneManager.LoadScene("PlayScene");
    }

    public void ShowRanking(List<RankingEntry> rankings)
    {
        // 기존 랭킹 항목 제거
        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        // 새 랭킹 항목 추가
        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingContent);
            TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
            text.text = $"{i + 1}. {rankings[i].playerName} - {rankings[i].score}점";
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
        Debug.Log("UIManager 점수 업데이트 호출 됨: " + score);
        scoreText.text = "Score :" + score;
    }

    public void UpdateTimer(int seconds)
    {
        timerText.text = "Time: " + seconds + "s";
    }

    public void UpdateDayNightUI(bool isNight)
    {
        Debug.Log("낮밤 UI 업데이트: " + (isNight ? "밤" : "낮"));


        if (dayNightImage == null || daySprite == null || nightSprite == null)
        {
            Debug.LogWarning("낮밤 UI 이미지가 연결되어 있지 않아요!");
            return;
        }

        dayNightImage.sprite = isNight ? nightSprite : daySprite;

        if (nightOverlay != null)
        {
            nightOverlay.SetActive(isNight);
        }
    }




}
