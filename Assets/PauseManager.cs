using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject spawnCanvas;
    public MonoBehaviour[] playerScriptsToDisable;

    [SerializeField] private GameObject settingsPanel; 
    private KeyRebinder keyRebinder;

    public static bool isPaused { get; private set; }

    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Initialize UI elements
        if (pauseMenu) pauseMenu.SetActive(false);
        if (spawnCanvas) spawnCanvas.SetActive(true);

        // Initialize SettingsPanel and its KeyRebinder
        if (settingsPanel)
        {
            settingsPanel.SetActive(false);
            var rebinder = settingsPanel.GetComponent<KeyRebinder>();
            if (rebinder) rebinder.Initialize();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void EnsureUIState()
    {
        if (pauseMenu) pauseMenu.SetActive(isPaused);
        if (spawnCanvas) spawnCanvas.SetActive(!isPaused);
    }

    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            bool showSettings = !settingsPanel.activeSelf;
            settingsPanel.SetActive(showSettings);
            pauseMenu.SetActive(!showSettings); // Hide main pause menu when settings are open
        }
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked!"); // Check if this appears in Console
        if (settingsPanel != null)
        {
            Debug.Log("SettingsPanel found: " + settingsPanel.name);
            settingsPanel.SetActive(true);
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("SettingsPanel reference is null!");
        }
    }

    // Called by the "Return" button in settings
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;


        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }

        // Force UI update immediately
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }

        if (spawnCanvas != null)
        {
            spawnCanvas.SetActive(!isPaused);
        }

        //  Ensure scripts are enabled/disabled correctly
        foreach (var script in playerScriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = !isPaused;
            }
        }

        // Force stats update when paused
        if (isPaused && pauseMenu != null)
        {
            var stats = pauseMenu.GetComponent<PauseMenuStats>();
            if (stats != null)
            {
                stats.UpdateStats();
            }
        }
    }



    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause(); // Unpauses the game
        }
    }

    public void RestartLevel()
    {
        // Reset state before reload
        isPaused = false;
        Time.timeScale = 1f;

        // Clear any potential duplicate GameManager
        var gameManagers = FindObjectsOfType<GameManager>();
        foreach (var gm in gameManagers)
        {
            if (gm != GameManager.Instance)
            {
                Destroy(gm.gameObject);
            }
        }

        GameManager.Instance.ResetAllStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}