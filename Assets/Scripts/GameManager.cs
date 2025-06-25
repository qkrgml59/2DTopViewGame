using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int goalScore = 100;
    public int day = 1;

    public bool isNight = false;
    public bool hutOpen = false;

    public GameObject hutDoor;

    public int timer = 20;
    public int dayDuration = 20;
    public int nightDuration = 10;

    private float secondCounter = 0f;

    public GameObject[] cropPrefabs;

    private List<SpawnPoint> cropSpawnPoints = new List<SpawnPoint>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (GameObject obj in spawnObjects)
        {
            SpawnPoint spawnPoint = obj.GetComponent<SpawnPoint>();
            if (spawnPoint != null)
            {
                cropSpawnPoints.Add(spawnPoint);
            }
        }

        SpawnCrops();
    }

    private void Update()
    {
        secondCounter += Time.deltaTime;

        if (secondCounter >= 1f)
        {
            secondCounter = 0f;

            if (!isNight)
            {
                timer--;

                if (timer <= 0)
                {
                    isNight = true;
                    timer = 0;
                    Debug.Log("밤 시작");
                }
            }
            else
            {
                timer++;

                if (timer >= nightDuration)
                {
                    isNight = false;
                    day++;
                    timer = dayDuration;

                    Debug.Log("다음 날 : " + day);
                    SpawnCrops();
                }
            }

            UIManager.Instance.UpdateDay(day);
            UIManager.Instance.UpdateDayNightUI(isNight);
            UIManager.Instance.UpdateTimer(timer);
        }
    }

    public void CheckScore(int currentScore)
    {
        if (currentScore >= goalScore && !hutOpen)
        {
            hutOpen = true;
            hutDoor.SetActive(false); // 문 열기
            Debug.Log("점수 도달 문이 열렸습니다.");
        }
    }

    public void NextDay()
    {
        if (isNight && hutOpen)
        {
            isNight = false;
            day++;
            score = 0;
            hutOpen = false;
            hutDoor.SetActive(true);
            timer = dayDuration;

            goalScore += 50; // 목표 점수 +50씩 증가

            Debug.Log("다음 날로 넘어갑니다. 현재 일수: " + day + " | 새로운 목표 점수: " + goalScore);

            SpawnCrops();
        }
        else
        {
            Debug.Log("목표 점수에 도달하지 못해서 오두막 문이 닫혀 있습니다.");
        }
    }

    public void SpawnCrops()
    {
        // 기존 작물 삭제
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }

        // 스폰 포인트마다 작물 생성
        foreach (SpawnPoint spawnPoint in cropSpawnPoints)
        {
            Instantiate(spawnPoint.cropPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    public void GameOver()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");

        FindObjectOfType<RankingManager>().AddRanking(playerName, score);

        UIManager.Instance.ShowRanking(FindObjectOfType<RankingManager>().currentData.rankings);

        InventoryManager.Instance.SaveGold();

        SceneManager.LoadScene("GameOverScene");
    }

    public void StartNight()
    {
        isNight = true;
    }

    public void StartDay()
    {
        isNight = false;
    }
}