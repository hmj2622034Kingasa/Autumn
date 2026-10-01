using System.Collections.Generic;
using UnityEngine;

public class MidBossRewardPickup : MonoBehaviour
{
    private Contestant owner;

    [SerializeField]
    private float rotateSpeed = 120.0f;

    [SerializeField]
    private float healAmount = 30.0f;

    private bool collected = false;

    public void Setup(
        Contestant newOwner
    )
    {
        owner =
            newOwner;
    }

    void Update()
    {
        transform.Rotate(
            0.0f,
            0.0f,
            rotateSpeed *
            Time.deltaTime
        );
    }

    void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (collected)
        {
            return;
        }

        Contestant contestant =
            other.GetComponentInParent<
                Contestant
            >();

        if (contestant == null)
        {
            return;
        }

        // 撃破者本人以外は通り抜ける
        if (contestant != owner)
        {
            return;
        }

        CollectReward(
            contestant
        );
    }

    void CollectReward(
        Contestant contestant
    )
    {
        PlayerLoadout loadout =
            contestant.GetComponent<
                PlayerLoadout
            >();

        PlayerHealth health =
            contestant.GetComponent<
                PlayerHealth
            >();

        if (loadout == null)
        {
            return;
        }

        collected = true;

        // ==========================
        // 1.
        // 所持武器に未MAXがある
        // ==========================

        List<PlayerLoadout.WeaponType>
            upgradeable =
            loadout
                .GetOwnedWeaponsBelowMax();

        if (upgradeable.Count > 0)
        {
            PlayerLoadout.WeaponType weapon =
                upgradeable[
                    Random.Range(
                        0,
                        upgradeable.Count
                    )
                ];

            loadout.AddOrLevelUpWeapon(
                weapon
            );

            SyncCandyShooter(
                contestant,
                weapon
            );

            Debug.Log(
                contestant.DisplayName +
                " が中ボス報酬で " +
                weapon +
                " をレベルアップ！"
            );

            Destroy(gameObject);

            return;
        }

        // ==========================
        // 2.
        // 所持武器は全部MAXだが
        // 武器枠に空きがある
        // ==========================

        if (loadout.HasWeaponSlot())
        {
            List<PlayerLoadout.WeaponType>
                unowned =
                loadout
                    .GetUnownedWeapons();

            if (unowned.Count > 0)
            {
                PlayerLoadout.WeaponType weapon =
                    unowned[
                        Random.Range(
                            0,
                            unowned.Count
                        )
                    ];

                loadout.AddOrLevelUpWeapon(
                    weapon
                );

                Debug.Log(
                    contestant.DisplayName +
                    " が中ボス報酬で " +
                    weapon +
                    " を獲得！"
                );

                Destroy(gameObject);

                return;
            }
        }

        // ==========================
        // 3.
        // 全枠MAXならHP回復
        // ==========================

        if (health != null)
        {
            health.Heal(
                healAmount
            );
        }

        Debug.Log(
            contestant.DisplayName +
            " は武器が完成しているため" +
            " HPを回復！"
        );

        Destroy(gameObject);
    }

    void SyncCandyShooter(
        Contestant contestant,
        PlayerLoadout.WeaponType weapon
    )
    {
        if (weapon !=
            PlayerLoadout
                .WeaponType.CandyShooter)
        {
            return;
        }

        CandyShooter shooter =
            contestant.GetComponent<
                CandyShooter
            >();

        if (shooter != null)
        {
            shooter.LevelUpWeapon();
        }
    }
}