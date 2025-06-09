using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 2f;
    public int cropCount = 0;         //지금까지 수확한 작물 수
    public int totalScore = 0;        //플레이어가 모은 총 점수

    [Header("애니메이션 프레임들")]
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    [Header("채집 프레임")]
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

        if (Input.GetKeyDown(KeyCode.Z))           //플레이어가 Z 를 눌렀을 때
        {
            TryHarvest();                    //함수 발동
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
        //낮인지 확인
        if (!FindObjectOfType<DayNightController>().isDay)                    //현재 밤인지 확인
        {
            Debug.Log("밤에는 채집 할 수 없습니다.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);            //플레이어 주변 1.0 반경의 오브젝트들을 모두 가져옴

        foreach (var hit in hits)
        {
            Crop crop = hit.GetComponent<Crop>();       //주변 오브젝트들 중 crop이 붙어있고 수확 안 된것만 채집
            if (crop != null && !crop.isHarvested)
            {
                crop.isHarvested = true;              //다시 채집 못하게 설정
                Destroy(crop.gameObject);                   //작물 제거
                cropCount++;                               //작물 수확 수 증가
                totalScore += crop.score;

                ShowHarvestAnimation();
                // 채집 점수 추가
                Debug.Log("작물 수확 총 수확 수 : " + crop.score + "점 | 총 점수 : " + totalScore);
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
