using UnityEngine;
using UnityEngine.UI;

public class DefenderSpawner : MonoBehaviour
{
    public GameObject troop1;
    public GameObject troop2;
    public GameObject troop3;
    public GameObject troop4;
    public GameObject troop5;
    public Transform spawnPoint;

    public Button spawnTroop1Button;
    public Button spawnTroop2Button;
    public Button spawnTroop3Button;
    public Button spawnTroop4Button;
    public Button spawnTroop5Button;

    void Update()
    {
        // Check for key presses and invoke corresponding button click
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnTroop1Button.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnTroop2Button.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnTroop3Button.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spawnTroop4Button.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            spawnTroop5Button.onClick.Invoke();
        }
    }

    public void SpawnTroop1()
    {
        Instantiate(troop1, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnTroop2()
    {
        Instantiate(troop2, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnTroop3()
    {
        Instantiate(troop3, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnTroop4()
    {
        Instantiate(troop4, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnTroop5()
    {
        Instantiate(troop5, spawnPoint.position, Quaternion.identity);
    }
}
