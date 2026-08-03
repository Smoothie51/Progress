using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string mainGameSceneName = "SampleScene"; 

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(mainGameSceneName); 
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit(); 
    }
}