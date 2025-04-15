using TMPro;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] Transform Spawn;


    private void OnEnable()
    {
        BossEvent.BossSpawn += SpawnBoss;
    }
    private void OnDisable()
    {
        BossEvent.BossSpawn -= SpawnBoss;    
    }

    void SpawnBoss()
    {
        Instantiate(Boss,Spawn.position,Quaternion.identity);
    }

}
