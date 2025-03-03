using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages various game menus
/// </summary>
public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject firstSelectedButton;

    public MenuTypes menuType;
    public MenuTypes previousMenu;

    void Start()
    {
        menuCanvas.SetActive(false);
        MenuNotifier.OnMenuToggled += MenuToggle;
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void ExitMenus()
    {
        Debug.Log("Exiting menu");
        menuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.ResumeGame();
    }

    public void NavigateToPreviousMenu()
    {
        MenuNotifier.ToggleMenu(previousMenu);
    }

    private void OpenMenu()
    {
        menuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton); 
    }

    private void MenuToggle(MenuTypes menu)
    {
        if (menu == menuType)
        {
            Debug.Log($"Toggling {menu.ToString()} menu");
            if (menuCanvas.activeSelf == false)
            {
                GameManager.Instance.PauseGame();
                OpenMenu();
            }
            else
            {
                ExitMenus();
                GameManager.Instance.ResumeGame();
            }
        }
        else
        {
            // make sure this menu is not showing up
            menuCanvas.SetActive(false);
        }
    }
}
