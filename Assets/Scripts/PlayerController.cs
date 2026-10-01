using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerHealth playerHealth;
    private LevelUpSystem levelUpSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerHealth =
            GetComponent<PlayerHealth>();

        levelUpSystem =
            GetComponent<LevelUpSystem>();
    }

    void Update()
    {
        // ダウン中
        if (playerHealth != null &&
            playerHealth.IsDown)
        {
            moveInput = Vector2.zero;
            return;
        }

        // レベルアップ選択中
        if (levelUpSystem != null &&
            levelUpSystem.IsChoosing)
        {
            moveInput = Vector2.zero;
            return;
        }

        float moveX =
            Input.GetAxisRaw("Horizontal");

        float moveY =
            Input.GetAxisRaw("Vertical");

        moveInput =
            new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        if (playerHealth != null &&
            playerHealth.IsDown)
        {
            rb.linearVelocity =
                Vector2.zero;
            return;
        }

        if (levelUpSystem != null &&
            levelUpSystem.IsChoosing)
        {
            rb.linearVelocity =
                Vector2.zero;
            return;
        }

        rb.linearVelocity =
            moveInput * moveSpeed;
    }

    // 魔女のブーツ用
    public void IncreaseMoveSpeed(
        float percent
    )
    {
        moveSpeed *=
            1.0f + percent;

        Debug.Log(
            "Move Speed : " + moveSpeed
        );
    }
}