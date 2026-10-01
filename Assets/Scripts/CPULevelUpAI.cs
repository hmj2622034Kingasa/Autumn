using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPULevelUpAI : MonoBehaviour
{
    private PlayerCandy candy;

    private PlayerHealth health;

    private PlayerLoadout loadout;

    private CPUController controller;

    private CandyShooter candyShooter;

    private int levelUpCount = 0;

    private bool isThinking = false;

    public bool IsThinking
    {
        get { return isThinking; }
    }

    private class UpgradeOption
    {
        public bool isWeapon;

        public PlayerLoadout.WeaponType weapon;

        public PlayerLoadout.EquipmentType equipment;
    }

    void Start()
    {
        candy =
            GetComponent<PlayerCandy>();

        health =
            GetComponent<PlayerHealth>();

        loadout =
            GetComponent<PlayerLoadout>();

        controller =
            GetComponent<CPUController>();

        candyShooter =
            GetComponent<CandyShooter>();
    }

    void Update()
    {
        if (isThinking)
        {
            return;
        }

        if (health != null &&
            health.IsDown)
        {
            return;
        }

        TryLevelUp();
    }

    void TryLevelUp()
    {
        if (candy == null ||
            loadout == null)
        {
            return;
        }

        if (!HasAnyUpgrade())
        {
            return;
        }

        int cost =
            GetLevelUpCost();

        if (!candy.HasCandy(cost))
        {
            return;
        }

        float hpRate =
            health.CurrentHP /
            health.MaxHP;

        // HP70%以上
        // → レベルアップを優先
        if (hpRate >= 0.70f)
        {
            BeginLevelUp(cost);

            return;
        }

        // HP40～70%
        // → 魔女召喚1回分の45を残す
        if (hpRate >= 0.40f)
        {
            if (candy.CandyCount >=
                cost + 45)
            {
                BeginLevelUp(cost);
            }

            return;
        }

        // HP40%未満
        // → 今はキャンディを温存
        // 次に作る魔女召喚AIを優先させる
    }

    void BeginLevelUp(int cost)
    {
        if (!candy.SpendCandy(cost))
        {
            return;
        }

        StartCoroutine(
            ThinkingRoutine()
        );
    }

    IEnumerator ThinkingRoutine()
    {
        isThinking = true;

        // 悩んでいる間は無敵
        health.SetLevelUpInvincible(
            true
        );

        List<UpgradeOption> options =
            CreateOptions();

        if (options.Count == 0)
        {
            isThinking = false;

            health.SetLevelUpInvincible(
                false
            );

            yield break;
        }

        float thinkingTime =
            GetThinkingTime(
                options
            );

        Debug.Log(
            gameObject.name +
            " がレベルアップで " +
            thinkingTime.ToString("0.0") +
            "秒考えています"
        );

        yield return
            new WaitForSeconds(
                thinkingTime
            );

        UpgradeOption selected =
            ChooseBestOption(
                options
            );

        ApplyOption(
            selected
        );

        levelUpCount++;

        isThinking = false;

        health.SetLevelUpInvincible(
            false
        );

        Debug.Log(
            gameObject.name +
            " がレベルアップ！"
        );
    }

    // ============================
    // 3択作成
    // ============================

    List<UpgradeOption> CreateOptions()
    {
        List<UpgradeOption> weapons =
            CreateWeaponOptions();

        List<UpgradeOption> equipments =
            CreateEquipmentOptions();

        Shuffle(weapons);
        Shuffle(equipments);

        List<UpgradeOption> result =
            new List<UpgradeOption>();

        // 両方候補がある
        if (weapons.Count > 0 &&
            equipments.Count > 0)
        {
            // 必ず1つずつ
            result.Add(
                weapons[0]
            );

            result.Add(
                equipments[0]
            );

            List<UpgradeOption> remaining =
                new List<UpgradeOption>();

            for (
                int i = 1;
                i < weapons.Count;
                i++
            )
            {
                remaining.Add(
                    weapons[i]
                );
            }

            for (
                int i = 1;
                i < equipments.Count;
                i++
            )
            {
                remaining.Add(
                    equipments[i]
                );
            }

            Shuffle(remaining);

            if (remaining.Count > 0)
            {
                result.Add(
                    remaining[0]
                );
            }
        }
        else if (weapons.Count > 0)
        {
            AddOptions(
                result,
                weapons
            );
        }
        else if (equipments.Count > 0)
        {
            AddOptions(
                result,
                equipments
            );
        }

        return result;
    }

    List<UpgradeOption>
        CreateWeaponOptions()
    {
        List<UpgradeOption> result =
            new List<UpgradeOption>();

        if (loadout.HasWeaponSlot())
        {
            List<PlayerLoadout.WeaponType>
                weapons =
                loadout.GetUnownedWeapons();

            foreach (
                PlayerLoadout.WeaponType weapon
                in weapons
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = true;
                option.weapon = weapon;

                result.Add(option);
            }
        }
        else
        {
            List<PlayerLoadout.WeaponType>
                weapons =
                loadout.GetOwnedWeaponsBelowMax();

            foreach (
                PlayerLoadout.WeaponType weapon
                in weapons
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = true;
                option.weapon = weapon;

                result.Add(option);
            }
        }

        return result;
    }

    List<UpgradeOption>
        CreateEquipmentOptions()
    {
        List<UpgradeOption> result =
            new List<UpgradeOption>();

        if (loadout.HasEquipmentSlot())
        {
            List<PlayerLoadout.EquipmentType>
                equipments =
                loadout.GetUnownedEquipments();

            foreach (
                PlayerLoadout.EquipmentType equipment
                in equipments
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = false;
                option.equipment =
                    equipment;

                result.Add(option);
            }
        }
        else
        {
            List<PlayerLoadout.EquipmentType>
                equipments =
                loadout.GetOwnedEquipmentsBelowMax();

            foreach (
                PlayerLoadout.EquipmentType equipment
                in equipments
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = false;
                option.equipment =
                    equipment;

                result.Add(option);
            }
        }

        return result;
    }

    // ============================
    // 悩む時間
    // ============================

    float GetThinkingTime(
        List<UpgradeOption> options
    )
    {
        int nearbyEnemies =
            CountNearbyEnemies(
                2.5f
            );

        float hpRate =
            health.CurrentHP /
            health.MaxHP;

        // ピンチなら即決
        if (hpRate < 0.5f ||
            nearbyEnemies >= 6)
        {
            return Random.Range(
                0.2f,
                0.6f
            );
        }

        // 普通なら少し考える
        if (Random.value < 0.5f)
        {
            return Random.Range(
                0.5f,
                1.2f
            );
        }

        // たまにかなり悩む
        return Random.Range(
            1.5f,
            3.0f
        );
    }

    // ============================
    // 候補評価
    // ============================

    UpgradeOption ChooseBestOption(
        List<UpgradeOption> options
    )
    {
        UpgradeOption best =
            options[0];

        float bestScore =
            float.MinValue;

        foreach (
            UpgradeOption option
            in options
        )
        {
            float score =
                GetOptionScore(
                    option
                );

            // 毎回完全に同じ判断にはしない
            score +=
                Random.Range(
                    -0.5f,
                    0.5f
                );

            if (score > bestScore)
            {
                bestScore = score;
                best = option;
            }
        }

        return best;
    }

    float GetOptionScore(
        UpgradeOption option
    )
    {
        int nearbyEnemies =
            CountNearbyEnemies(
                3.0f
            );

        float hpRate =
            health.CurrentHP /
            health.MaxHP;

        if (option.isWeapon)
        {
            switch (option.weapon)
            {
                case PlayerLoadout
                    .WeaponType.PumpkinBomb:

                    return
                        nearbyEnemies >= 5
                        ? 9.0f
                        : 6.0f;

                case PlayerLoadout
                    .WeaponType.GhostLantern:

                    return
                        nearbyEnemies >= 5
                        ? 9.0f
                        : 5.0f;

                case PlayerLoadout
                    .WeaponType.DeathScythe:

                    return
                        nearbyEnemies >= 6
                        ? 9.0f
                        : 6.0f;

                case PlayerLoadout
                    .WeaponType.BatFamiliar:

                    return 6.5f;

                case PlayerLoadout
                    .WeaponType.WitchBroom:

                    return 7.0f;

                default:
                    return 6.0f;
            }
        }

        switch (option.equipment)
        {
            case PlayerLoadout
                .EquipmentType.FrankenBolt:

                return
                    hpRate < 0.65f
                    ? 10.0f
                    : 5.0f;

            case PlayerLoadout
                .EquipmentType.VampireCape:

                return
                    hpRate < 0.75f
                    ? 8.0f
                    : 5.0f;

            case PlayerLoadout
                .EquipmentType.BlackCatCharm:

                return 8.0f;

            case PlayerLoadout
                .EquipmentType.MoonClock:

                return 8.0f;

            case PlayerLoadout
                .EquipmentType.CandyBag:

                return 7.0f;

            case PlayerLoadout
                .EquipmentType.WitchBoots:

                return 6.0f;

            default:
                return 5.0f;
        }
    }

    // ============================
    // 選択を適用
    // ============================

    void ApplyOption(
        UpgradeOption option
    )
    {
        if (option.isWeapon)
        {
            bool success =
                loadout.AddOrLevelUpWeapon(
                    option.weapon
                );

            if (!success)
            {
                return;
            }

            if (option.weapon ==
                PlayerLoadout
                .WeaponType.CandyShooter)
            {
                int level =
                    loadout.GetWeaponLevel(
                        option.weapon
                    );

                if (level >= 2 &&
                    candyShooter != null)
                {
                    candyShooter
                        .LevelUpWeapon();
                }
            }

            return;
        }

        bool equipmentSuccess =
            loadout.AddOrLevelUpEquipment(
                option.equipment
            );

        if (!equipmentSuccess)
        {
            return;
        }

        switch (option.equipment)
        {
            case PlayerLoadout
                .EquipmentType.WitchBoots:

                controller
                    .IncreaseMoveSpeed(
                        0.10f
                    );

                break;

            case PlayerLoadout
                .EquipmentType.FrankenBolt:

                health
                    .IncreaseMaxHP(
                        20.0f
                    );

                break;
        }
    }

    // ============================
    // その他
    // ============================

    bool HasAnyUpgrade()
    {
        if (loadout.HasWeaponSlot())
        {
            if (loadout
                .GetUnownedWeapons()
                .Count > 0)
            {
                return true;
            }
        }
        else if (
            loadout
            .GetOwnedWeaponsBelowMax()
            .Count > 0)
        {
            return true;
        }

        if (loadout.HasEquipmentSlot())
        {
            if (loadout
                .GetUnownedEquipments()
                .Count > 0)
            {
                return true;
            }
        }
        else if (
            loadout
            .GetOwnedEquipmentsBelowMax()
            .Count > 0)
        {
            return true;
        }

        return false;
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

    int GetLevelUpCost()
    {
        switch (levelUpCount)
        {
            case 0:
                return 8;

            case 1:
                return 12;

            case 2:
                return 18;

            case 3:
                return 27;

            case 4:
                return 40;

            case 5:
                return 58;

            case 6:
                return 82;

            case 7:
                return 115;

            case 8:
                return 155;

            case 9:
                return 205;

            default:
                return 270 +
                    (levelUpCount - 10)
                    * 80;
        }
    }

    void Shuffle<T>(
        List<T> list
    )
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--
        )
        {
            int randomIndex =
                Random.Range(
                    0,
                    i + 1
                );

            T temp =
                list[i];

            list[i] =
                list[randomIndex];

            list[randomIndex] =
                temp;
        }
    }

    void AddOptions(
        List<UpgradeOption> result,
        List<UpgradeOption> source
    )
    {
        Shuffle(source);

        for (
            int i = 0;
            i < 3;
            i++
        )
        {
            result.Add(
                source[
                    i % source.Count
                ]
            );
        }
    }
}