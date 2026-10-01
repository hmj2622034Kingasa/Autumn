using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private LevelUpUI levelUpUI;

    private PlayerCandy playerCandy;
    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private PlayerLoadout loadout;
    private CandyShooter candyShooter;

    private int levelUpCount = 0;

    private bool isChoosing = false;

    private List<UpgradeOption> currentOptions =
        new List<UpgradeOption>();

    public bool IsChoosing
    {
        get { return isChoosing; }
    }

    // 3択1個分のデータ
    private class UpgradeOption
    {
        public bool isWeapon;

        public PlayerLoadout.WeaponType weapon;

        public PlayerLoadout.EquipmentType equipment;
    }

    void Start()
    {
        playerCandy =
            GetComponent<PlayerCandy>();

        playerHealth =
            GetComponent<PlayerHealth>();

        playerController =
            GetComponent<PlayerController>();

        loadout =
            GetComponent<PlayerLoadout>();

        candyShooter =
            GetComponent<CandyShooter>();

        // 現在は最初からCandyShooterを持っているので
        // Loadout側にもLv1として登録
        if (!loadout.HasWeapon(
            PlayerLoadout.WeaponType.CandyShooter))
        {
            loadout.AddOrLevelUpWeapon(
                PlayerLoadout.WeaponType.CandyShooter
            );
        }
    }

    void Update()
    {
        if (isChoosing)
        {
            return;
        }

        // Eキーでレベルアップ
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryLevelUp();
        }
    }

    void TryLevelUp()
    {
        if (!HasAnyUpgrade())
        {
            Debug.Log("全ての武器・装備が最大です");
            return;
        }

        int cost =
            GetLevelUpCost();

        if (!playerCandy.HasCandy(cost))
        {
            Debug.Log(
                "Candy不足 必要：" + cost
            );

            return;
        }

        playerCandy.SpendCandy(cost);

        currentOptions =
            CreateRandomOptions();

        if (currentOptions.Count == 0)
        {
            Debug.Log(
                "選択可能な強化がありません"
            );

            return;
        }

        isChoosing = true;

        // 選択中は無敵
        playerHealth
            .SetLevelUpInvincible(true);

        OpenOptionUI();
    }

    // =========================================
    // 3択を作成
    // =========================================

    List<UpgradeOption> CreateRandomOptions()
    {
        List<UpgradeOption> weaponOptions =
            CreateWeaponOptions();

        List<UpgradeOption> equipmentOptions =
            CreateEquipmentOptions();

        Shuffle(weaponOptions);
        Shuffle(equipmentOptions);

        List<UpgradeOption> result =
            new List<UpgradeOption>();

        bool hasWeapons =
            weaponOptions.Count > 0;

        bool hasEquipments =
            equipmentOptions.Count > 0;

        // 武器・装備の両方が候補にある
        if (hasWeapons && hasEquipments)
        {
            // 必ず最低1個ずつ入れる
            result.Add(weaponOptions[0]);
            result.Add(equipmentOptions[0]);

            // 残り1枠
            List<UpgradeOption> remaining =
                new List<UpgradeOption>();

            for (int i = 1;
                 i < weaponOptions.Count;
                 i++)
            {
                remaining.Add(
                    weaponOptions[i]
                );
            }

            for (int i = 1;
                 i < equipmentOptions.Count;
                 i++)
            {
                remaining.Add(
                    equipmentOptions[i]
                );
            }

            Shuffle(remaining);

            if (remaining.Count > 0)
            {
                result.Add(
                    remaining[0]
                );
            }
            else
            {
                // 候補が2種類しかない場合の保険
                result.Add(
                    Random.value < 0.5f
                    ? weaponOptions[0]
                    : equipmentOptions[0]
                );
            }
        }

        // 武器しか残っていない
        else if (hasWeapons)
        {
            AddOptions(
                result,
                weaponOptions,
                3
            );
        }

        // 装備しか残っていない
        else if (hasEquipments)
        {
            AddOptions(
                result,
                equipmentOptions,
                3
            );
        }

        // 表示順もランダムにする
        Shuffle(result);

        return result;
    }

    // =========================================
    // 武器候補
    // =========================================

    List<UpgradeOption> CreateWeaponOptions()
    {
        List<UpgradeOption> result =
            new List<UpgradeOption>();

        // 枠が空いている
        if (loadout.HasWeaponSlot())
        {
            // 未所持武器だけ
            List<PlayerLoadout.WeaponType> weapons =
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
            // 枠が埋まっているので
            // 所持済み＆Lv3未満だけ
            List<PlayerLoadout.WeaponType> weapons =
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

    // =========================================
    // 装備候補
    // =========================================

    List<UpgradeOption> CreateEquipmentOptions()
    {
        List<UpgradeOption> result =
            new List<UpgradeOption>();

        // 枠が空いている
        if (loadout.HasEquipmentSlot())
        {
            // 未所持装備だけ
            List<PlayerLoadout.EquipmentType> equipments =
                loadout.GetUnownedEquipments();

            foreach (
                PlayerLoadout.EquipmentType equipment
                in equipments
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = false;
                option.equipment = equipment;

                result.Add(option);
            }
        }
        else
        {
            // 枠が埋まっているので
            // 所持済み＆Lv3未満だけ
            List<PlayerLoadout.EquipmentType> equipments =
                loadout.GetOwnedEquipmentsBelowMax();

            foreach (
                PlayerLoadout.EquipmentType equipment
                in equipments
            )
            {
                UpgradeOption option =
                    new UpgradeOption();

                option.isWeapon = false;
                option.equipment = equipment;

                result.Add(option);
            }
        }

        return result;
    }

    // =========================================
    // UI表示
    // =========================================

    void OpenOptionUI()
    {
        string option1 =
            GetOptionText(
                currentOptions[0]
            );

        string option2 =
            GetOptionText(
                currentOptions[1]
            );

        string option3 =
            GetOptionText(
                currentOptions[2]
            );

        levelUpUI.Open(
            this,
            option1,
            option2,
            option3
        );
    }

    string GetOptionText(
        UpgradeOption option
    )
    {
        if (option.isWeapon)
        {
            int level =
                loadout.GetWeaponLevel(
                    option.weapon
                );

            if (level == 0)
            {
                return
                    "WEAPON\n" +
                    GetWeaponName(
                        option.weapon
                    ) +
                    "\nNEW";
            }

            return
                "WEAPON\n" +
                GetWeaponName(
                    option.weapon
                ) +
                "\nLv." +
                level +
                " → Lv." +
                (level + 1);
        }
        else
        {
            int level =
                loadout.GetEquipmentLevel(
                    option.equipment
                );

            if (level == 0)
            {
                return
                    "EQUIPMENT\n" +
                    GetEquipmentName(
                        option.equipment
                    ) +
                    "\nNEW";
            }

            return
                "EQUIPMENT\n" +
                GetEquipmentName(
                    option.equipment
                ) +
                "\nLv." +
                level +
                " → Lv." +
                (level + 1);
        }
    }

    // =========================================
    // 選択
    // =========================================

    public void SelectOption(int index)
    {
        if (!isChoosing)
        {
            return;
        }

        if (index < 0 ||
            index >= currentOptions.Count)
        {
            return;
        }

        UpgradeOption option =
            currentOptions[index];

        if (option.isWeapon)
        {
            ApplyWeaponUpgrade(
                option.weapon
            );
        }
        else
        {
            ApplyEquipmentUpgrade(
                option.equipment
            );
        }

        levelUpCount++;

        isChoosing = false;

        playerHealth
            .SetLevelUpInvincible(false);

        currentOptions.Clear();
    }

    // =========================================
    // 武器強化
    // =========================================

    void ApplyWeaponUpgrade(
        PlayerLoadout.WeaponType weapon
    )
    {
        bool success =
            loadout.AddOrLevelUpWeapon(
                weapon
            );

        if (!success)
        {
            return;
        }

        // 現在実装済みの武器
        if (weapon ==
            PlayerLoadout.WeaponType.CandyShooter)
        {
            // 新規取得時ではなく
            // Lv2以上になった時だけ強化
            int level =
                loadout.GetWeaponLevel(
                    weapon
                );

            if (level >= 2 &&
                candyShooter != null)
            {
                candyShooter
                    .LevelUpWeapon();
            }
        }

        Debug.Log(
            "Weapon : " +
            weapon +
            " Lv." +
            loadout.GetWeaponLevel(
                weapon
            )
        );
    }

    // =========================================
    // 装備強化
    // =========================================

    void ApplyEquipmentUpgrade(
        PlayerLoadout.EquipmentType equipment
    )
    {
        bool success =
            loadout.AddOrLevelUpEquipment(
                equipment
            );

        if (!success)
        {
            return;
        }

        int level =
            loadout.GetEquipmentLevel(
                equipment
            );

        switch (equipment)
        {
            // 移動速度アップ
            case PlayerLoadout.EquipmentType.WitchBoots:

                playerController
                    .IncreaseMoveSpeed(
                        0.10f
                    );

                break;

            // 最大HPアップ
            case PlayerLoadout.EquipmentType.FrankenBolt:

                playerHealth
                    .IncreaseMaxHP(
                        20.0f
                    );

                break;

            // ↓この4種類は後から効果を実装
            case PlayerLoadout.EquipmentType.CandyBag:

                Debug.Log(
                    "Candy Bag Lv." +
                    level
                );

                break;

            case PlayerLoadout.EquipmentType.VampireCape:

                Debug.Log(
                    "Vampire Cape Lv." +
                    level
                );

                break;

            case PlayerLoadout.EquipmentType.BlackCatCharm:

                Debug.Log(
                    "Black Cat Charm Lv." +
                    level
                );

                break;

            case PlayerLoadout.EquipmentType.MoonClock:

                Debug.Log(
                    "Moon Clock Lv." +
                    level
                );

                break;
        }
    }

    // =========================================
    // 全強化完了判定
    // =========================================

    bool HasAnyUpgrade()
    {
        if (loadout.HasWeaponSlot())
        {
            if (loadout.GetUnownedWeapons().Count > 0)
            {
                return true;
            }
        }
        else
        {
            if (loadout
                .GetOwnedWeaponsBelowMax()
                .Count > 0)
            {
                return true;
            }
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
        else
        {
            if (loadout
                .GetOwnedEquipmentsBelowMax()
                .Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    // =========================================
    // レベルアップ必要キャンディ
    // =========================================

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
                    (levelUpCount - 10) * 80;
        }
    }

    // =========================================
    // 表示名
    // =========================================

    string GetWeaponName(
        PlayerLoadout.WeaponType weapon
    )
    {
        switch (weapon)
        {
            case PlayerLoadout.WeaponType.CandyShooter:
                return "Candy Shooter";

            case PlayerLoadout.WeaponType.PumpkinBomb:
                return "Pumpkin Bomb";

            case PlayerLoadout.WeaponType.GhostLantern:
                return "Ghost Lantern";

            case PlayerLoadout.WeaponType.DeathScythe:
                return "Death Scythe";

            case PlayerLoadout.WeaponType.BatFamiliar:
                return "Bat Familiar";

            case PlayerLoadout.WeaponType.WitchBroom:
                return "Witch Broom";
        }

        return weapon.ToString();
    }

    string GetEquipmentName(
        PlayerLoadout.EquipmentType equipment
    )
    {
        switch (equipment)
        {
            case PlayerLoadout.EquipmentType.CandyBag:
                return "Candy Bag";

            case PlayerLoadout.EquipmentType.WitchBoots:
                return "Witch Boots";

            case PlayerLoadout.EquipmentType.FrankenBolt:
                return "Franken Bolt";

            case PlayerLoadout.EquipmentType.VampireCape:
                return "Vampire Cape";

            case PlayerLoadout.EquipmentType.BlackCatCharm:
                return "Black Cat Charm";

            case PlayerLoadout.EquipmentType.MoonClock:
                return "Moon Clock";
        }

        return equipment.ToString();
    }

    // =========================================
    // Listをランダムに並び替える
    // =========================================

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
        List<UpgradeOption> destination,
        List<UpgradeOption> source,
        int amount
    )
    {
        Shuffle(source);

        for (
            int i = 0;
            i < amount;
            i++
        )
        {
            // 候補が3個未満になった場合の保険
            destination.Add(
                source[
                    i % source.Count
                ]
            );
        }
    }
}