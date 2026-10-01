using System.Collections.Generic;
using UnityEngine;

public class GhostLanternWeapon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject ghostLanternPrefab;

    // ƒ‰ƒ“ƒ^ƒ“‚ÌŒÂ”

    [Header("Lantern Count")]
    [SerializeField]
    private int level1LanternCount = 3;

    [SerializeField]
    private int level2LanternCount = 5;

    [SerializeField]
    private int level3LanternCount = 9;

    // ‰ñ“]

    [Header("Movement")]
    [SerializeField]
    private float orbitRadius = 1.5f;

    [SerializeField]
    private float level1RotationSpeed = 120.0f;

    [SerializeField]
    private float level2RotationSpeed = 150.0f;

    [SerializeField]
    private float level3RotationSpeed = 210.0f;

    // UŒ‚

    [Header("Attack")]
    [SerializeField]
    private float hitRadius = 0.45f;

    [SerializeField]
    private float level1Damage = 10.0f;

    [SerializeField]
    private float level2Damage = 13.0f;

    [SerializeField]
    private float level3Damage = 16.0f;

    [SerializeField]
    private float level1DamageInterval = 0.5f;

    [SerializeField]
    private float level2DamageInterval = 0.45f;

    [SerializeField]
    private float level3DamageInterval = 0.4f;

    private PlayerLoadout loadout;

    private List<GameObject> lanterns =
        new List<GameObject>();

    private float currentAngle = 0.0f;

    private float nextDamageTime = 0.0f;

    private int lastLevel = 0;

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
                PlayerLoadout.WeaponType.GhostLantern
            );

        // –¢Š
        if (level <= 0)
        {
            RemoveAllLanterns();

            lastLevel = 0;

            return;
        }

        // ƒŒƒxƒ‹‚ª•Ï‚í‚Á‚½‚ç
        // ƒ‰ƒ“ƒ^ƒ“‚ğì‚è’¼‚·
        if (level != lastLevel)
        {
            CreateLanterns(level);

            lastLevel = level;
        }

        MoveLanterns(level);

        if (Time.time >= nextDamageTime)
        {
            AttackEnemies(level);

            float interval =
                GetDamageInterval(level);

            // Œ–é‚ÌŒv‚ÌŒø‰Ê
            interval *=
                loadout.GetCooldownMultiplier();

            nextDamageTime =
                Time.time + interval;
        }
    }

    // ƒ‰ƒ“ƒ^ƒ“¶¬

    void CreateLanterns(int level)
    {
        RemoveAllLanterns();

        int lanternCount =
            GetLanternCount(level);

        for (
            int i = 0;
            i < lanternCount;
            i++
        )
        {
            GameObject lantern =
                Instantiate(
                    ghostLanternPrefab,
                    transform.position,
                    Quaternion.identity
                );

            lanterns.Add(lantern);
        }
    }

    // ƒ‰ƒ“ƒ^ƒ“ˆÚ“®

    void MoveLanterns(int level)
    {
        if (lanterns.Count == 0)
        {
            return;
        }

        float rotationSpeed =
            GetRotationSpeed(level);

        currentAngle +=
            rotationSpeed *
            Time.deltaTime;

        float angleDistance =
            360.0f /
            lanterns.Count;

        for (
            int i = 0;
            i < lanterns.Count;
            i++
        )
        {
            float angle =
                currentAngle +
                angleDistance * i;

            float radian =
                angle *
                Mathf.Deg2Rad;

            Vector2 offset =
                new Vector2(
                    Mathf.Cos(radian),
                    Mathf.Sin(radian)
                ) *
                orbitRadius;

            lanterns[i]
                .transform.position =
                (Vector2)transform.position +
                offset;
        }
    }

    // UŒ‚

    void AttackEnemies(int level)
    {
        float baseDamage =
            GetDamage(level);

        // •”L‚Ì‚¨ç‚è‚ÌŒø‰Ê
        float finalDamage =
            baseDamage *
            loadout.GetAttackMultiplier();

        foreach (
            GameObject lantern
            in lanterns
        )
        {
            if (lantern == null)
            {
                continue;
            }

            EnemyHealth[] enemies =
                EnemyHealth.AllEnemies
                    .ToArray();

            foreach (
                EnemyHealth enemy
                in enemies
            )
            {
                if (enemy == null)
                {
                    continue;
                }

                float distance =
                    Vector2.Distance(
                        lantern.transform.position,
                        enemy.transform.position
                    );

                if (distance <= hitRadius)
                {
                    enemy.TakeDamage(
                        finalDamage
                    );
                }
            }
        }
    }

    // Lv‚²‚Æ‚ÌŒÂ”

    int GetLanternCount(int level)
    {
        switch (level)
        {
            case 1:
                return level1LanternCount;

            case 2:
                return level2LanternCount;

            default:
                return level3LanternCount;
        }
    }

    // Lv‚²‚Æ‚Ì‰ñ“]‘¬“x

    float GetRotationSpeed(int level)
    {
        switch (level)
        {
            case 1:
                return level1RotationSpeed;

            case 2:
                return level2RotationSpeed;

            default:
                return level3RotationSpeed;
        }
    }

    // Lv‚²‚Æ‚Ìƒ_ƒ[ƒW

    float GetDamage(int level)
    {
        switch (level)
        {
            case 1:
                return level1Damage;

            case 2:
                return level2Damage;

            default:
                return level3Damage;
        }
    }

    // Lv‚²‚Æ‚ÌUŒ‚ŠÔŠu

    float GetDamageInterval(int level)
    {
        switch (level)
        {
            case 1:
                return level1DamageInterval;

            case 2:
                return level2DamageInterval;

            default:
                return level3DamageInterval;
        }
    }

    // ‘Síœ

    void RemoveAllLanterns()
    {
        foreach (
            GameObject lantern
            in lanterns
        )
        {
            if (lantern != null)
            {
                Destroy(lantern);
            }
        }

        lanterns.Clear();
    }

    void OnDestroy()
    {
        RemoveAllLanterns();
    }
}