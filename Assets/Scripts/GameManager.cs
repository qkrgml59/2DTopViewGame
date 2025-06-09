using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public int score = 0;
    public int goalScore = 100;
    public int day = 1;

    public bool isNight = false;
    public bool hutOpen = false;
    public GameObject hutDoor;

    public int timer = 0;      //초 단위 누적 시간
    public int dayDuration = 10;    //낮 지속 시간 (초)
    public int nightDuration = 10;   //밤 지속 시간 (초)

    private float secondCounter = 0f;     // 1초 단위 체크용
    public static GameManager Instance;
   

    public GameObject[] cropPrefabs;       //작물 프리팹 7개
    public Transform[] cropSpawnPoints;     // 작물 생성 위치들


    // Start is called before the first frame update
    void Start()
    {
        SpawnCrops();     //첫 날 시작할 때 작물 생성
    }

   


    // Update is called once per frame
    void Update()
    {

        secondCounter += Time.deltaTime;
        if (secondCounter >= 1f)
        {
            secondCounter = 0f;
            timer++;

            UIManager.Instance.UpdateTimer(timer);

            if (!isNight && timer >= dayDuration)
            {

                // 낮 종료 밤 시작
                isNight = true;
                timer = 0;
                Debug.Log("밤 시작");

                UIManager.Instance.UpdateDayNightUI(isNight);
            }
            else if (isNight && timer >= nightDuration)
            {

                //밤 종료 낮 시작 + 다음날
                isNight = false;
                timer = 0;
                day++;
                Debug.Log("다음 날 : " + day);

                UIManager.Instance.UpdateDay(day);
                UIManager.Instance.UpdateDayNightUI(isNight);
                SpawnCrops();
            }
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

    public void AddScore(int value)
{
    score += value;
    UIManager.Instance.UpdateScore(score);  // UI 갱신
}
    public void SpawnCrops()              //작물 생성 함수
    {
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }
        
        for (int i = 0; i < Mathf.Min(cropSpawnPoints.Length, cropPrefabs.Length); i++)
            {
            Instantiate(cropPrefabs[i], cropSpawnPoints[i].position, Quaternion.identity);
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

}
