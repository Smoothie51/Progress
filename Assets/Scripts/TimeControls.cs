using UnityEngine;
using UnityEngine.UI;

public class TimeManager: MonoBehaviour
{
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
}       
