using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{

    private float fps;
    private int ping;
    public TMPro.TextMeshProUGUI PerformanceText;
    private bool isVisible = false;
    private string pingAddress = "8.8.8.8";


    void Start()
    {
        PerformanceText.enabled = isVisible;
        InvokeRepeating("UpdateStats", 1, 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) 
        {
            isVisible = !isVisible;
            PerformanceText.enabled = isVisible;
        }
    }

    void UpdateStats()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        StartCoroutine(GetPing());
    }

    IEnumerator GetPing()
    {
        Ping pingTest = new Ping(pingAddress);
        yield return new WaitUntil(() => pingTest.isDone);
        ping = pingTest.time; 

        
        PerformanceText.text = $"FPS: {fps}  Ping: {ping} ms";
    }
}