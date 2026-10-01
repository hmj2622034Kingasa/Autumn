using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;

    [SerializeField]
    private float damage = 7.0f;

    [SerializeField]
    private float lifeTime = 5.0f;

    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity =
            direction * moveSpeed;

        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction =
            newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            return;
        }

        playerHealth.TakeDamage(damage);

        Destroy(gameObject);
    }
}