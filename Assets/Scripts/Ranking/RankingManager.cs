using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
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
    public RankingData currentData = new RankingData();

    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
        LoadRanking();
    }

    public void AddRanking(string playerName, int score)
    {
        currentData.rankings.Add(new RankingEntry { playerName = playerName, score = score });
        currentData.rankings.Sort((a, b) => b.score.CompareTo(a.score));
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
            Debug.Log("랭킹 불러오기 완료");
        }

    }

    void GameOver()
    {
        FindObjectOfType<RankingManager>().AddRanking("플레이어1", 350);
    }
}
