using UnityEngine;

public class CandyBullet : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float lifeTime = 3.0f;

    private Rigidbody2D rb;

    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * moveSpeed;

        // âÊñ äOÇ»Ç«Ç…îÚÇÒÇ≈Ç¢Ç¡ÇΩíeÇé©ìÆçÌèú
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth =
            other.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            return;
        }

        enemyHealth.TakeDamage(damage);

        Destroy(gameObject);
    }
}