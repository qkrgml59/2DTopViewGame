using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DayNightController : MonoBehaviour
{
    public int timeCount = 0;     //현재 시간
    public int dayCount = 1;      //현재 일 (1일 2일 )
    public bool isDay = true;     // 현재 낮/ 밤 상태

    private void Start()
    {
        InvokeRepeating("TimeTick", 1f, 1f);    // 매 초마다 TimeTick 실행
    }

    void TimeTick()
    {
        timeCount++;

        if (isDay && timeCount >= 10)  // 낮이 60초가 지나면 밤으로 전환
        {
            isDay = false;
            timeCount = 0;
            Debug.Log("밤이 시작되었습니다.");
        }
        else if (!isDay && timeCount >= 10)  //밤 상태에서 10초가 지나면 낮으로 
        {
            isDay = true;
            timeCount = 0;
            dayCount++;
            Debug.Log("낮이 시작되었습니다. 다음 날로 넘어가겠습니다. 현재 일수 : " + dayCount);
        }
    }
}
