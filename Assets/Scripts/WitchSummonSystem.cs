using System.Collections.Generic;
using UnityEngine;

public class WitchSummonSystem : MonoBehaviour
{
    [Header("Summon")]
    [SerializeField]
    private int summonCost = 45;

    [SerializeField]
    private float summonCooldown = 2.5f;

    private PlayerCandy playerCandy;
    private PlayerHealth playerHealth;
    private LevelUpSystem levelUpSystem;
    private CPULevelUpAI cpuLevelUpAI;
    private Contestant contestant;

    private float nextSummonTime = 0.0f;

    public int SummonCost
    {
        get { return summonCost; }
    }

    void Start()
    {
        playerCandy =
            GetComponent<PlayerCandy>();

        playerHealth =
            GetComponent<PlayerHealth>();

        levelUpSystem =
            GetComponent<LevelUpSystem>();

        cpuLevelUpAI =
            GetComponent<CPULevelUpAI>();

        contestant =
            GetComponent<Contestant>();
    }

    void Update()
    {
        // Qキーは人間プレイヤーだけ
        if (contestant != null &&
            contestant.IsPlayer &&
            Input.GetKeyDown(KeyCode.Q))
        {
            TrySummon();
        }
    }

    public bool TrySummon()
    {
        if (playerCandy == null)
        {
            return false;
        }

        // ダウン中
        if (playerHealth != null &&
            playerHealth.IsDown)
        {
            return false;
        }

        // プレイヤーのレベルアップ中
        if (levelUpSystem != null &&
            levelUpSystem.IsChoosing)
        {
            return false;
        }

        // CPUのレベルアップ思考中
        if (cpuLevelUpAI != null &&
            cpuLevelUpAI.IsThinking)
        {
            return false;
        }

        // クールタイム
        if (Time.time < nextSummonTime)
        {
            return false;
        }

        // キャンディ不足
        if (!playerCandy.HasCandy(summonCost))
        {
            return false;
        }

        playerCandy.SpendCandy(
            summonCost
        );

        nextSummonTime =
            Time.time +
            summonCooldown;

        ActivateRandomEffect();

        return true;
    }

    void ActivateRandomEffect()
    {
        float swapChance =
            GetCandySwapChance();

        float otherChance =
            (1.0f - swapChance) / 2.0f;

        float random =
            Random.value;

        // キャンディ交換
        if (random < swapChance)
        {
            if (!CandySwap())
            {
                // 交換相手がいない場合
                if (Random.value < 0.5f)
                {
                    ClearEnemies();
                }
                else
                {
                    FullHeal();
                }
            }

            return;
        }

        // 敵一掃
        if (random <
            swapChance + otherChance)
        {
            ClearEnemies();
            return;
        }

        // HP全回復
        FullHeal();
    }

    // =========================
    // 敵一掃
    // =========================

    void ClearEnemies()
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

            EnemyHealth.AllEnemies.Remove(
                enemy
            );

            Destroy(
                enemy.gameObject
            );
        }

        Debug.Log(
            gameObject.name +
            "：魔女召喚！ 敵を一掃！"
        );
    }

    // =========================
    // HP全回復
    // =========================

    void FullHeal()
    {
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.Heal(
            playerHealth.MaxHP
        );

        Debug.Log(
            gameObject.name +
            "：魔女召喚！ HP全回復！"
        );
    }

    // =========================
    // キャンディ交換
    // =========================

    bool CandySwap()
    {
        PlayerCandy[] allCandyUsers =
            FindObjectsByType<PlayerCandy>(
                FindObjectsSortMode.None
            );

        List<PlayerCandy> candidates =
            new List<PlayerCandy>();

        foreach (
            PlayerCandy candyUser
            in allCandyUsers
        )
        {
            if (candyUser == null ||
                candyUser == playerCandy)
            {
                continue;
            }

            PlayerHealth targetHealth =
                candyUser.GetComponent<PlayerHealth>();

            if (targetHealth != null &&
                targetHealth.IsDown)
            {
                continue;
            }

            candidates.Add(
                candyUser
            );
        }

        if (candidates.Count == 0)
        {
            return false;
        }

        PlayerCandy target =
            candidates[
                Random.Range(
                    0,
                    candidates.Count
                )
            ];

        int myCandy =
            playerCandy.CandyCount;

        int targetCandy =
            target.CandyCount;

        playerCandy.SetCandy(
            targetCandy
        );

        target.SetCandy(
            myCandy
        );

        Debug.Log(
            gameObject.name +
            "：魔女召喚！ キャンディ交換！"
        );

        return true;
    }

    // =========================
    // 交換魔法の確率
    // =========================

    float GetCandySwapChance()
    {
        if (GameTimer.Instance == null)
        {
            return 0.34f;
        }

        float time =
            GameTimer.Instance.ElapsedTime;

        if (time < 120.0f)
        {
            return 0.34f;
        }

        if (time < 180.0f)
        {
            return 0.25f;
        }

        if (time < 240.0f)
        {
            return 0.15f;
        }

        if (time < 300.0f)
        {
            return 0.05f;
        }

        // ボス戦では交換なし
        return 0.0f;
    }
}