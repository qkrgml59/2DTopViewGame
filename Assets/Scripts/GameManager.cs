using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    
  public int score = 0;
    public int goalScore = 100;
    public int day = 1;
    public int Gold = 0;

    public bool isNight = false;
    public bool hutOpen = false;
    public GameObject hutDoor;

    public int timer = 20;      //초 단위 누적 시간
    public int dayDuration = 20;    //낮 지속 시간 (초)
    public int nightDuration = 10;   //밤 지속 시간 (초)

    private float secondCounter = 0f;     // 1초 단위 체크용
    public static GameManager Instance;
   

    public GameObject[] cropPrefabs;       //작물 프리팹 7개
         // 작물 생성 위치들

    private List<SpawnPoint> cropSpawnPoints = new List<SpawnPoint>();


    // Start is called before the first frame update
    void Start()
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

   


    // Update is called once per frame
    void Update()
    {

        secondCounter += Time.deltaTime;

        if (secondCounter >= 1f)
        {
            secondCounter = 0f;

            if (!isNight)
            {
                // 낮: 시간이 감소
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
                // 밤: 시간이 증가
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

        //밤에 목표 점수 달성시 오두막 문 열기
        if (isNight && score >= goalScore)
        {
            hutOpen = true;
            hutDoor.SetActive(false); // 문 비활성화 = 열린 상태
        }
        else if (isNight)
        {
            hutOpen = false;
            hutDoor.SetActive(true);
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void SpawnCrops()
    {
        // 기존 작물 삭제
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }

        // 스폰 포인트마다 설정된 작물 생성
        foreach (SpawnPoint spawnPoint in cropSpawnPoints)
        {
            Instantiate(spawnPoint.cropPrefab, spawnPoint.transform.position, Quaternion.identity);
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
            timer = 0;

            Debug.Log("다음 날로 넘어갑니다. 현재 일수 " + day);

            SpawnCrops();
        }
        else
        {
            Debug.Log("목표 점수에 도달하지 못해서 오두막 문이 닫혀 있습니다.");
        }
    }

    public void GameOver()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");

        FindObjectOfType<RankingManager>().AddRanking(playerName, score);

        UIManager.Instance.ShowRanking(FindObjectOfType<RankingManager>().currentData.rankings);
    }

}
