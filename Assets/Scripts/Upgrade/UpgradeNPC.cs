using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNPC : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            UpgradeManager.Instance.OpenUpgradePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("강화 NPC에 접근. E를 눌러 상호작용하세요.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}

