using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DayNightController : MonoBehaviour
{
    public int timeCount = 0;     //���� �ð�
    public int dayCount = 1;      //���� �� (1�� 2�� )
    public bool isDay = true;     // ���� ��/ �� ����

    private void Start()
    {
        InvokeRepeating("TimeTick", 1f, 1f);    // �� �ʸ��� TimeTick ����
    }

    void TimeTick()
    {
        timeCount++;

        if (isDay && timeCount >= 10)  // ���� 60�ʰ� ������ ������ ��ȯ
        {
            isDay = false;
            timeCount = 0;
            Debug.Log("���� ���۵Ǿ����ϴ�.");
        }
        else if (!isDay && timeCount >= 10)  //�� ���¿��� 10�ʰ� ������ ������ 
        {
            isDay = true;
            timeCount = 0;
            dayCount++;
            Debug.Log("���� ���۵Ǿ����ϴ�. ���� ���� �Ѿ�ڽ��ϴ�. ���� �ϼ� : " + dayCount);
        }
    }
}
