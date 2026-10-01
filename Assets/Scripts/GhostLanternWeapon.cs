using System.Collections.Generic;
using UnityEngine;

public class GhostLanternWeapon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject ghostLanternPrefab;

    [Header("Movement")]
    [SerializeField]
    private float orbitRadius = 1.5f;

    [SerializeField]
    private float normalRotationSpeed = 120.0f;

    [SerializeField]
    private float level3RotationSpeed = 180.0f;

    [Header("Attack")]
    [SerializeField]
    private float hitRadius = 0.45f;

    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float damageInterval = 0.5f;

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

        // 未所持
        if (level <= 0)
        {
            RemoveAllLanterns();

            lastLevel = 0;

            return;
        }

        // レベルが変わったら
        // ランタンの数を作り直す
        if (level != lastLevel)
        {
            CreateLanterns(level);

            lastLevel = level;
        }

        MoveLanterns(level);

        if (Time.time >= nextDamageTime)
        {
            AttackEnemies();

            nextDamageTime =
                Time.time +
                damageInterval *
                loadout.GetCooldownMultiplier();
        }
    }

    void CreateLanterns(int level)
    {
        RemoveAllLanterns();

        int lanternCount = level;

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

    void MoveLanterns(int level)
    {
        if (lanterns.Count == 0)
        {
            return;
        }

        float rotationSpeed =
            level >= 3
            ? level3RotationSpeed
            : normalRotationSpeed;

        currentAngle +=
            rotationSpeed * Time.deltaTime;

        float angleDistance =
            360.0f / lanterns.Count;

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
                angle * Mathf.Deg2Rad;

            Vector2 offset =
                new Vector2(
                    Mathf.Cos(radian),
                    Mathf.Sin(radian)
                ) * orbitRadius;

            lanterns[i].transform.position =
                (Vector2)transform.position +
                offset;
        }
    }

    void AttackEnemies()
    {
        float finalDamage =
            damage *
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

            // ToArrayを使って、
            // 攻撃中に敵が消えても安全にする
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

                float distance =
                    Vector2.Distance(
                        lantern.transform.position,
                        enemy.transform.position
                    );

                if (distance <= hitRadius)
                {
                    enemy.TakeDamage(finalDamage);
                }
            }
        }
    }

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