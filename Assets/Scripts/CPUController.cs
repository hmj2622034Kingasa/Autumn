using UnityEngine;

public class CPUController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 4.5f;

    [SerializeField]
    private float dangerDistance = 1.2f;

    private Rigidbody2D rb;

    private PlayerHealth health;

    private CPULevelUpAI levelUpAI;

    void Start()
    {
        rb =
            GetComponent<Rigidbody2D>();

        health =
            GetComponent<PlayerHealth>();

        levelUpAI =
            GetComponent<CPULevelUpAI>();
    }

    void FixedUpdate()
    {
        // ダウン中
        if (health != null &&
            health.IsDown)
        {
            rb.linearVelocity =
                Vector2.zero;

            return;
        }

        // レベルアップで悩み中
        if (levelUpAI != null &&
            levelUpAI.IsThinking)
        {
            rb.linearVelocity =
                Vector2.zero;

            return;
        }

        EnemyHealth target =
            FindNearestEnemy();

        if (target == null)
        {
            rb.linearVelocity =
                Vector2.zero;

            return;
        }

        Vector2 difference =
            target.transform.position -
            transform.position;

        float distance =
            difference.magnitude;

        Vector2 direction =
            difference.normalized;

        // 敵に近すぎたら離れる
        if (distance <
            dangerDistance)
        {
            rb.linearVelocity =
                -direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity =
                direction * moveSpeed;
        }
    }

    EnemyHealth FindNearestEnemy()
    {
        EnemyHealth nearest = null;

        float nearestDistance =
            float.MaxValue;

        foreach (
            EnemyHealth enemy
            in EnemyHealth.AllEnemies
        )
        {
            if (enemy == null)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance <
                nearestDistance)
            {
                nearestDistance =
                    distance;

                nearest = enemy;
            }
        }

        return nearest;
    }

    // Witch Boots用
    public void IncreaseMoveSpeed(
        float percent
    )
    {
        moveSpeed *=
            1.0f + percent;
    }
}