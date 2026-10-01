using UnityEngine;

public class CandyShooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField]
    private GameObject bulletPrefab;

    [Header("Attack")]
    [SerializeField]
    private float attackRange = 8.0f;

    private float attackInterval = 0.6f;

    private float nextAttackTime = 0.0f;

    private int weaponLevel = 1;

    private PlayerLoadout loadout;

    private Contestant contestant;
    public int WeaponLevel
    {
        get { return weaponLevel; }
    }

    void Start()
    {
        loadout = 
            GetComponent<PlayerLoadout>();
        contestant = 
            GetComponent<Contestant>();
    }

    void Update()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        EnemyHealth target =
            FindNearestEnemy();

        if (target == null)
        {
            return;
        }

        Shoot(target);

        nextAttackTime =
            Time.time +
            attackInterval *
            loadout.GetCooldownMultiplier();
    }

    EnemyHealth FindNearestEnemy()
    {
        EnemyHealth nearestEnemy = null;

        float nearestDistance =
            attackRange;

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

            if (distance <
                nearestDistance)
            {
                nearestDistance =
                    distance;

                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot(EnemyHealth target)
    {
        Vector2 direction =
            ((Vector2)target.transform.position -
             (Vector2)transform.position)
            .normalized;

        GameObject bulletObject =
            Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

        CandyBullet bullet =
            bulletObject
            .GetComponent<CandyBullet>();

        if (bullet != null)
        {
            bullet.SetDirection(
                direction);

            bullet.SetOwner(
                contestant
                );

            if (loadout != null)
            {
                float finalDamage =
                    10.0f *
                    loadout.GetAttackMultiplier();

                bullet.SetDamage(finalDamage);
            } 

           
        }
    }

    public void LevelUpWeapon()
    {
        if (weaponLevel >= 3)
        {
            return;
        }

        weaponLevel++;

        if (weaponLevel == 2)
        {
            attackInterval = 0.30f;
        }
        else if (weaponLevel == 3)
        {
            attackInterval = 0.10f;
        }

        Debug.Log(
            "Candy Shooter Lv." +
            weaponLevel
        );
    }
}