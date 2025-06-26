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

  
    private GameObject player;
    private SpriteRenderer playerSR;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

       
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerSR = player.GetComponent<SpriteRenderer>();
        }
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
            hutDoor.SetActive(false);
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
            UIManager.Instance.UpdateScore(score);
            hutOpen = false;
            hutDoor.SetActive(true);
            timer = dayDuration;

            goalScore += 50;

            Debug.Log("다음 날로 넘어갑니다. 현재 일수: " + day + " | 새로운 목표 점수: " + goalScore);

            SpawnCrops();

           
            Debug.Log("플레이어 복구 시도");

            if (player == null)
            {
                Debug.LogWarning("Player를 찾지 못했습니다!");
            }
            else
            {
                player.SetActive(true);
                Debug.Log("플레이어 활성화 성공");

                if (playerSR != null)
                {
                    playerSR.color = new Color(1f, 1f, 1f, 1f); // 완전 불투명
                    Debug.Log("알파값 복원 완료");
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer가 없습니다!");
                }
            }
        }
        else
        {
            Debug.Log("목표 점수에 도달하지 못해서 오두막 문이 닫혀 있습니다.");
        }
    }

    public void SpawnCrops()
    {
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }

        foreach (SpawnPoint spawnPoint in cropSpawnPoints)
        {
            Instantiate(spawnPoint.cropPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    public void GameOver()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");

     
        RankingManager rankingManager = FindObjectOfType<RankingManager>();
        if (rankingManager != null)
        {
            rankingManager.AddRanking(playerName, score); // 랭킹 추가 & 저장
        }
        else
        {
            Debug.LogWarning("RankingManager를 찾을 수 없습니다.");
        }

      
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.SaveGold();
        }

        Debug.Log("게임오버 - 씬 전환");

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