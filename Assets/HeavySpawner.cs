using UnityEngine;

public class HeavySpawner : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;  // Enemy prefab to spawn
    [SerializeField] private Transform spawnPoint;    // Enemy base spawn point
    [SerializeField] private float minSpawnTime = 2f; // Min time between spawns
    [SerializeField] private float maxSpawnTime = 5f; // Max time between spawns
    [SerializeField] private int towersToDestroyBeforeSpawning = 1; // How many towers must be destroyed

    [Header("Minimap Settings")]
    [SerializeField] private GameObject minimapIconPrefab;
    [SerializeField] private RectTransform minimapUI;
    [SerializeField] private Camera minimapCamera;

    private float timeUntilSpawn;
    private bool isSpawningEnabled = false;

    void Start()
    {
        SetTimeUntilSpawn();

        // Subscribe to tower destruction event
        turretScript.OnTowerDestroyed += CheckForSpawning;
    }

    void OnDestroy()
    {
        // Unsubscribe from event to avoid memory leaks
        turretScript.OnTowerDestroyed -= CheckForSpawning;
    }

    void Update()
    {
        if (!isSpawningEnabled) return; // Wait until conditions are met

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

    private void CheckForSpawning(int destroyedCount)
    {
        if (destroyedCount >= towersToDestroyBeforeSpawning)
        {
            isSpawningEnabled = true;
            Debug.Log($"Enemy spawning started after {destroyedCount} towers were destroyed!");
        }
    }

}
