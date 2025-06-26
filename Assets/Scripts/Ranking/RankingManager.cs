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
            DontDestroyOnLoad(gameObject);  // 씬 넘어가도 살아있게 만들기
            filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
            LoadRanking();
        }
        else
        {
            Destroy(gameObject);  // 중복 생성 방지
        }

        filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
        LoadRanking();
    }

    public void AddRanking(string playerName, int score)
    {
        currentData.rankings.Add(new RankingEntry { playerName = playerName, score = score });
        currentData.rankings.Sort((a, b) => b.score.CompareTo(a.score)); // 내림차순 정렬
        SaveRanking();
    }

    public void SaveRanking()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("랭킹 저장 완료");
    }

    public void LoadRanking()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentData = JsonUtility.FromJson<RankingData>(json);
            Debug.Log("랭킹 로드 완료");
        }
        else
        {
            Debug.Log("랭킹 파일이 존재하지 않습니다. 새로 생성합니다.");
        }
    }
        public void GameOverSave()
        {
        string playerName = PlayerPrefs.GetString("PlayerName", "플레이어");
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

