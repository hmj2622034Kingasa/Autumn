using System.Collections.Generic;
using UnityEngine;

public class WitchBroomProjectile : MonoBehaviour
{
    private Vector2 direction;

    private float moveSpeed;
    private float damage;
    private float lifeTime;

    [SerializeField]
    private float hitRadius = 0.6f;

    // 同じ敵に何度もダメージを入れないため
    private HashSet<EnemyHealth> hitEnemies =
        new HashSet<EnemyHealth>();

    public void Setup(
        Vector2 newDirection,
        float speed,
        float newDamage,
        float newLifeTime
    )
    {
        direction =
            newDirection.normalized;

        moveSpeed = speed;
        damage = newDamage;
        lifeTime = newLifeTime;

        // 進行方向に見た目を合わせる
        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0.0f,
                0.0f,
                angle
            );

        Destroy(
            gameObject,
            lifeTime
        );
    }

    void Update()
    {
        // 移動
        transform.position +=
            (Vector3)(
                direction *
                moveSpeed *
                Time.deltaTime
            );

        AttackEnemies();
    }

    void AttackEnemies()
    {
        EnemyHealth[] enemies =
            EnemyHealth.AllEnemies.ToArray();

        foreach (
            EnemyHealth enemy
            in enemies
        )
        {
            if (enemy == null)
            {
                continue;
            }

            // すでに攻撃済み
            if (hitEnemies.Contains(enemy))
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance <= hitRadius)
            {
                enemy.TakeDamage(damage);

                hitEnemies.Add(enemy);
            }
        }
    }
}