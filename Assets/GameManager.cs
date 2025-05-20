using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    // Public stats
    public int coinCount = 0;
    public int XP = 0;
    public int skill = 0;

    public float TimePlayed { get; private set; }
    public int EnemiesDefeated { get; private set; }
    
    public int TowersDestroyed { get; private set; }
    
    private HashSet<string> destroyedTowers = new();

    void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Hook into scene change to fix lost references
        SceneManager.sceneLoaded += OnSceneLoaded;

        ResetAllStats();
    }

    void Update()
    {

        if (!PauseManager.isPaused)
        {
            TimePlayed += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DisableDestroyedTowers();
    }

    public void ResetAllStats()
    {
        TimePlayed = 0f;
        XP = 0;
        EnemiesDefeated = 0;
        TowersDestroyed = 0;
        coinCount = 0;
        skill = 0;
        destroyedTowers.Clear();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
    }

    public void AddSkill(int amount)
    {
        skill += amount;
    }
    
    public void AddXP(int amount)
    {
        XP += amount;
    }
    
    private void DisableDestroyedTowers()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("EnemyTurret"))
        {
            if (destroyedTowers.Contains(obj.name))
            {
                obj.SetActive(false);
            }
        }
    }


    public virtual void AddEnemyDefeated() => EnemiesDefeated++;
    //public void AddTowerDestroyed() => TowersDestroyed++;
    
    public void AddTowerDestroyed(GameObject tower)
    {
        TowersDestroyed++;
        destroyedTowers.Add(tower.name);
    }
    
    public void ResetXP()
    {
        XP = 0;
    }
}

