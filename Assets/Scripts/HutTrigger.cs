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
                Debug.Log("오두막에 들어감 생존 성공!");
            }
            else
            {
                Debug.Log("점수가 부족해서 문이 안 열려!");
            }
        }
    }
}

    // Update is called once per frame
    

