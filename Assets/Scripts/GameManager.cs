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
                    Debug.Log("�� ����");
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

                    Debug.Log("���� �� : " + day);
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
            hutDoor.SetActive(false); // �� ����
            Debug.Log("���� ���� ���� ���Ƚ��ϴ�.");
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

            goalScore += 50; // ��ǥ ���� +50�� ����

            Debug.Log("���� ���� �Ѿ�ϴ�. ���� �ϼ�: " + day + " | ���ο� ��ǥ ����: " + goalScore);

            SpawnCrops();
        }
        else
        {
            Debug.Log("��ǥ ������ �������� ���ؼ� ���θ� ���� ���� �ֽ��ϴ�.");
        }
    }

    public void SpawnCrops()
    {
        // ���� �۹� ����
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }

        // ���� ����Ʈ���� �۹� ����
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