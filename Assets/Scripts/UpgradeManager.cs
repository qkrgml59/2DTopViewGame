using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Player player;

    public void UpgradeHarvestRange()
    {
        player.IncreaseHarvestRange(0.2f); // ä�� ���� 0.2�� ����
        player.IncreaseMoveSpeed(0.5f);
    }
}
