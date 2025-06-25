using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Inspector���� ���� ������ ����
    public Transform spawnPoint;   // ���� ��ġ
    private GameObject spawnedEnemy;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.isNight && spawnedEnemy == null)
        {
            SpawnEnemy();
        }
        else if (!gameManager.isNight && spawnedEnemy != null)
        {
            Destroy(spawnedEnemy);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0); // z�� 0����
        spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Debug.Log("���� ���� �Ϸ�!");
    }


}
