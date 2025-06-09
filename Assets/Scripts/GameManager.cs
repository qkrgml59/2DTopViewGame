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

    public int timer = 0;      //�� ���� ���� �ð�
    public int dayDuration = 10;    //�� ���� �ð� (��)
    public int nightDuration = 10;   //�� ���� �ð� (��)

    private float secondCounter = 0f;     // 1�� ���� üũ��
    public static GameManager Instance;
   

    public GameObject[] cropPrefabs;       //�۹� ������ 7��
    public Transform[] cropSpawnPoints;     // �۹� ���� ��ġ��


    // Start is called before the first frame update
    void Start()
    {
        SpawnCrops();     //ù �� ������ �� �۹� ����
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

                // �� ���� �� ����
                isNight = true;
                timer = 0;
                Debug.Log("�� ����");

                UIManager.Instance.UpdateDayNightUI(isNight);
            }
            else if (isNight && timer >= nightDuration)
            {

                //�� ���� �� ���� + ������
                isNight = false;
                timer = 0;
                day++;
                Debug.Log("���� �� : " + day);

                UIManager.Instance.UpdateDay(day);
                UIManager.Instance.UpdateDayNightUI(isNight);
                SpawnCrops();
            }
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

    public void AddScore(int value)
{
    score += value;
    UIManager.Instance.UpdateScore(score);  // UI ����
}
    public void SpawnCrops()              //�۹� ���� �Լ�
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

            Debug.Log("���� ���� �Ѿ�ϴ�. ���� �ϼ� " + day);

            SpawnCrops();
        }
        else
        {
            Debug.Log("��ǥ ������ �������� ���ؼ� ���θ� ���� ���� �ֽ��ϴ�.");
        }
    }

}
