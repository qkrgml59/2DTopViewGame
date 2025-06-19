using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public int score = 10;               //이 작물의 점수
    public bool isHarvested = false;  // 이미 수확된건지 확인

    public string cropName = "감자";

    private void OnMouseDown()
    {
        InventoryManager.Instance.AddItem(cropName);
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
