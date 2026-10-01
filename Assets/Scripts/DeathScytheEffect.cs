using UnityEngine;

public class DeathScytheEffect : MonoBehaviour
{
    private Transform owner;

    private float rotateSpeed;
    private float lifeTime;

    private float currentLife = 0.0f;

    public void Setup(
        Transform player,
        float startAngle,
        float totalAngle,
        float duration
    )
    {
        owner = player;

        transform.position =
            owner.position;

        transform.rotation =
            Quaternion.Euler(
                0.0f,
                0.0f,
                startAngle
            );

        rotateSpeed =
            totalAngle / duration;

        lifeTime = duration;
    }

    void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position =
            owner.position;

        transform.Rotate(
            0.0f,
            0.0f,
            rotateSpeed * Time.deltaTime
        );

        currentLife +=
            Time.deltaTime;

        if (currentLife >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}