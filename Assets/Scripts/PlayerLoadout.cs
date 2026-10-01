using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public enum WeaponType
    {
        CandyShooter,
        PumpkinBomb,
        GhostLantern,
        DeathScythe,
        BatFamiliar,
        WitchBroom
    }

    public enum EquipmentType
    {
        CandyBag,
        WitchBoots,
        FrankenBolt,
        VampireCape,
        BlackCatCharm,
        MoonClock
    }

    [Header("Slot Limit")]
    [SerializeField]
    private int maxWeaponSlots = 3;

    [SerializeField]
    private int maxEquipmentSlots = 3;

    private Dictionary<WeaponType, int> weapons =
        new Dictionary<WeaponType, int>();

    private Dictionary<EquipmentType, int> equipments =
        new Dictionary<EquipmentType, int>();

    public int WeaponCount
    {
        get { return weapons.Count; }
    }

    public int EquipmentCount
    {
        get { return equipments.Count; }
    }

    public bool HasWeapon(WeaponType weapon)
    {
        return weapons.ContainsKey(weapon);
    }

    public bool HasEquipment(EquipmentType equipment)
    {
        return equipments.ContainsKey(equipment);
    }

    public int GetWeaponLevel(WeaponType weapon)
    {
        if (weapons.ContainsKey(weapon))
        {
            return weapons[weapon];
        }

        return 0;
    }

    public int GetEquipmentLevel(
        EquipmentType equipment
    )
    {
        if (equipments.ContainsKey(equipment))
        {
            return equipments[equipment];
        }

        return 0;
    }

    public bool HasWeaponSlot()
    {
        return weapons.Count < maxWeaponSlots;
    }

    public bool HasEquipmentSlot()
    {
        return equipments.Count < maxEquipmentSlots;
    }

    public bool AddOrLevelUpWeapon(
        WeaponType weapon
    )
    {
        if (weapons.ContainsKey(weapon))
        {
            if (weapons[weapon] >= 3)
            {
                return false;
            }

            weapons[weapon]++;

            Debug.Log(
                weapon +
                " Lv." +
                weapons[weapon]
            );

            return true;
        }

        if (!HasWeaponSlot())
        {
            return false;
        }

        weapons.Add(weapon, 1);

        Debug.Log(
            weapon +
            " GET! Lv.1"
        );

        return true;
    }

    public bool AddOrLevelUpEquipment(
        EquipmentType equipment
    )
    {
        if (equipments.ContainsKey(equipment))
        {
            if (equipments[equipment] >= 3)
            {
                return false;
            }

            equipments[equipment]++;

            Debug.Log(
                equipment +
                " Lv." +
                equipments[equipment]
            );

            return true;
        }

        if (!HasEquipmentSlot())
        {
            return false;
        }

        equipments.Add(equipment, 1);

        Debug.Log(
            equipment +
            " GET! Lv.1"
        );

        return true;
    }

    public List<WeaponType> GetUnownedWeapons()
    {
        List<WeaponType> result =
            new List<WeaponType>();

        foreach (
            WeaponType weapon
            in System.Enum.GetValues(
                typeof(WeaponType)
            )
        )
        {
            if (!HasWeapon(weapon))
            {
                result.Add(weapon);
            }
        }

        return result;
    }

    public List<EquipmentType>
        GetUnownedEquipments()
    {
        List<EquipmentType> result =
            new List<EquipmentType>();

        foreach (
            EquipmentType equipment
            in System.Enum.GetValues(
                typeof(EquipmentType)
            )
        )
        {
            if (!HasEquipment(equipment))
            {
                result.Add(equipment);
            }
        }

        return result;
    }

    public List<WeaponType>
        GetOwnedWeaponsBelowMax()
    {
        List<WeaponType> result =
            new List<WeaponType>();

        foreach (
            KeyValuePair<WeaponType, int> pair
            in weapons
        )
        {
            if (pair.Value < 3)
            {
                result.Add(pair.Key);
            }
        }

        return result;
    }

    public List<EquipmentType>
        GetOwnedEquipmentsBelowMax()
    {
        List<EquipmentType> result =
            new List<EquipmentType>();

        foreach (
            KeyValuePair<EquipmentType, int> pair
            in equipments
        )
        {
            if (pair.Value < 3)
            {
                result.Add(pair.Key);
            }
        }

        return result;
    }

    // •”L‚Ì‚¨Žç‚è
    // ‘S•Ší‚ÌUŒ‚—Í”{—¦
    public float GetAttackMultiplier()
    {
        int level =
            GetEquipmentLevel(
                EquipmentType.BlackCatCharm
            );

        switch (level)
        {
            case 1:
                return 1.10f;

            case 2:
                return 1.20f;

            case 3:
                return 1.30f;

            default:
                return 1.00f;
        }
    }

    // ŒŽ–é‚ÌŽžŒv
    // ‘S•Ší‚ÌUŒ‚ŠÔŠu”{—¦
    public float GetCooldownMultiplier()
    {
        int level =
            GetEquipmentLevel(
                EquipmentType.MoonClock
            );

        switch (level)
        {
            case 1:
                return 0.92f;

            case 2:
                return 0.84f;

            case 3:
                return 0.75f;

            default:
                return 1.00f;
        }
    }
}