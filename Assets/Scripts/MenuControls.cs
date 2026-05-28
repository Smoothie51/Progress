using UnityEngine;

public class MenuControls : MonoBehaviour
{
    // Replace your old "GameObject menuUI" with this:
    [SerializeField] private CyberpunkUIAnimations cyberpunkPanel; 
    private bool isMenuOpen = false;

    void Update()
    {
        // Example: Press Escape or Tab to toggle the menu
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     if (isMenuOpen)
        //     {
        //         CloseMenu();
        //     }
        //     else
        //     {
        //         OpenMenu();
        //     }
        // }
    }

    public void OpenMenu()
    {
        isMenuOpen = true;
        cyberpunkPanel.Open(); // Trigger the cool animation!
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        cyberpunkPanel.Close(); // Reverse the animation!
    }
}