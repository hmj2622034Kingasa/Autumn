using UnityEngine;

public class CandyBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float lifeTime = 3.0f;

    private Rigidbody2D rb;

    // Ç±ÇÃíeÇåÇÇ¡ÇΩêl
    private Contestant owner;

    void Awake()
    {
        rb =
            GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(
            gameObject,
            lifeTime
        );
    }

    public void SetDirection(
        Vector2 direction
    )
    {
        if (rb == null)
        {
            return;
        }

        rb.linearVelocity =
            direction.normalized *
            speed;
    }

    public void SetDamage(
        float newDamage
    )
    {
        damage =
            newDamage;
    }

    public void SetOwner(
        Contestant newOwner
    )
    {
        owner =
            newOwner;
    }

    void OnTriggerEnter2D(
        Collider2D other
    )
    {
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();

        if (enemy == null)
        {
            return;
        }

        enemy.TakeDamage(
            damage,
            owner
        );

        Destroy(gameObject);
    }
}