using UnityEngine;

public class BatFamiliarWeapon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject batPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackInterval = 1.2f;

    [SerializeField]
    private float attackRange = 10.0f;

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
                PlayerLoadout.WeaponType.BatFamiliar
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

        SpawnBats(
            target,
            level
        );

        nextAttackTime =
            Time.time +
            attackInterval;
    }

    EnemyHealth FindNearestEnemy()
    {
        EnemyHealth nearest = null;

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

    void SpawnBats(
        EnemyHealth target,
        int level
    )
    {
        int batCount =
            GetBatCount(level);

        float damage =
            GetDamage(level) *
            loadout.GetAttackMultiplier();

        for (
            int i = 0;
            i < batCount;
            i++
        )
        {
            Vector2 spawnOffset =
                Random.insideUnitCircle
                * 0.4f;

            GameObject batObject =
                Instantiate(
                    batPrefab,
                    (Vector2)transform.position +
                    spawnOffset,
                    Quaternion.identity
                );

            BatProjectile bat =
                batObject
                .GetComponent<BatProjectile>();

            if (bat == null)
            {
                Debug.LogError(
                    "Bat Prefab Ç… BatProjectile Ç™Ç†ÇËÇ‹ÇπÇÒÅI"
                );

                Destroy(batObject);

                continue;
            }

            bat.Setup(
                target,
                damage
            );
        }
    }

    int GetBatCount(int level)
    {
        switch (level)
        {
            case 1:
                return 2;

            case 2:
                return 4;

            default:
                return 7;
        }
    }

    float GetDamage(int level)
    {
        switch (level)
        {
            case 1:
                return 14.0f;

            case 2:
                return 16.0f;

            default:
                return 18.0f;
        }
    }
}