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
            ExperienceManager.Instance.Add(-10, 0);
            Instantiate(troop1, spawnPoint.transform.position, Quaternion.identity);
        }   
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ExperienceManager.Instance.Add(-20, 0);
            Instantiate(troop2, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ExperienceManager.Instance.Add(-50, 0);
            Instantiate(troop3, spawnPoint.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ExperienceManager.Instance.Add(-20, 0);
            Instantiate(troop4, new Vector3(spawnPoint.transform.position.x,spawnPoint.transform.position.y + 5,1), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ExperienceManager.Instance.Add(-100, 0);
            Instantiate(troop5, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 5, 1), Quaternion.identity);
        }
    }
}
