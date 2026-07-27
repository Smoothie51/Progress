using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUI : MonoBehaviour
{
    public GameObject menuUI;

    private float previousTimeScale = 1f;
    public void onMenuClick()
    {
        Debug.Log("Menu Clicked");
        menuUI.SetActive(true);
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f; // Freeze the game
    }
    public void onRestartClick()
    {
        Debug.Log("restart Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void onExitClick()
    {
        Debug.Log("Exit Clicked");
        Application.Quit();
    }
    public void onResumeClick()
    {
        Debug.Log("Resume Clicked");
        menuUI.SetActive(false);
        Time.timeScale = previousTimeScale; // Resume the game
    }
}
