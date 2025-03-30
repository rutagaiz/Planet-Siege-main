using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Minimap Settings")]
    [SerializeField] private GameObject minimapIconPrefab;
    [SerializeField] private RectTransform minimapUI;
    [SerializeField] private Camera minimapCamera;

    private float timeUntilSpawn;

    void Start()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Enemy Prefab or Spawn Point is missing!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        AttachMinimapIcon(enemy);
    }

    private void AttachMinimapIcon(GameObject enemy)
    {
        if (minimapIconPrefab == null || minimapUI == null || minimapCamera == null)
        {
            Debug.LogWarning("Minimap references are missing!");
            return;
        }

        GameObject icon = Instantiate(minimapIconPrefab, minimapUI);
        RectTransform iconTransform = icon.GetComponent<RectTransform>();

        if (iconTransform == null)
        {
            Debug.LogWarning("Minimap icon prefab is missing a RectTransform!");
            return;
        }

        // Attach minimap tracking component to enemy
        EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
        tracker.Initialize(enemy.transform, iconTransform, minimapCamera, minimapUI);
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }


}
