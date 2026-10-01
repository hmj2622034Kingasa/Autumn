using UnityEngine;

public class DeathScytheWeapon : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField]
    private GameObject scytheEffectPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackInterval = 2.5f;

    private PlayerLoadout loadout;

    private float nextAttackTime = 0.0f;

    void Start()
    {
        loadout =
            GetComponent<PlayerLoadout>();
    }

    void Update()
    {
        if (loadout == null)
        {
            return;
        }

        int level =
            loadout.GetWeaponLevel(
                PlayerLoadout.WeaponType.DeathScythe
            );

        // ñ¢èäéù
        if (level <= 0)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        EnemyHealth target =
            FindNearestEnemy();

        if (target == null)
        {
            return;
        }

        Attack(
            target,
            level
        );

        nextAttackTime =
            Time.time +
            attackInterval *
            loadout.GetCooldownMultiplier();
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

    void Attack(
        EnemyHealth target,
        int level
    )
    {
        float radius =
            GetRadius(level);

        float damage =
            GetDamage(level) *
            loadout.GetAttackMultiplier();

        float attackAngle =
            GetAttackAngle(level);

        Vector2 targetDirection =
            (
                target.transform.position -
                transform.position
            ).normalized;

        float centerAngle =
            Mathf.Atan2(
                targetDirection.y,
                targetDirection.x
            ) * Mathf.Rad2Deg;

        // çUåÇîÕàÕì‡ÇÃìGÇ÷É_ÉÅÅ[ÉW
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

            Vector2 enemyDirection =
                (
                    enemy.transform.position -
                    transform.position
                );

            float distance =
                enemyDirection.magnitude;

            if (distance > radius)
            {
                continue;
            }

            // Lv3ÇÕëSé¸àÕ
            if (level >= 3)
            {
                enemy.TakeDamage(damage);
                continue;
            }

            float enemyAngle =
                Mathf.Atan2(
                    enemyDirection.y,
                    enemyDirection.x
                ) * Mathf.Rad2Deg;

            float angleDifference =
                Mathf.Abs(
                    Mathf.DeltaAngle(
                        centerAngle,
                        enemyAngle
                    )
                );

            if (angleDifference <=
                attackAngle / 2.0f)
            {
                enemy.TakeDamage(damage);
            }
        }

        CreateEffect(
            centerAngle,
            attackAngle
        );
    }

    void CreateEffect(
        float centerAngle,
        float attackAngle
    )
    {
        if (scytheEffectPrefab == null)
        {
            return;
        }

        GameObject effectObject =
            Instantiate(
                scytheEffectPrefab,
                transform.position,
                Quaternion.identity
            );

        DeathScytheEffect effect =
            effectObject
            .GetComponent<DeathScytheEffect>();

        if (effect == null)
        {
            return;
        }

        float startAngle =
            centerAngle -
            attackAngle / 2.0f;

        effect.Setup(
            transform,
            startAngle,
            attackAngle,
            0.35f
        );
    }

    float GetDamage(int level)
    {
        switch (level)
        {
            case 1:
                return 35.0f;

            case 2:
                return 50.0f;

            default:
                return 70.0f;
        }
    }

    float GetRadius(int level)
    {
        switch (level)
        {
            case 1:
                return 2.0f;

            case 2:
                return 2.5f;

            default:
                return 3.0f;
        }
    }

    float GetAttackAngle(int level)
    {
        switch (level)
        {
            case 1:
                return 180.0f;

            case 2:
                return 270.0f;

            default:
                return 360.0f;
        }
    }
}