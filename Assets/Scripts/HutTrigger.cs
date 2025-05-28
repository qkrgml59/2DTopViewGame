using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutTrigger : MonoBehaviour
{
    GameManager gm;

    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(gm.hutOpen)
            {
                Debug.Log("���θ��� �� ���� ����!");
            }
            else
            {
                Debug.Log("������ �����ؼ� ���� �� ����!");
            }
        }
    }
}

    // Update is called once per frame
    

