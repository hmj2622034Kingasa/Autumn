using UnityEngine;

public class PumpkinBombWeapon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject pumpkinBombPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackRange = 8.0f;

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
                PlayerLoadout.WeaponType.PumpkinBomb
            );

        // ñ¢èäéùÇ»ÇÁâΩÇ‡ÇµÇ»Ç¢
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

        FireBombs(target, level);

        nextAttackTime =
            Time.time +
            GetAttackInterval(level) *
            loadout.GetCooldownMultiplier();
    }

    EnemyHealth FindNearestEnemy()
    {
        EnemyHealth nearestEnemy = null;

        float nearestDistance =
            attackRange;

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
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void FireBombs(
        EnemyHealth target,
        int level
    )
    {
        int bombCount =
            level >= 3 ? 2 : 1;

        float damage =
            GetDamage(level) *
            loadout.GetAttackMultiplier();

        float radius =
            GetExplosionRadius(level);

        for (
            int i = 0;
            i < bombCount;
            i++
        )
        {
            Vector2 targetPosition =
                target.transform.position;

            // Lv3ÇÃ2å¬ñ⁄ÇÕè≠ÇµÇ∏ÇÁÇ∑
            if (bombCount >= 2 && i == 1)
            {
                targetPosition +=
                    Random.insideUnitCircle
                    * 0.7f;
            }

            GameObject bombObject =
                Instantiate(
                    pumpkinBombPrefab,
                    transform.position,
                    Quaternion.identity
                );

            PumpkinBomb bomb =
                bombObject.GetComponent<PumpkinBomb>();

            if (bomb != null)
            {
                bomb.Setup(
                    targetPosition,
                    6.0f,
                    damage,
                    radius
                );
            }
        }
    }

    float GetDamage(int level)
    {
        switch (level)
        {
            case 1:
                return 25.0f;

            case 2:
                return 38.0f;

            default:
                return 50.0f;
        }
    }

    float GetExplosionRadius(int level)
    {
        switch (level)
        {
            case 1:
                return 1.3f;

            case 2:
                return 1.7f;

            default:
                return 2.0f;
        }
    }

    float GetAttackInterval(int level)
    {
        switch (level)
        {
            case 1:
                return 2.0f;

            case 2:
                return 1.5f;

            default:
                return 1.0f;
        }
    }
}