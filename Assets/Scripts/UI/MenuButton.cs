using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public MenuTypes menuToOpen;

    public void ActivateConfiguredMenu()
    {
        MenuNotifier.ToggleMenu(menuToOpen);
    }
}
