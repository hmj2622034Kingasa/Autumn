using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    [Header("Candy")]
    [SerializeField]
    private int candyValue = 1;

    [Header("Pickup")]
    [SerializeField]
    private float baseAttractRange = 1.2f;

    [SerializeField]
    private float attractSpeed = 6.0f;

    [SerializeField]
    private float collectDistance = 0.2f;

    public int CandyValue
    {
        get { return candyValue; }
    }

    void Update()
    {
        Contestant target =
            FindBestTarget();

        if (target == null)
        {
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                target.transform.position
            );

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                target.transform.position,
                attractSpeed * Time.deltaTime
            );

        distance =
            Vector2.Distance(
                transform.position,
                target.transform.position
            );

        if (distance <= collectDistance)
        {
            PlayerCandy candy =
                target.GetComponent<PlayerCandy>();

            CollectCandy(candy);
        }
    }

    Contestant FindBestTarget()
    {
        Contestant bestTarget = null;

        float bestDistance =
            float.MaxValue;

        foreach (
            Contestant contestant
            in Contestant.All
        )
        {
            if (contestant == null)
            {
                continue;
            }

            PlayerCandy candy =
                contestant.GetComponent<PlayerCandy>();

            PlayerHealth health =
                contestant.GetComponent<PlayerHealth>();

            PlayerLoadout loadout =
                contestant.GetComponent<PlayerLoadout>();

            if (candy == null)
            {
                continue;
            }

            if (health != null &&
                health.IsDown)
            {
                continue;
            }

            float attractRange =
                GetAttractRange(loadout);

            float distance =
                Vector2.Distance(
                    transform.position,
                    contestant.transform.position
                );

            if (distance <= attractRange &&
                distance < bestDistance)
            {
                bestDistance = distance;
                bestTarget = contestant;
            }
        }

        return bestTarget;
    }

    float GetAttractRange(
        PlayerLoadout loadout
    )
    {
        if (loadout == null)
        {
            return baseAttractRange;
        }

        int level =
            loadout.GetEquipmentLevel(
                PlayerLoadout.EquipmentType.CandyBag
            );

        switch (level)
        {
            case 1:
                return baseAttractRange * 1.20f;

            case 2:
                return baseAttractRange * 1.40f;

            case 3:
                return baseAttractRange * 1.65f;

            default:
                return baseAttractRange;
        }
    }

    void OnTriggerEnter2D(
        Collider2D other
    )
    {
        PlayerCandy candy =
            other.GetComponent<PlayerCandy>();

        if (candy == null)
        {
            return;
        }

        CollectCandy(candy);
    }

    void CollectCandy(
        PlayerCandy target
    )
    {
        if (target == null)
        {
            return;
        }

        target.AddCandy(
            candyValue
        );

        Destroy(gameObject);
    }
}