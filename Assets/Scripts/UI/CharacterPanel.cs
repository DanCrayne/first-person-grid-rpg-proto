using TMPro;
using UnityEngine;

/// <summary>
/// Represents the UI for a character in an encounter
/// </summary>
public class CharacterPanel : MonoBehaviour
{
    public TMP_Text characterNameSlot;
    public TMP_Text characterHpSlot;
    public static readonly string SELECTED_PANEL_COLOR = "#59EE2C";
    public static readonly string DEFAULT_PANEL_COLOR = "#FFFFFF";
    public static readonly string TARGETED_PANEL_COLOR = "#FF0000";
    public static readonly string DEAD_PANEL_COLOR = "#000000";

    public GameObject _characterPanel;

    /// <summary>
    /// Creates an <see cref="CharacterPanel"/> panel for the character representing their current status during battle
    /// </summary>
    /// <param name="parent">The <see cref="Transform"/> to add the created panel to</param>
    /// <param name="prefab">The prefab to use for the panel</param>
    /// <returns></returns>
    public static CharacterPanel Create(Transform parent, GameObject prefab)
    {
        var encounterCharacterInfoGameObject = Instantiate(prefab, parent);
        return encounterCharacterInfoGameObject.GetComponent<CharacterPanel>();
    }

    public void SetCharacterInfo(string name, int hp)
    {
        characterNameSlot.text = name;
        characterHpSlot.text = $"HP: {hp}";
    }

    /// <summary>
    /// Highlights the character's panel so that it's clear that the character is having their turn
    /// </summary>
    public void ShowCharacterPanelAsSelected()
    {
        TrySettingCharacterPanelToColor(SELECTED_PANEL_COLOR);
    }

    /// <summary>
    /// Sets the character panel back to it's default look
    /// </summary>
    public void ShowCharacterPanelAsDeselected()
    {
        TrySettingCharacterPanelToColor(DEFAULT_PANEL_COLOR);
    }

    public void ShowCharacterPanelAsDead()
    {
        characterHpSlot.text = "DEAD";
        TrySettingCharacterPanelToColor(DEAD_PANEL_COLOR);
    }

    public void ShowCharacterPanelAsTargeted()
    {
        TrySettingCharacterPanelToColor(TARGETED_PANEL_COLOR);
    }

    private void TrySettingCharacterPanelToColor(string color)
    {
        var imageComponent = _characterPanel.GetComponent<UnityEngine.UI.Image>();
        if (imageComponent != null)
        {
            imageComponent.color = ColorUtility.TryParseHtmlString(color, out Color parsedColor) ? parsedColor : Color.white;
        }
        else
        {
            Debug.LogError("No Image component found on characterPanel");
        }
    }
}
