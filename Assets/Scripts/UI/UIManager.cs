using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
   

    [Header("�ؽ�Ʈ UI")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("��ŷ UI")]
    public GameObject rankingPanel;
    public TextMeshProUGUI rankingListText;
    public TMP_InputField playerNameInput;

    public RankingManager rankingManager;

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
        scoreText.text = "Score: " + score;
    }

    public void UpdateTimer(int seconds)
    {
        timerText.text = "Time: " + seconds + "s";
    }

    public void ShowRanking()
    {
        rankingPanel.SetActive(true);
        rankingListText.text = "";

        int rank = 1;
        foreach (RankingEntry entry in rankingManager.currentData.rankings)
        {
            rankingListText.text += $"{rank}. {entry.playerName} - {entry.score}��\n";
            rank++;
        }
    }

    public string GetPlayerName()
    {
        return playerNameInput.text;
    }
}
