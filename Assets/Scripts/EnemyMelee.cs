using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 2.0f;

    [Header("Attack")]
    [SerializeField]
    private float attackDamage = 10.0f;

    [SerializeField]
    private float attackInterval = 1.0f;

    private Rigidbody2D rb;

    private float nextAttackTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Contestant target =
            FindNearestTarget();

        if (target == null)
        {
            rb.linearVelocity =
                Vector2.zero;

            return;
        }

        Vector2 direction =
            (
                target.transform.position -
                transform.position
            ).normalized;

        rb.linearVelocity =
            direction * moveSpeed;
    }

    Contestant FindNearestTarget()
    {
        Contestant nearest = null;

        float nearestDistance =
            float.MaxValue;

        foreach (
            Contestant contestant
            in Contestant.All
        )
        {
            if (contestant == null)
            {
                continue;
            }

            PlayerHealth health =
                contestant.GetComponent<PlayerHealth>();

            if (health == null)
            {
                continue;
            }

            if (health.IsDown)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    contestant.transform.position
                );

            if (distance < nearestDistance)
            {
                nearestDistance =
                    distance;

                nearest = contestant;
            }
        }

        return nearest;
    }

    void OnTriggerStay2D(
        Collider2D other
    )
    {
        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health == null)
        {
            return;
        }

        if (Time.time <
            nextAttackTime)
        {
            return;
        }

        health.TakeDamage(
            attackDamage
        );

        nextAttackTime =
            Time.time +
            attackInterval;
    }
}