using UnityEngine;
using TMPro;

public class Logger : MonoBehaviour
{
    public TextMeshProUGUI logText;      // Kur rašomas tekstas
    public GameObject logPanel;          // UI panel su log'u

    private string fullLog = "";

    void Start()
    {
   
        logPanel.SetActive(false);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            logPanel.SetActive(!logPanel.activeSelf);
            Log($"Log langas {(logPanel.activeSelf ? "atidarytas" : "uždarytas")} (F2)");
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = type switch
        {
            LogType.Warning => "yellow",
            LogType.Error => "red",
            LogType.Exception => "orange",
            _ => "white"
        };

        string formatted = $"<color={color}>[{type}] {logString}</color>\n";
        fullLog += formatted;
        logText.text = fullLog;
    }

    public void Log(string message)
    {
        HandleLog(message, "", LogType.Log);
    }
}