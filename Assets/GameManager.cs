using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Public stats
    public int coinCount = 0;
    public Text coinText; // This gets destroyed when scene reloads!

    public float TimePlayed { get; private set; }
    public int EnemiesDefeated { get; private set; }
    public int TowersDestroyed { get; private set; }

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
        if (coinText == null) FindCoinText(); // Auto-fix missing reference

        if (!PauseManager.isPaused)
        {
            TimePlayed += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCoinText();
        UpdateCoinUI(); // Ensures UI updates properly
    }


    private void FindCoinText()
    {
        if (coinText == null)
        {
            GameObject coinTextObject = GameObject.Find("CoinText"); // Make sure the UI element is named correctly
            if (coinTextObject != null)
                coinText = coinTextObject.GetComponent<Text>();
        }
    }

    public void ResetAllStats()
    {
        TimePlayed = 0f;
        EnemiesDefeated = 0;
        TowersDestroyed = 0;
        coinCount = 0;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Money: " + coinCount;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    public void AddEnemyDefeated() => EnemiesDefeated++;
    public void AddTowerDestroyed() => TowersDestroyed++;
}
