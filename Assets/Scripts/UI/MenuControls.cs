using UnityEngine;

public class MenuControls : MonoBehaviour
{
    [SerializeField] private CyberpunkUIAnimations cyberpunkPanel; 
    private bool isMenuOpen = false;
    public void OpenMenu()
    {
        if (isMenuOpen == true) {
            cyberpunkPanel.Close(); 
            isMenuOpen = false;
        }
        else
        {
            cyberpunkPanel.Open(); 
            isMenuOpen = true;
        }
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        cyberpunkPanel.Close(); // Reverse the animation!
    }
}