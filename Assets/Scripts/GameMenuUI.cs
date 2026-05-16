using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUI : MonoBehaviour
{
    public GameObject menuUI;
    public void onMenuClick()
    {
        Debug.Log("Menu Clicked");
        menuUI.SetActive(true);
    }
    public void onRestartClick()
    {
        Debug.Log("Restart Clicked");
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
    }
}
