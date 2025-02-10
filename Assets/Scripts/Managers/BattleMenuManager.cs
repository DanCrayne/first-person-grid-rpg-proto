using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMenuManager : MonoBehaviour
{
    public GameObject rootMenuObject;
    public GameObject battleMenuCanvas;

    public GameObject battleMenuFirstSelectedButton;

    void Start()
    {
        rootMenuObject.SetActive(false);
    }

    /// <summary>
    /// Opens the main menu and sets the first selected button
    /// </summary>
    public void OpenMainMenu()
    {
        Debug.Log("Opening main menu");
        CloseAllMenus();
        rootMenuObject.SetActive(true);
        battleMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(battleMenuFirstSelectedButton);
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void ExitMainMenu()
    {
        Debug.Log("Exiting main menu");
        CloseAllMenus();
    }

    /// <summary>
    /// Deactivates all menus and clears the currently selected game object
    /// </summary>
    private void CloseAllMenus()
    {
        Debug.Log("Closing all menus");
        battleMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
