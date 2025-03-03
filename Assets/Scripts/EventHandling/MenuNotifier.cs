using System;
using UnityEngine;

public class MenuNotifier : MonoBehaviour
{
    public static event Action<MenuTypes> OnMenuToggled;
    public static event Action<MenuTypes> OnNavigateBackToMenu;

    public static void ToggleMenu(MenuTypes menuType)
    {
        OnMenuToggled?.Invoke(menuType);
    }

    public static void NavigateBackToMenu(MenuTypes backToMenu)
    {
        OnNavigateBackToMenu?.Invoke(backToMenu);
    }
}
