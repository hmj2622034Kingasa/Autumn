using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    public static CandySpawner Instance
    {
        get;
        private set;
    }

    [Header("Candy Prefabs")]
    [SerializeField]
    private GameObject blueCandyPrefab;

    [SerializeField]
    private GameObject redCandyPrefab;

    [SerializeField]
    private GameObject goldCandyPrefab;

    void Awake()
    {
        Instance = this;
    }

    // 通常ドロップ
    public void SpawnCandy(
        int totalValue,
        Vector2 position
    )
    {
        SpawnCandy(
            totalValue,
            position,
            0.4f
        );
    }

    // 広範囲ドロップ
    public void SpawnCandy(
        int totalValue,
        Vector2 position,
        float spreadRadius
    )
    {
        int remaining =
            totalValue;

        while (remaining > 0)
        {
            GameObject prefab;
            int value;

            if (remaining >= 5 &&
                Random.value < 0.50f)
            {
                prefab =
                    goldCandyPrefab;

                value = 5;
            }
            else if (remaining >= 3)
            {
                prefab =
                    redCandyPrefab;

                value = 3;
            }
            else
            {
                prefab =
                    blueCandyPrefab;

                value = 1;
            }

            if (prefab == null)
            {
                return;
            }

            Vector2 offset =
                Random.insideUnitCircle *
                spreadRadius;

            Instantiate(
                prefab,
                position + offset,
                Quaternion.identity
            );

            remaining -= value;
        }
    }
}