using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Player player;

    public void UpgradeHarvestRange()
    {
        player.IncreaseHarvestRange(0.2f); // 채집 범위 0.2씩 증가
        player.IncreaseMoveSpeed(0.5f);
    }
}
