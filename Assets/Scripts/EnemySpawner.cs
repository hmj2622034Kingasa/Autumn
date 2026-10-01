using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField]
    private GameObject meleeEnemyPrefab;

    [SerializeField]
    private GameObject rangedEnemyPrefab;

    [SerializeField]
    private GameObject tankEnemyPrefab;

    [Header("Spawn Area")]
    [SerializeField]
    private float spawnDistanceX = 10.0f;

    [SerializeField]
    private float spawnDistanceY = 6.0f;

    [Header("Enemy Limit")]
    [SerializeField]
    private int maxEnemies = 120;

    private float nextSpawnTime = 0.0f;

    void Update()
    {
        if (GameTimer.Instance == null)
        {
            return;
        }

        // 5ï™åoâﬂÇµÇΩÇÁí èÌìGÇÃèoåªÇé~ÇﬂÇÈ
        if (GameTimer.Instance.ElapsedTime >= 300.0f)
        {
            return;
        }

        EnemyHealth.AllEnemies.RemoveAll(
            enemy => enemy == null
            );
        // ìGÇ™ëΩÇ∑Ç¨ÇÈèÍçáÇÕèoåªÇ≥ÇπÇ»Ç¢
        if (EnemyHealth.AllEnemies.Count >= maxEnemies)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();

            nextSpawnTime =
                Time.time + GetSpawnInterval();
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyPrefab = ChooseEnemy();

        if (enemyPrefab == null)
        {
            return;
        }

        Vector2 spawnPosition =
            GetSpawnPosition();

        Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    GameObject ChooseEnemy()
    {
        float time = GameTimer.Instance.ElapsedTime;

        float random = Random.value;

        // 0Å`1ï™
        if (time < 60.0f)
        {
            // ãﬂãóó£ 80%
            // âìãóó£ 15%
            // É^ÉìÉN 5%

            if (random < 0.80f)
            {
                return meleeEnemyPrefab;
            }
            else if (random < 0.95f)
            {
                return rangedEnemyPrefab;
            }
            else
            {
                return tankEnemyPrefab;
            }
        }

        // 1Å`3ï™
        if (time < 180.0f)
        {
            // ãﬂãóó£ 65%
            // âìãóó£ 20%
            // É^ÉìÉN 15%

            if (random < 0.65f)
            {
                return meleeEnemyPrefab;
            }
            else if (random < 0.85f)
            {
                return rangedEnemyPrefab;
            }
            else
            {
                return tankEnemyPrefab;
            }
        }

        // 3Å`5ï™
        // ãﬂãóó£ 50%
        // âìãóó£ 25%
        // É^ÉìÉN 25%

        if (random < 0.50f)
        {
            return meleeEnemyPrefab;
        }
        else if (random < 0.75f)
        {
            return rangedEnemyPrefab;
        }
        else
        {
            return tankEnemyPrefab;
        }
    }

    float GetSpawnInterval()
    {
        float time = GameTimer.Instance.ElapsedTime;

        // 0Å`1ï™
        if (time < 60.0f)
        {
            return 0.60f;
        }

        // 1Å`2ï™
        if (time < 120.0f)
        {
            return 0.45f;
        }

        // 2Å`3ï™
        if (time < 180.0f)
        {
            return 0.32f;
        }

        // 3Å`4ï™
        if (time < 240.0f)
        {
            return 0.24f;
        }

        // 4Å`5ï™
        return 0.18f;
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 center = Vector2.zero;

        // âÊñ ÇÃè„â∫ç∂âEÇ«Ç±Ç©ÇÁèoÇ∑Ç©
        int side = Random.Range(0, 4);

        switch (side)
        {
            // è„
            case 0:
                return new Vector2(
                    Random.Range(-spawnDistanceX, spawnDistanceX),
                    spawnDistanceY
                );

            // â∫
            case 1:
                return new Vector2(
                    Random.Range(-spawnDistanceX, spawnDistanceX),
                    -spawnDistanceY
                );

            // âE
            case 2:
                return new Vector2(
                    spawnDistanceX,
                    Random.Range(-spawnDistanceY, spawnDistanceY)
                );

            // ç∂
            default:
                return new Vector2(
                    -spawnDistanceX,
                    Random.Range(-spawnDistanceY, spawnDistanceY)
                );
        }
    }
}