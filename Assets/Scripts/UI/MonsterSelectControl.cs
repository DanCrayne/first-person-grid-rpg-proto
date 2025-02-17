using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonsterSelectControl : MonoBehaviour
{
    public TMP_Text monsterNameSlot;
    private Button button;
    private MonsterUI monsterUI;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on MonsterSelectControl GameObject.");
        }

        // Add EventTrigger component if not already present
        var eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        // Set up focus event (e.g., gamepad navigation or keyboard tab)
        var focusEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select // This can be used for gamepad navigation or keyboard tab
        };
        focusEntry.callback.AddListener((data) => { OnFocus(); });
        eventTrigger.triggers.Add(focusEntry);

        // Set up defocus event (e.g., gamepad navigation away or keyboard shift+tab)
        var defocusEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Deselect // This can be used for gamepad navigation away or keyboard shift+tab
        };
        defocusEntry.callback.AddListener((data) => { OnDefocus(); });
        eventTrigger.triggers.Add(defocusEntry);
    }

    public void SetMonsterUI(MonsterUI ui)
    {
        monsterUI = ui;
    }

    public void SetMonsterNameOnControl(string monsterName)
    {
        this.monsterNameSlot.text = monsterName;
    }

    public void SetOnClick(UnityAction action)
    {
        button = gameObject.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners(); // Clear any existing listeners
            button.onClick.AddListener(action); // Add the new listener
        }
    }

    public void SetOnFocus(UnityAction action)
    {
        var eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            var focusEntry = eventTrigger.triggers.Find(entry => entry.eventID == EventTriggerType.Select);
            if (focusEntry != null)
            {
                focusEntry.callback.RemoveAllListeners();
                focusEntry.callback.AddListener((data) => action.Invoke());
            }
        }
    }

    public void SetOnDefocus(UnityAction action)
    {
        var eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            var defocusEntry = eventTrigger.triggers.Find(entry => entry.eventID == EventTriggerType.Deselect);
            if (defocusEntry != null)
            {
                defocusEntry.callback.RemoveAllListeners();
                defocusEntry.callback.AddListener((data) => action.Invoke());
            }
        }
    }

    public void OnSelect()
    {
        Debug.Log("Monster control selected");
        // Add your select logic here
    }

    public void OnFocus()
    {
        Debug.Log("Monster control focused");
        if (monsterUI != null)
        {
            monsterUI.OnMonsterFocus();
        }
    }

    public void OnDefocus()
    {
        Debug.Log("Monster control defocused");
        if (monsterUI != null)
        {
            monsterUI.OnMonsterDefocus();
        }
    }
}
