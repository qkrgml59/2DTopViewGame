using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

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

    [Header("낮과 밤 UI")]
    public Image dayNightImage;
    public Sprite daySprite;
    public Sprite nightSprite;
    public GameObject nightOverlay;




    public RankingManager rankingManager;
    public GameObject helpUI;
    public GameObject RankingUI;

    private void Start()
    {
        Instance = this;
    }

    public void GameStart()
    {
        // gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        SceneManager.LoadScene("PlayScene");
    }

    private void OnGameStartButtonClicked()
    {
        string playerName = inputField.text;
        if (string.IsNullOrEmpty(playerName) )
        {
            Debug.Log("플레이어 이름을 입력하세요");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("플레이어 이름 저장 됨 : " + playerName);

        SceneManager.LoadScene("PlayScene");
    }

   


    public void GameExit()
    {
        Application.Quit();
    }

    
   

    private void Awake()
    {
        // 싱글톤 패턴으로 어디서든 접근 가능하게
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
