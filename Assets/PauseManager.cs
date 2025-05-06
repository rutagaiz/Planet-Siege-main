using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject spawnCanvas;
    public MonoBehaviour[] playerScriptsToDisable;

    public static bool isPaused { get; private set; }

    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); //  Make sure it's hidden at start
        }

        if (spawnCanvas != null)
        {
            spawnCanvas.SetActive(true); //  Ensure gameplay UI is visible
        }

        EnsureUIState();
        SoundManager.Instance.PlayLevel1Music();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused == false)
            TogglePause();
            else
            ResumeGame();
        }
    }

    void EnsureUIState()
    {
        if (pauseMenu) pauseMenu.SetActive(isPaused);
        if (spawnCanvas) spawnCanvas.SetActive(!isPaused);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
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
        SoundManager.Instance.StopBackgroundMusic();
        SoundManager.Instance.PlayMenuMusic();
    }



    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause(); // Unpauses the game
            SoundManager.Instance.StopMenuMusic();
            SoundManager.Instance.PlayLevel1Music();
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
        SoundManager.Instance.PlayMenuMusic();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}