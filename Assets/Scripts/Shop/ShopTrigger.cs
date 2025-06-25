using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ShopTrigger : MonoBehaviour
{
    public GameObject shopPanel; // 상점 패널
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
                Debug.Log("상점이 열렸습니다."); // 디버그 찍어보기
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("상점 근처에 접근했습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
                Debug.Log("상점에서 멀어졌습니다.");
            }
        }
    }
}