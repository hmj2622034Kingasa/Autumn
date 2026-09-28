using UnityEngine;

public class CandyShooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField]
    private GameObject bulletPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackInterval = 0.6f;

    [SerializeField]
    private float attackRange = 8.0f;

    private float nextAttackTime = 0.0f;

    void Update()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        EnemyHealth target = FindNearestEnemy();

        if (target == null)
        {
            return;
        }

        Shoot(target);

        nextAttackTime = Time.time + attackInterval;
    }

    EnemyHealth FindNearestEnemy()
    {
        EnemyHealth nearestEnemy = null;

        float nearestDistance = attackRange;

        foreach (EnemyHealth enemy in EnemyHealth.AllEnemies)
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
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot(EnemyHealth target)
    {
        Vector2 direction =
            ((Vector2)target.transform.position -
             (Vector2)transform.position).normalized;

        GameObject bulletObject =
            Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

        CandyBullet bullet =
            bulletObject.GetComponent<CandyBullet>();

        bullet.SetDirection(direction);
    }
}