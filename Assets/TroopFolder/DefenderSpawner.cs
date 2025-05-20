using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    public GameObject troop1;
    public GameObject troop2;
    public GameObject troop3;
    public GameObject troop4;
    public GameObject troop5;
    public GameObject spawnPoint;

    private KeyRebinder keyRebinder;

    void Start()
    {
        // Find KeyRebinder in active or inactive objects
        keyRebinder = Object.FindFirstObjectByType<KeyRebinder>();

        if (keyRebinder == null)
        {
            Debug.LogError("KeyRebinder not found! Attach it to SettingsPanel.");
            return;
        }

        // Verify initialization
        if (!keyRebinder.IsInitialized())
        {
            keyRebinder.Initialize();
        }
    }

    void Update()
    {
        if (keyRebinder == null) return;

        // Check for custom key presses
        if (Input.GetKeyDown(keyRebinder.GetTroopKey(0))) // Troop1
        {
            ExperienceManager.Instance.Add(-10, 0);
            Instantiate(troop1, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(keyRebinder.GetTroopKey(1))) // Troop2
        {
            ExperienceManager.Instance.Add(-20, 0);
            Instantiate(troop2, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(keyRebinder.GetTroopKey(2))) // Troop3
        {
            ExperienceManager.Instance.Add(-50, 0);
            Instantiate(troop3, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(keyRebinder.GetTroopKey(3))) // Troop4
        {
            ExperienceManager.Instance.Add(-20, 0);
            Instantiate(troop4, new Vector3(
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y + 5,
                1
            ), Quaternion.identity);
        }
        if (Input.GetKeyDown(keyRebinder.GetTroopKey(4))) // Troop5
        {
            ExperienceManager.Instance.Add(-100, 0);
            Instantiate(troop5, new Vector3(
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y + 5,
                1
            ), Quaternion.identity);
        }
    }
}