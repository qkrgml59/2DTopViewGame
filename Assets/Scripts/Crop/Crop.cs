using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public string cropName; // 작물 이름
    public int score;       // 작물 점수
    public bool isHarvested = false; // 수확 여부

    private void Start()
    {
        // 필요하면 초기화
    }
}