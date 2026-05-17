using UnityEngine;
using UnityEngine.UI;

public class TimeManager: MonoBehaviour
{
    [Header("UI Toggles")]
    [SerializeField] private Toggle pauseToggle;
    [SerializeField] private Toggle x1Toggle;
    [SerializeField] private Toggle x2Toggle;
    [SerializeField] private Toggle x4Toggle;

    void Start()
    {
        // Force the UI to start at x1 speed naturally
        x1Toggle.isOn = true;
        Time.timeScale = 1.0f;
    }

    public void onPause()
    {
        Time.timeScale = 0.0f;
        Debug.Log("Game Paused");
    }
    public void onX1()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Time Scale set to 1x");
    }
    public void onX2()
    {
        Time.timeScale = 2.0f;
        Debug.Log("Time Scale set to 2x");
    }
    public void onX4()
    {
        Time.timeScale = 4.0f;
        Debug.Log("Time Scale set to 4x");
    }

    private void OnToggleChanged()
    {
        if (pauseToggle.isOn)  Time.timeScale = 0.0f;
        else if (x1Toggle.isOn) Time.timeScale = 1.0f;
        else if (x2Toggle.isOn) Time.timeScale = 2.0f;
        else if (x4Toggle.isOn) Time.timeScale = 4.0f;
        
        Debug.Log($"Current Time Scale: {Time.timeScale}x");
    }

}       
