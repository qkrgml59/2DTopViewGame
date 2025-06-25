using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.isNight)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x < -10f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("적이 플레이어를 잡았다! 게임오버!");
            gameManager.GameOver(); // 게임오버 함수 호출
        }
    }
}
