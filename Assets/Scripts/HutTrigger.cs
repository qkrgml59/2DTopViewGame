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

                // �÷��̾ ����ȭ ��Ű��
                StartCoroutine(FadeOutPlayer(other.gameObject));
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

        // õõ�� ���İ� ���̱�
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(0.02f);
        }

        // ������ ���������� �������� �Ѿ��
        player.SetActive(false);
        GameManager.Instance.NextDay();
    }
}