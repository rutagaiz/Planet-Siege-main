using System;
using System.Threading;
using UnityEngine;

public class AllySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Troop1Prefab, Troop2Prefab, Troop3Prefab, Troop4Prefab, Troop5Prefab;

    [SerializeField]
    GameObject TroopSpawnPoint;

    public static AllySpawner Instance;

    private float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        timer = 0;

    }
    private void Update()
    {
        //if (timer <= 0)
        //  {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ExperienceManager.Instance.Add(-20, 0);
                GameObject Troop = Instantiate(Troop1Prefab, TroopSpawnPoint.transform.position, Quaternion.identity);
                timer += 200;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {

            }
      //  }
       // timer--;
    }


}
