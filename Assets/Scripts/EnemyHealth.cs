using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Œ»İ‘¶İ‚µ‚Ä‚¢‚é“G‚ğ‚Ü‚Æ‚ß‚ÄŠÇ—
    public static List<EnemyHealth> AllEnemies = new List<EnemyHealth>();

    [SerializeField]
    private float maxHP = 30.0f;

    private float currentHP;

    public float CurrentHP
    {
        get { return currentHP; }
    }

    void Start()
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

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP : " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        AllEnemies.Remove(this);

        Destroy(gameObject);
    }
}