using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 2f;
    public int cropCount = 0;         //���ݱ��� ��Ȯ�� �۹� ��
    public int totalScore = 0;        //�÷��̾ ���� �� ����

    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;

    Vector2 input;
    Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        velocity = input.normalized * moveSpeed;

        if(input.sqrMagnitude > .01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    sR.sprite = spriteRight;
                else if (input.x < 0)
                    sR.sprite = spriteLeft;
            }
            else
            {
                if (input.y > 0)
                    sR.sprite = spriteUp;
                else
                    sR.sprite = spriteDown;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))           //�÷��̾ Z �� ������ ��
        {
            TryHarvest();                    //�Լ� �ߵ�
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void TryHarvest()
    {
        //������ Ȯ��
        if (!FindObjectOfType<DayNightController>().isDay)                    //���� ������ Ȯ��
        {
            Debug.Log("�㿡�� ä�� �� �� �����ϴ�.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f);            //�÷��̾� �ֺ� 1.0 �ݰ��� ������Ʈ���� ��� ������

        foreach (var hit in hits)
        {
            Crop crop = hit.GetComponent<Crop>();       //�ֺ� ������Ʈ�� �� crop�� �پ��ְ� ��Ȯ �� �Ȱ͸� ä��
            if (crop != null && !crop.isHarvested)
            {
                crop.isHarvested = true;              //�ٽ� ä�� ���ϰ� ����
                Destroy(crop.gameObject);                   //�۹� ����
                cropCount++;                               //�۹� ��Ȯ �� ����
                totalScore += crop.score;                      // ä�� ���� �߰�
                Debug.Log("�۹� ��Ȯ �� ��Ȯ �� : " + crop.score + "�� | �� ���� : " + totalScore);
                return;
            }
        }
    }
}
