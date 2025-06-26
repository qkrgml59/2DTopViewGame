using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class RankingEntry
{
    public string playerName;
    public int score;

}
[System.Serializable]
public class RankingData
{
    public List<RankingEntry> rankings = new List<RankingEntry>();
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    public RankingData currentData = new RankingData();

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �� �Ѿ�� ����ְ� �����
            filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
            LoadRanking();
        }
        else
        {
            Destroy(gameObject);  // �ߺ� ���� ����
        }

        filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
        LoadRanking();
    }

    public void AddRanking(string playerName, int score)
    {
        currentData.rankings.Add(new RankingEntry { playerName = playerName, score = score });
        currentData.rankings.Sort((a, b) => b.score.CompareTo(a.score)); // �������� ����
        SaveRanking();
    }

    public void SaveRanking()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("��ŷ ���� �Ϸ�");
    }

    public void LoadRanking()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentData = JsonUtility.FromJson<RankingData>(json);
            Debug.Log("��ŷ �ε� �Ϸ�");
        }
        else
        {
            Debug.Log("��ŷ ������ �������� �ʽ��ϴ�. ���� �����մϴ�.");
        }
    }
        public void GameOverSave()
        {
        string playerName = PlayerPrefs.GetString("PlayerName", "�÷��̾�");
        int finalScore = FindObjectOfType<GameManager>().score;
        AddRanking(playerName, finalScore);
    }

    public void Start()
    {
        UIManager.Instance.ShowRanking(currentData.rankings);
    }

    public void ShowRankingUI()
    {
        UIManager.Instance.ShowRanking(currentData.rankings);
    }

    public void OnRankingButtonCliked()
    {
        FindObjectOfType<RankingManager>().ShowRankingUI();
    }
}

