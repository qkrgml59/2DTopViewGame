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


    [Header("�ؽ�Ʈ UI")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI score;
    

    [Header("��ŷ UI")]
    public TMP_InputField inputField;
    public Button gameStartButton;

    [Header("���� �� UI")]
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
            Debug.Log("�÷��̾� �̸��� �Է��ϼ���");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("�÷��̾� �̸� ���� �� : " + playerName);

        SceneManager.LoadScene("PlayScene");
    }

   


    public void GameExit()
    {
        Application.Quit();
    }

    
   

    private void Awake()
    {
        // �̱��� �������� ��𼭵� ���� �����ϰ�
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
