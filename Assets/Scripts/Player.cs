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
    public Sprite harvestUp;
    public Sprite harvestDown;
    public Sprite harvestLeft;
    public Sprite harvestRight;

    Rigidbody2D rb;
    SpriteRenderer sR;

    Vector2 input;
    Vector2 velocity;

    bool isHarvesting = false;
    float animTimer = 0f;
    int animFrame = 0;
    float frameRate = 0.1f;

    GameManager gameManager;   // 🔥 GameManager 저장 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameManager = FindObjectOfType<GameManager>();  // 🔥 GameManager 한번만 찾기
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            AnimateDirection();
        }

        if (Input.GetKeyDown(KeyCode.Z))           // 플레이어가 Z를 눌렀을 때
        {
            TryHarvest();                    // 채집 시도
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;  // ✔️ MovePosition 대신 velocity 사용 (물 충돌 적용)
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
        // 🔥 GameManager를 참조해서 낮/밤 확인
        if (gameManager.isNight)
        {
            Debug.Log("밤에는 채집할 수 없습니다.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);  // 채집 범위 0.5로 확대

        foreach (var hit in hits)
        {
            Crop crop = hit.GetComponent<Crop>();  // Crop이 붙은 오브젝트 찾기
            if (crop != null && !crop.isHarvested)
            {
                crop.isHarvested = true;              // 재채집 방지
                Destroy(crop.gameObject);             // 작물 제거
                cropCount++;                          // 작물 수량 증가
                totalScore += crop.score;             // 점수 추가

                ShowHarvestAnimation();

                // UI 업데이트
                UIManager.Instance.UpdateScore(totalScore);

                Debug.Log("작물 수확! 점수: " + crop.score + " | 총 점수: " + totalScore);
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
        Sprite originalSprite = sR.sprite;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            sR.sprite = input.x > 0 ? harvestRight : harvestLeft;
        }
        else
        {
            sR.sprite = input.y > 0 ? harvestUp : harvestDown;
        }

        yield return new WaitForSeconds(0.2f);
        sR.sprite = originalSprite;
    }
}