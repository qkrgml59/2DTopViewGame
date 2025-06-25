using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Inspector에서 늑대 프리팹 연결
    public Transform spawnPoint;   // 생성 위치
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
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0); // z를 0으로
        spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Debug.Log("늑대 생성 완료!");
    }


}
