using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 7.0f;

    [Header("Attack")]
    [SerializeField]
    private float damage = 14.0f;

    [Header("Life")]
    [SerializeField]
    private float lifeTime = 5.0f;

    private EnemyHealth target;

    void Start()
    {
        Destroy(
            gameObject,
            lifeTime
        );
    }

    public void Setup(
        EnemyHealth newTarget,
        float newDamage
    )
    {
        target = newTarget;
        damage = newDamage;
    }

    void Update()
    {
        // É^Å[ÉQÉbÉgÇ™ì|Ç≥ÇÍÇΩèÍçá
        // êVÇµÇ¢ìGÇíTÇ∑
        if (target == null)
        {
            target =
                FindNearestEnemy();

            if (target == null)
            {
                return;
            }
        }

        Vector2 direction =
            (
                target.transform.position -
                transform.position
            ).normalized;

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                target.transform.position,
                moveSpeed * Time.deltaTime
            );
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

            if (distance < nearestDistance)
            {
                nearestDistance =
                    distance;

                nearest = enemy;
            }
        }

        return nearest;
    }

    void OnTriggerEnter2D(
        Collider2D other
    )
    {
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();

        if (enemy == null)
        {
            return;
        }

        enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}