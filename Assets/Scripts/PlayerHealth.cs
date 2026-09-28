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
        // ダウン中または無敵中ならダメージを受けない
        if (isDown || isInvincible)
        {
            return;
        }

        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log("Player HP : " + currentHP);

        if (currentHP <= 0)
        {
            StartCoroutine(DownRoutine());
        }
    }

    IEnumerator DownRoutine()
    {
        isDown = true;

        Debug.Log("Player Down!");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 3秒間ダウン
        yield return new WaitForSeconds(respawnTime);

        // HP全回復
        currentHP = maxHP;

        isDown = false;

        // 復活直後は少し無敵
        isInvincible = true;

        Debug.Log("Player Respawn!");

        yield return new WaitForSeconds(respawnInvincibleTime);

        isInvincible = false;
    }
}