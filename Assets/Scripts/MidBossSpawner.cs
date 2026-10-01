using UnityEngine;

public class MidBossSpawner : MonoBehaviour
{
    [Header("Mid Boss Prefabs")]
    [SerializeField]
    private GameObject meleeMidBossPrefab;

    [SerializeField]
    private GameObject rangedMidBossPrefab;

    [Header("Spawn")]
    [SerializeField]
    private int minSpawnCount = 4;

    [SerializeField]
    private int maxSpawnCount = 7;

    [SerializeField]
    private float minSpawnDistance = 8.0f;

    [SerializeField]
    private float maxSpawnDistance = 11.0f;

    private bool spawnedAtTwoMinutes = false;
    private bool spawnedAtFourMinutes = false;

    private Transform player;

    void Start()
    {
        GameObject playerObject =
            GameObject.Find("Player");

        if (playerObject != null)
        {
            player =
                playerObject.transform;
        }
    }

    void Update()
    {
        if (GameTimer.Instance == null)
        {
            return;
        }

        float time =
            GameTimer.Instance.ElapsedTime;

        // 2分
        if (!spawnedAtTwoMinutes &&
            time >= 120.0f)
        {
            spawnedAtTwoMinutes = true;

            SpawnMidBossWave(
                "2:00"
            );
        }

        // 4分
        if (!spawnedAtFourMinutes &&
            time >= 240.0f)
        {
            spawnedAtFourMinutes = true;

            SpawnMidBossWave(
                "4:00"
            );
        }
    }

    void SpawnMidBossWave(
        string waveName
    )
    {
        int count =
            Random.Range(
                minSpawnCount,
                maxSpawnCount + 1
            );

        Debug.Log(
            waveName +
            " 中ボス襲来！ " +
            count +
            "体出現！"
        );

        for (
            int i = 0;
            i < count;
            i++
        )
        {
            SpawnOneMidBoss();
        }
    }

    void SpawnOneMidBoss()
    {
        GameObject prefab;

        // 50%ずつ
        if (Random.value < 0.5f)
        {
            prefab =
                meleeMidBossPrefab;
        }
        else
        {
            prefab =
                rangedMidBossPrefab;
        }

        if (prefab == null)
        {
            return;
        }

        Vector2 center =
            player != null
            ? (Vector2)player.position
            : Vector2.zero;

        float angle =
            Random.Range(
                0.0f,
                360.0f
            );

        float distance =
            Random.Range(
                minSpawnDistance,
                maxSpawnDistance
            );

        Vector2 direction =
            new Vector2(
                Mathf.Cos(
                    angle *
                    Mathf.Deg2Rad
                ),
                Mathf.Sin(
                    angle *
                    Mathf.Deg2Rad
                )
            );

        Vector2 spawnPosition =
            center +
            direction * distance;

        Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}