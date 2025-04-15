using Codice.Client.BaseCommands;
using PlasticGui.WorkspaceWindow.Locks;
using TMPro;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    private float defaultRemaining;

    public delegate void EventHappen();
    public static event EventHappen BossSpawn;

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = defaultRemaining;
            BossSpawn?.Invoke();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
    private void Awake()
    {
         defaultRemaining = remainingTime;
    }

}
