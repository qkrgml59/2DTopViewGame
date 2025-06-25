using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ShopTrigger : MonoBehaviour
{
    public GameObject shopPanel; // ���� �г�
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
                Debug.Log("������ ���Ƚ��ϴ�."); // ����� ����
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("���� ��ó�� �����߽��ϴ�.");
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
                Debug.Log("�������� �־������ϴ�.");
            }
        }
    }
}