using UnityEngine;

public class MonsterUI : MonoBehaviour
{
    public Transform selectionIndicator;
    public MonsterSelectControl selectionControl;


    private void Start()
    {
        HideSelectionIndicator();

        if (selectionControl != null)
        {
            selectionControl.SetOnFocus(OnMonsterFocus);
            selectionControl.SetOnDefocus(OnMonsterDefocus);
        }
    }

    public void OnMonsterFocus()
    {
        ShowSelectionIndicator();
    }

    public void OnMonsterDefocus()
    {
        HideSelectionIndicator();
    }

    private void ShowSelectionIndicator()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.gameObject.SetActive(true);
        }
    }

    public void HideSelectionIndicator()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.gameObject.SetActive(false);
        }
    }
}
