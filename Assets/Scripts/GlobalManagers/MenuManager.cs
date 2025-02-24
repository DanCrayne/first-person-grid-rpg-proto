using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages various game menus
/// </summary>
public class MenuManager : MonoBehaviour
{
    public GameObject rootMenuObject;
    public GameObject mainMenuCanvas;
    public GameObject settingsMenuCanvas;

    public GameObject mainMenuFirstSelectedButton;
    public GameObject settingsMenuFirstSelectedButton;

    void Start()
    {
        rootMenuObject.SetActive(false);
        InputManager.Instance.OnToggleMainMenu += MainMenuToggle;
    }

    /// <summary>
    /// Opens the main menu and sets the first selected button
    /// </summary>
    public void OpenMainMenu()
    {
        Debug.Log("Opening main menu");
        CloseAllMenus();
        rootMenuObject.SetActive(true);
        mainMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelectedButton);
    }

    /// <summary>
    /// Opens the settings menu and sets the first selected button
    /// </summary>
    public void OpenSettingsMenu()
    {
        Debug.Log("Opening settings menu");
        settingsMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelectedButton);
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void ExitMainMenu()
    {
        Debug.Log("Exiting main menu");
        CloseAllMenus();
        GameManager.Instance.ResumeGame();
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting application");
        Application.Quit();
    }

    /// <summary>
    /// Toggles the main menu on and off and pauses / unpauses the game as appropriate
    /// </summary>
    private void MainMenuToggle()
    {
        Debug.Log("Toggling main menu");
        if (rootMenuObject.activeSelf == false || mainMenuCanvas.activeSelf == false)
        {
            GameManager.Instance.PauseGame();
            OpenMainMenu();
        }
        else
        {
            ExitMainMenu();
            GameManager.Instance.ResumeGame();
        }
    }

    /// <summary>
    /// Deactivates all menus and clears the currently selected game object
    /// </summary>
    private void CloseAllMenus()
    {
        Debug.Log("Closing all menus");
        mainMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
