using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public int score = 10;               //�� �۹��� ����
    public bool isHarvested = false;  // �̹� ��Ȯ�Ȱ��� Ȯ��

    public string cropName = "����";

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
