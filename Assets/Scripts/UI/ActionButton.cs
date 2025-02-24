using System;
using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public TMP_Text actionNameUIText;
    public ActionData actionData;

    private void Start()
    {
        SetActionName(actionData.actionName);
    }

    public void SetActionName(string name)
    {
        actionNameUIText.text = name;
    }

    public void SetActionData(ActionData data)
    {
        actionData = data;
    }

    public void SetupOnClick(Action onClickCallback)
    {
        var button = GetComponent<UnityEngine.UI.Button>();

        if (button != null)
        {
            button.onClick.AddListener(() => onClickCallback());
        }
    }
}
