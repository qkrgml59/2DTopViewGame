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
                Debug.Log("���θ��� �� ���� ����!");

                // ��¥ Player�� �������
                Player playerScript = other.GetComponent<Player>();
                if (playerScript != null)
                {
                    StartCoroutine(FadeOutPlayer(playerScript.gameObject));
                }
            }
            else
            {
                Debug.Log("������ �����ؼ� ���� �� ����!");
            }
        }
    }

    IEnumerator FadeOutPlayer(GameObject player)
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer ����! : " + player.name);
            yield break;
        }

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(0.02f);
        }

        // ������ ���� ��ġ ���
        Vector3 pos = player.transform.position;

        player.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        GameManager.Instance.NextDay();

        // �÷��̾� �ٽ� �����ϰ� ��ġ �ʱ�ȭ (���Ѵٸ�)
        player.SetActive(true);
        player.transform.position = pos;
    }
}