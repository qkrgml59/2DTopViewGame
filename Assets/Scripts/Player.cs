using UnityEngine;
using System.Collections; // 이거 꼭 있어야 함 (IEnumerator를 인식하려면 필수)
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float harvestRange = 0.5f; // 채집 범위
    public int cropCount = 0;
    public int totalScore = 0;

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

    float animTimer = 0f;
    int animFrame = 0;
    float frameRate = 0.1f;

    public int money = 0;

    GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        velocity = input.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            TryHarvest();  // ⭐️ 이거 호출하면 됨
        }

        if (input.sqrMagnitude > 0.01f)
        {
            AnimateDirection();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
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
                sR.sprite = input.x > 0 ? rightSprites[animFrame % rightSprites.Length] : leftSprites[animFrame % leftSprites.Length];
            }
            else
            {
                sR.sprite = input.y > 0 ? upSprites[animFrame % upSprites.Length] : downSprites[animFrame % downSprites.Length];
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
        if (gameManager.isNight)
        {
            Debug.Log("밤에는 채집할 수 없습니다.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, harvestRange);

        foreach (var hit in hits)
        {
            Crop crop = hit.GetComponent<Crop>();
            if (crop != null && !crop.isHarvested)
            {
                crop.isHarvested = true;

                Destroy(crop.gameObject);

                
                

                ShowHarvestAnimation();

                // 인벤토리, UI 업데이트
                InventoryManager.Instance.AddItem(crop.cropName);
                

                Debug.Log($"작물 수확! {crop.cropName} - {crop.score}점 | 총 점수: {totalScore}");

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

    // 채집 범위 업그레이드 함수
    public void IncreaseHarvestRange(float amount)
    {
        harvestRange += amount;
        Debug.Log("채집 범위 증가! 현재 채집 범위: " + harvestRange);
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("이동 속도 증가 현재 속도 : " + moveSpeed);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("현재 돈: " + money);
    }
}