using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]
    private float maxHP = 100.0f;

    [Header("Respawn")]
    [SerializeField]
    private float respawnTime = 3.0f;

    [SerializeField]
    private float respawnInvincibleTime = 1.0f;

    private float currentHP;

    private bool isDown = false;
    private bool isInvincible = false;
    private bool isLevelUpInvincible = false;

    public float CurrentHP
    {
        get { return currentHP; }
    }

    public float MaxHP
    {
        get { return maxHP; }
    }

    public bool IsDown
    {
        get { return isDown; }
    }

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isDown ||
            isInvincible ||
            isLevelUpInvincible)
        {
            return;
        }

        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log(
            "Player HP : " +
            currentHP
        );

        if (currentHP <= 0)
        {
            StartCoroutine(
                DownRoutine()
            );
        }
    }

    IEnumerator DownRoutine()
    {
        isDown = true;

        Debug.Log(
            "Player Down!"
        );

        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity =
                Vector2.zero;
        }

        yield return
            new WaitForSeconds(
                respawnTime
            );

        currentHP = maxHP;

        isDown = false;
        isInvincible = true;

        Debug.Log(
            "Player Respawn!"
        );

        yield return
            new WaitForSeconds(
                respawnInvincibleTime
            );

        isInvincible = false;
    }

    public void SetLevelUpInvincible(
        bool value
    )
    {
        isLevelUpInvincible = value;
    }

    public void IncreaseMaxHP(float amount)
    {
        maxHP += amount;

        currentHP += amount;

        Debug.Log(
            "Max HP : " +
            maxHP
        );
    }

    // ‹zŒŒ‹S‚Ìƒ}ƒ“ƒg‚È‚Ç‚ÅŽg—p
    public void Heal(float amount)
    {
        if (isDown)
        {
            return;
        }

        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        Debug.Log(
            "Player Heal! HP : " +
            currentHP +
            " / " +
            maxHP
        );
    }
}