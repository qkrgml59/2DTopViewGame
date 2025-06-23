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

    public int timer = 20;      //�� ���� ���� �ð�
    public int dayDuration = 20;    //�� ���� �ð� (��)
    public int nightDuration = 10;   //�� ���� �ð� (��)

    private float secondCounter = 0f;     // 1�� ���� üũ��
    public static GameManager Instance;
   

    public GameObject[] cropPrefabs;       //�۹� ������ 7��
         // �۹� ���� ��ġ��

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
                // ��: �ð��� ����
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
                // ��: �ð��� ����
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

        //�㿡 ��ǥ ���� �޼��� ���θ� �� ����
        if (isNight && score >= goalScore)
        {
            hutOpen = true;
            hutDoor.SetActive(false); // �� ��Ȱ��ȭ = ���� ����
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
        // ���� �۹� ����
        GameObject[] oldCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in oldCrops)
        {
            Destroy(crop);
        }

        // ���� ����Ʈ���� ������ �۹� ����
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

            Debug.Log("���� ���� �Ѿ�ϴ�. ���� �ϼ� " + day);

            SpawnCrops();
        }
        else
        {
            Debug.Log("��ǥ ������ �������� ���ؼ� ���θ� ���� ���� �ֽ��ϴ�.");
        }
    }

    public void GameOver()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");

        FindObjectOfType<RankingManager>().AddRanking(playerName, score);

        UIManager.Instance.ShowRanking(FindObjectOfType<RankingManager>().currentData.rankings);
    }

}
