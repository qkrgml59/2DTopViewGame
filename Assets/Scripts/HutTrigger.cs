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

                // 플레이어를 투명화 시키기
                StartCoroutine(FadeOutPlayer(other.gameObject));
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

        // 천천히 알파값 줄이기
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(0.02f);
        }

        // 완전히 투명해지면 다음날로 넘어가기
        player.SetActive(false);
        GameManager.Instance.NextDay();
    }
}