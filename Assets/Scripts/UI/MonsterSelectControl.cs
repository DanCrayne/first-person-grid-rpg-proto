using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterSelectControl : MonoBehaviour
{
    public TMP_Text monsterNameSlot;
    //private Button button;

    void Awake()
    {
        //button = GetComponent<Button>();
        //if (button == null)
        //{
        //    Debug.LogError("Button component not found on MonsterSelectControl GameObject.");
        //}
    }

    public void SetMonsterNameOnControl(string monsterName)
    {
        this.monsterNameSlot.text = monsterName;
    }

    public void SetOnClick(UnityAction action)
    {
        var button = this.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners(); // Clear any existing listeners
            button.onClick.AddListener(action); // Add the new listener
        }
    }

    public void OnClick()
    {
        Debug.Log("Monster control clicked");
    }
}
