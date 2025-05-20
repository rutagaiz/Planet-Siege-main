using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject[] mainMenuTexts;
    [SerializeField] private GameObject[] mainMenuButtons;

    public void StartGame()
    {
        Debug.Log("StartGame called!");
        // Play the level 1 music
        SoundManager.Instance.StopMenuMusic();
        SoundManager.Instance.PlayLevel1Music();
        SceneManager.LoadScene("atsarginis");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked!");
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            SetMainMenuElementsActive(false); 
        }
        else
        {
            Debug.LogError("SettingsPanel reference is null!");
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        SetMainMenuElementsActive(true); 
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetMainMenuElementsActive(bool isActive)
    {
        foreach (GameObject text in mainMenuTexts)
        {
            if (text != null) text.SetActive(isActive);
        }

        foreach (GameObject button in mainMenuButtons)
        {
            if (button != null) button.SetActive(isActive);
        }
    }
}
