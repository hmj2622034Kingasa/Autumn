using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public enum EnemyType
    {
        Melee,
        Ranged,
        Tank
    }

    public static List<EnemyHealth> AllEnemies =
        new List<EnemyHealth>();

    [Header("Enemy")]
    [SerializeField]
    private EnemyType enemyType =
        EnemyType.Melee;

    [SerializeField]
    private float maxHP = 30.0f;

    [Header("Mid Boss Candy")]
    [SerializeField]
    private float midBossDamageCandyPercent =
        0.10f;

    [SerializeField]
    private int midBossDamageCandyValue = 4;

    [SerializeField]
    private float midBossDamageCandySpread =
        2.5f;

    [SerializeField]
    private int midBossDeathCandyValue = 30;

    [SerializeField]
    private float midBossDeathCandySpread =
        4.0f;

    [Header("Mid Boss Reward")]
    [SerializeField]
    private GameObject midBossRewardPrefab;

    private float currentHP;

    private float accumulatedMidBossDamage =
        0.0f;

    private bool isDead = false;

    // 最後に攻撃したキャラクター
    private Contestant lastAttacker;

    public float CurrentHP
    {
        get { return currentHP; }
    }

    public float MaxHP
    {
        get { return maxHP; }
    }

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.SubsystemRegistration
    )]
    static void ResetStaticList()
    {
        AllEnemies.Clear();
    }

    void Awake()
    {
        currentHP = maxHP;
    }

    void OnEnable()
    {
        if (!AllEnemies.Contains(this))
        {
            AllEnemies.Add(this);
        }
    }

    void OnDisable()
    {
        AllEnemies.Remove(this);
    }

    // 旧式
    // 攻撃者不明でも使える
    public void TakeDamage(float damage)
    {

        lastAttacker = null;

        TakeDamage(
            damage,
            null
        );
    }

    // 新式
    // 誰が攻撃したか分かる
    public void TakeDamage(
        float damage,
        Contestant attacker
    )
    {
        if (isDead)
        {
            return;
        }

        if (damage <= 0.0f)
        {
            return;
        }

        // 実際に減らせるHP分だけ
        float actualDamage =
            Mathf.Min(
                damage,
                currentHP
            );

        currentHP -= actualDamage;

        // 攻撃者を記録
        // null攻撃では以前の攻撃者を
        // 消さないようにする
        if (attacker != null)
        {
            lastAttacker =
                attacker;
        }

        // 中ボスなら被ダメキャンディ
        if (GetComponent<MidBossTag>()
            != null)
        {
            HandleMidBossDamageCandy(
                actualDamage
            );
        }

        if (currentHP <= 0.0f)
        {
            currentHP = 0.0f;

            Die();
        }
    }

    void HandleMidBossDamageCandy(
        float damage
    )
    {
        accumulatedMidBossDamage +=
            damage;

        float threshold =
            maxHP *
            midBossDamageCandyPercent;

        threshold =
            Mathf.Max(
                threshold,
                1.0f
            );

        while (
            accumulatedMidBossDamage >=
            threshold
        )
        {
            accumulatedMidBossDamage -=
                threshold;

            if (CandySpawner.Instance != null)
            {
                CandySpawner.Instance
                    .SpawnCandy(
                        midBossDamageCandyValue,
                        transform.position,
                        midBossDamageCandySpread
                    );
            }
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        bool isMidBoss =
            GetComponent<MidBossTag>()
            != null;

        // ==========================
        // キャンディドロップ
        // ==========================

        if (CandySpawner.Instance != null)
        {
            if (isMidBoss)
            {
                CandySpawner.Instance
                    .SpawnCandy(
                        midBossDeathCandyValue,
                        transform.position,
                        midBossDeathCandySpread
                    );
            }
            else
            {
                CandySpawner.Instance
                    .SpawnCandy(
                        GetCandyValue(),
                        transform.position
                    );
            }
        }

        // ==========================
        // 撃破者の装備効果
        // ==========================

        if (lastAttacker != null)
        {
            PlayerEquipmentEffects effects =
                lastAttacker.GetComponent<
                    PlayerEquipmentEffects
                >();

            if (effects != null)
            {
                effects.OnEnemyDefeated();
            }
        }

        // ==========================
        // 中ボス専用報酬
        // ==========================

        if (isMidBoss &&
            lastAttacker != null &&
            midBossRewardPrefab != null)
        {
            GameObject rewardObject =
                Instantiate(
                    midBossRewardPrefab,
                    transform.position,
                    Quaternion.identity
                );

            MidBossRewardPickup reward =
                rewardObject.GetComponent<
                    MidBossRewardPickup
                >();

            if (reward != null)
            {
                reward.Setup(
                    lastAttacker
                );
            }
        }

        AllEnemies.Remove(this);

        Destroy(gameObject);
    }

    int GetCandyValue()
    {
        float time = 0.0f;

        if (GameTimer.Instance != null)
        {
            time =
                GameTimer.Instance.ElapsedTime;
        }

        int minute =
            Mathf.Clamp(
                Mathf.FloorToInt(
                    time / 60.0f
                ),
                0,
                4
            );

        switch (enemyType)
        {
            case EnemyType.Melee:

                switch (minute)
                {
                    case 0: return 1;
                    case 1: return 2;
                    case 2: return 3;
                    case 3: return 5;
                    default: return 7;
                }

            case EnemyType.Ranged:

                switch (minute)
                {
                    case 0: return 2;
                    case 1: return 3;
                    case 2: return 5;
                    case 3: return 7;
                    default: return 10;
                }

            case EnemyType.Tank:

                switch (minute)
                {
                    case 0: return 3;
                    case 1: return 5;
                    case 2: return 8;
                    case 3: return 12;
                    default: return 17;
                }
        }

        return 1;
    }
}