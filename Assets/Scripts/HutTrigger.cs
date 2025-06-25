using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutTrigger : MonoBehaviour
{
    GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gm.hutOpen)
            {
                Debug.Log("오두막에 들어감 생존 성공!");

                // 진짜 Player만 대상으로
                Player playerScript = other.GetComponent<Player>();
                if (playerScript != null)
                {
                    StartCoroutine(FadeOutPlayer(playerScript.gameObject));
                }
            }
            else
            {
                Debug.Log("점수가 부족해서 문이 안 열려!");
            }
        }
    }

    IEnumerator FadeOutPlayer(GameObject player)
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer 없음! : " + player.name);
            yield break;
        }

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(0.02f);
        }

        // 꺼지기 전에 위치 백업
        Vector3 pos = player.transform.position;

        player.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        GameManager.Instance.NextDay();

        // 플레이어 다시 복원하고 위치 초기화 (원한다면)
        player.SetActive(true);
        player.transform.position = pos;
    }
}