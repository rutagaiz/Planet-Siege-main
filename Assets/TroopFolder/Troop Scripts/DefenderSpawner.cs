using UnityEngine;
using UnityEngine.UI;

public class DefenderSpawner : MonoBehaviour
{
    public GameObject troop1;
    public GameObject troop2;
    public GameObject troop3;
    public GameObject troop4;
    public GameObject troop5;
    public GameObject spawnPoint;

   

    void Update()
    {
        // Check for key presses and invoke corresponding butt1on click
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(troop1, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(troop2, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(troop3, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Instantiate(troop4, new Vector3(spawnPoint.transform.position.x,spawnPoint.transform.position.y + 5,1), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Instantiate(troop5, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 5, 1), Quaternion.identity);
        }
    }
}
