using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 1.5f;

    [SerializeField]
    private float attackDistance = 4.0f;

    [Header("Attack")]
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float attackInterval = 1.5f;

    private Rigidbody2D rb;

    private float nextAttackTime;

    void Start()
    {
        rb =
            GetComponent<Rigidbody2D>();
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

        float distance =
            Vector2.Distance(
                transform.position,
                target.transform.position
            );

        Vector2 direction =
            (
                target.transform.position -
                transform.position
            ).normalized;

        if (distance >
            attackDistance)
        {
            rb.linearVelocity =
                direction * moveSpeed;
        }
        else if (
            distance <
            attackDistance * 0.7f
        )
        {
            rb.linearVelocity =
                -direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity =
                Vector2.zero;
        }
    }

    void Update()
    {
        Contestant target =
            FindNearestTarget();

        if (target == null)
        {
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                target.transform.position
            );

        if (distance >
            attackDistance + 1.0f)
        {
            return;
        }

        if (Time.time <
            nextAttackTime)
        {
            return;
        }

        Shoot(target);

        nextAttackTime =
            Time.time +
            attackInterval;
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

            if (health == null ||
                health.IsDown)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    contestant.transform.position
                );

            if (distance <
                nearestDistance)
            {
                nearestDistance =
                    distance;

                nearest = contestant;
            }
        }

        return nearest;
    }

    void Shoot(
        Contestant target
    )
    {
        if (bulletPrefab == null)
        {
            return;
        }

        Vector2 direction =
            (
                target.transform.position -
                transform.position
            ).normalized;

        GameObject bulletObject =
            Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

        EnemyBullet bullet =
            bulletObject
            .GetComponent<EnemyBullet>();

        if (bullet != null)
        {
            bullet.SetDirection(
                direction
            );
        }
    }
}