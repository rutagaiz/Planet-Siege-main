using TMPro;
using UnityEngine;

public class PauseMenuStats : MonoBehaviour
{
    public TMP_Text timePlayedText;
    public TMP_Text enemiesDefeatedText;
    public TMP_Text towersDestroyedText;

    void OnEnable() => UpdateStats();

    public void UpdateStats()
    {
        if (!GameManager.Instance) return;

        int minutes = Mathf.FloorToInt(GameManager.Instance.TimePlayed / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.TimePlayed % 60);

        timePlayedText.text = $"Time: {minutes:00}:{seconds:00}";
        enemiesDefeatedText.text = $"Enemies: {GameManager.Instance.EnemiesDefeated}";
        towersDestroyedText.text = $"Towers: {GameManager.Instance.TowersDestroyed}";
    }
}