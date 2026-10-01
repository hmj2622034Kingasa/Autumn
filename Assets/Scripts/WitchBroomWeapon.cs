using UnityEngine;

public class WitchBroomWeapon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject broomPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackInterval = 2.0f;

    [SerializeField]
    private float spawnDistance = 8.0f;

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
                PlayerLoadout.WeaponType.WitchBroom
            );

        // 未所持
        if (level <= 0)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        // 敵がいなければ撃たない
        if (EnemyHealth.AllEnemies.Count == 0)
        {
            return;
        }

        FireBrooms(level);

        nextAttackTime =
            Time.time +
            GetAttackInterval(level) *
            loadout.GetCooldownMultiplier();
    }

    void FireBrooms(int level)
    {
        float damage =
            GetDamage(level) *
            loadout.GetAttackMultiplier();

        float speed =
            GetSpeed(level);

        // 左から右へ
        CreateBroom(
            (Vector2)transform.position +
            Vector2.left * spawnDistance,
            Vector2.right,
            speed,
            damage
        );

        // Lv3なら右から左へもう1本
        if (level >= 3)
        {
            CreateBroom(
                (Vector2)transform.position +
                Vector2.right * spawnDistance,
                Vector2.left,
                speed,
                damage
            );
        }
    }

    void CreateBroom(
        Vector2 position,
        Vector2 direction,
        float speed,
        float damage
    )
    {
        GameObject broomObject =
            Instantiate(
                broomPrefab,
                position,
                Quaternion.identity
            );

        WitchBroomProjectile broom =
            broomObject
            .GetComponent<WitchBroomProjectile>();

        if (broom == null)
        {
            Debug.LogError(
                "WitchBroom Prefab に " +
                "WitchBroomProjectile がありません！"
            );

            Destroy(broomObject);

            return;
        }

        broom.Setup(
            direction,
            speed,
            damage,
            4.0f
        );
    }

    float GetDamage(int level)
    {
        switch (level)
        {
            case 1:
                return 28.0f;

            case 2:
                return 40.0f;

            default:
                return 55.0f;
        }
    }

    float GetSpeed(int level)
    {
        switch (level)
        {
            case 1:
                return 7.0f;

            case 2:
                return 9.0f;

            default:
                return 10.0f;
        }
    }

    float GetAttackInterval(int level)
    {
        switch (level)
        {
            case 1:
                return 2.0f;

            case 2:
                return 1.7f;

            default:
                return 1.5f;
        }
    }
}