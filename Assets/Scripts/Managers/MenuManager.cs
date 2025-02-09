using UnityEngine;

/// <summary>
/// Manages various game menus
/// </summary>
public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;

    void Start()
    {
        MainMenu.SetActive(false);
        InputManager.Instance.OnToggleMainMenu += MainMenuToggle;
    }

    /// <summary>
    /// Toggles the main menu on and off and pauses / unpauses the game as appropriate
    /// </summary>
    private void MainMenuToggle()
    {
        if (MainMenu.activeSelf == false)
        {
            GameManager.Instance.PauseGame();
            MainMenu.SetActive(true);
        }
        else
        {
            GameManager.Instance.ResumeGame();
            MainMenu.SetActive(false);
        }
    }
}
