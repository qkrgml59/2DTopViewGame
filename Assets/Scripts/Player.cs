using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 2f;
    public int cropCount = 0;         //���ݱ��� ��Ȯ�� �۹� ��
    public int totalScore = 0;        //�÷��̾ ���� �� ����

    [Header("�ִϸ��̼� �����ӵ�")]
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    [Header("ä�� ������")]
    public Sprite[] harvestUpSprites;
    public Sprite[] harvestDownSprites;
    public Sprite[] harvestLeftSprites;
    public Sprite[] harvestRightSprites;




    Rigidbody2D rb;
    SpriteRenderer sR;

    Vector2 input;
    Vector2 velocity;

    bool isHarvesting = false;
    float animTimer = 0f;
    int animFrame = 0;
    float frameRate = 0.1f;

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

        if (input.sqrMagnitude > 0.2f)
        {
            AnimateDirection();
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

    void AnimateDirection()
    {
        if (input.sqrMagnitude > 0.01f)
        {


            animTimer += Time.deltaTime;
            if (animTimer >= frameRate)
            {
                animTimer = 0f;
                animFrame++;
            }

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    sR.sprite = rightSprites[animFrame % rightSprites.Length];
                else
                    sR.sprite = leftSprites[animFrame % leftSprites.Length];
            }
            else
            {
                if (input.y > 0)
                    sR.sprite = upSprites[animFrame % upSprites.Length];
                else
                    sR.sprite = downSprites[animFrame % downSprites.Length];
            }
        }
        else
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                sR.sprite = input.x > 0 ? rightSprites[0] : leftSprites[0];
            }
            else
            {
                sR.sprite = input.y > 0 ? upSprites[0] : downSprites[0];
            }
        }
    }

    void TryHarvest()
    {
        //������ Ȯ��
        if (!FindObjectOfType<DayNightController>().isDay)                    //���� ������ Ȯ��
        {
            Debug.Log("�㿡�� ä�� �� �� �����ϴ�.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);            //�÷��̾� �ֺ� 1.0 �ݰ��� ������Ʈ���� ��� ������

        foreach (var hit in hits)
        {
            Crop crop = hit.GetComponent<Crop>();       //�ֺ� ������Ʈ�� �� crop�� �پ��ְ� ��Ȯ �� �Ȱ͸� ä��
            if (crop != null && !crop.isHarvested)
            {
                crop.isHarvested = true;              //�ٽ� ä�� ���ϰ� ����
                Destroy(crop.gameObject);                   //�۹� ����
                cropCount++;                               //�۹� ��Ȯ �� ����
                totalScore += crop.score;

                ShowHarvestAnimation();
                // ä�� ���� �߰�
                Debug.Log("�۹� ��Ȯ �� ��Ȯ �� : " + crop.score + "�� | �� ���� : " + totalScore);
                return;
            }
        }
    }

    void ShowHarvestAnimation()
    {
        StopCoroutine("HarvestRoutine");
        StartCoroutine("HarvestRoutine");
    }

    IEnumerator HarvestRoutine()
    {
        isHarvesting = true;
        animFrame = 0;
        animTimer = 0f;

        Sprite[] currentHarvestSprites;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            currentHarvestSprites = input.x > 0 ? harvestRightSprites : harvestLeftSprites;
        }
        else
        {
            currentHarvestSprites = input.y > 0 ? harvestUpSprites : harvestDownSprites;
        }

        float duration = currentHarvestSprites.Length * frameRate;

        while (animTimer < duration)
        {
            int index = Mathf.FloorToInt(animTimer / frameRate);
            index = Mathf.Clamp(index, 0, currentHarvestSprites.Length - 1);
            sR.sprite = currentHarvestSprites[index];

            animTimer += Time.deltaTime;
            yield return null;
        }

        isHarvesting = false;
    }
}
