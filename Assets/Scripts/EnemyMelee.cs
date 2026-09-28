using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 2.0f;

    [Header("Attack")]
    [SerializeField]
    private float attackDamage = 10.0f;

    [SerializeField]
    private float attackInterval = 1.0f;

    private Transform player;
    private Rigidbody2D rb;

    private float nextAttackTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        playerHealth.TakeDamage(attackDamage);

        nextAttackTime = Time.time + attackInterval;
    }
}