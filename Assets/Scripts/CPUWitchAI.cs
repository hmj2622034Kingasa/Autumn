using UnityEngine;

public class CPUWitchAI : MonoBehaviour
{
    [Header("AI")]
    [SerializeField]
    private float decisionInterval = 0.75f;

    [SerializeField]
    private float dangerRadius = 2.5f;

    private PlayerHealth health;
    private PlayerCandy candy;
    private WitchSummonSystem summonSystem;
    private CPULevelUpAI levelUpAI;

    private float nextDecisionTime;

    void Start()
    {
        health =
            GetComponent<PlayerHealth>();

        candy =
            GetComponent<PlayerCandy>();

        summonSystem =
            GetComponent<WitchSummonSystem>();

        levelUpAI =
            GetComponent<CPULevelUpAI>();
    }

    void Update()
    {
        if (Time.time <
            nextDecisionTime)
        {
            return;
        }

        nextDecisionTime =
            Time.time +
            decisionInterval;

        Think();
    }

    void Think()
    {
        if (health == null ||
            candy == null ||
            summonSystem == null)
        {
            return;
        }

        if (health.IsDown)
        {
            return;
        }

        // レベルアップで考え中
        if (levelUpAI != null &&
            levelUpAI.IsThinking)
        {
            return;
        }

        // 45キャンディ未満なら召喚不可
        if (!candy.HasCandy(
            summonSystem.SummonCost
        ))
        {
            return;
        }

        float hpRate =
            health.CurrentHP /
            health.MaxHP;

        int nearbyEnemies =
            CountNearbyEnemies(
                dangerRadius
            );

        // -------------------------
        // 1. HPがかなり危険
        // -------------------------

        if (hpRate <= 0.35f)
        {
            TryEmergencySummon(
                "HPが危険"
            );

            return;
        }

        // -------------------------
        // 2. 大量の敵に囲まれた
        // -------------------------

        if (nearbyEnemies >= 8)
        {
            TryEmergencySummon(
                "敵に囲まれた"
            );

            return;
        }

        // -------------------------
        // 3. HPも少し減っていて
        //    敵も多い
        // -------------------------

        if (hpRate <= 0.55f &&
            nearbyEnemies >= 5)
        {
            TryEmergencySummon(
                "戦況が危険"
            );

            return;
        }

        // -------------------------
        // 4. 残り1分
        //    キャンディで負けている
        // -------------------------

        if (GameTimer.Instance != null &&
            GameTimer.Instance.ElapsedTime
            >= 240.0f)
        {
            if (IsLosingCandyRace())
            {
                // 毎回必ず使うと連発するので
                // 少しランダム性を入れる
                if (Random.value <
                    0.20f)
                {
                    TryEmergencySummon(
                        "終盤で負けている"
                    );
                }
            }
        }
    }

    void TryEmergencySummon(
        string reason
    )
    {
        bool success =
            summonSystem.TrySummon();

        if (success)
        {
            Debug.Log(
                gameObject.name +
                " が魔女を召喚！ 理由：" +
                reason
            );
        }
    }

    int CountNearbyEnemies(
        float radius
    )
    {
        int count = 0;

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

            if (distance <= radius)
            {
                count++;
            }
        }

        return count;
    }

    bool IsLosingCandyRace()
    {
        int myCandy =
            candy.CandyCount;

        int highestOtherCandy = 0;

        foreach (
            Contestant contestant
            in Contestant.All
        )
        {
            if (contestant == null ||
                contestant.gameObject ==
                gameObject)
            {
                continue;
            }

            PlayerCandy otherCandy =
                contestant
                .GetComponent<PlayerCandy>();

            if (otherCandy == null)
            {
                continue;
            }

            if (otherCandy.CandyCount >
                highestOtherCandy)
            {
                highestOtherCandy =
                    otherCandy.CandyCount;
            }
        }

        // 10個以上負けていたら
        // 「負けている」と判断
        return highestOtherCandy >=
            myCandy + 10;
    }
}