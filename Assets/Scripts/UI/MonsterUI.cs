using UnityEngine;

public class MonsterUI : MonoBehaviour
{
    public GameObject selectionIndicator;
    public MonsterSelectControl selectionControl;

    //public void SetSelectionControlName(string monsterName)
    //{
    //    selectionControl.GetComponent<MonsterSelectControl>().monsterName = name;
    //}

    public void ShowMonsterAsSelected()
    {
        selectionIndicator.SetActive(true);
    }

    public void ShowMonsterAsDeselected()
    {
        selectionIndicator.SetActive(false);
    }
}
