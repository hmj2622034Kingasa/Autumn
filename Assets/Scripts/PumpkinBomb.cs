using UnityEngine;

public class PumpkinBomb : MonoBehaviour
{
    private Vector2 targetPosition;

    private float moveSpeed;
    private float damage;
    private float explosionRadius;

    private bool exploded = false;

    public void Setup(
        Vector2 target,
        float speed,
        float bombDamage,
        float radius
    )
    {
        targetPosition = target;
        moveSpeed = speed;
        damage = bombDamage;
        explosionRadius = radius;

        // ñúÇ™àÍÇ«Ç±Ç©Ç≈é~Ç‹Ç¡ÇƒÇ‡5ïbå„Ç…è¡Ç¶ÇÈ
        Destroy(gameObject, 5.0f);
    }

    void Update()
    {
        if (exploded)
        {
            return;
        }

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

        float distance =
            Vector2.Distance(
                transform.position,
                targetPosition
            );

        if (distance <= 0.1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (exploded)
        {
            return;
        }

        exploded = true;

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

            if (distance <= explosionRadius)
            {
                enemy.TakeDamage(damage);
            }
        }

        Debug.Log("Pumpkin Bomb Exploded!");

        Destroy(gameObject);
    }
}