using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMenuManager : MonoBehaviour
{
    public GameObject rootMenuObject;
    public GameObject battleMenuCanvas;
    public GameObject partyPanel;
    public GameObject actionsPanel;
    public TMP_Text battleLogText;

    /// <summary>
    /// The first selected button in the Actions Panel
    /// </summary>
    public GameObject firstSelectedActionsButton;

    /// <summary>
    /// A list of character panels in the Party Panel
    /// </summary>
    public List<GameObject> characterPanels;

    /// <summary>
    /// A map of <see cref="Character"/> to their corresponding character panels
    /// </summary>
    private Dictionary<Character, GameObject> characterToPanelMap = new Dictionary<Character, GameObject>();

    void Start()
    {
        rootMenuObject.SetActive(false);
    }

    /// <summary>
    /// Opens the main menu and sets the first selected button
    /// </summary>
    public void OpenBattleMenu()
    {
        Debug.Log("Opening main menu");
        CloseAllBattleMenus();
        rootMenuObject.SetActive(true);
        battleMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSelectedActionsButton);
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void ExitBattleMenu()
    {
        Debug.Log("Exiting main menu");
        CloseAllBattleMenus();
    }

    public void LogBattleMessage(string message, bool append = false)
    {
        if (append)
        {
            battleLogText.text += message;
            return;
        }

        battleLogText.text = message;
    }

    /// <summary>
    /// Populates the Party Panel with the given list of characters
    /// </summary>
    /// <param name="party">A list of <see cref="Character"/> representing the party in the battle</param>
    public void PopulatePartyPanel(List<Character> party)
    {
        // clear the PartyPanel
        DeleteChildrenOfGameObject(partyPanel);
        characterToPanelMap.Clear();

        // add characters to party panel
        foreach (var character in party)
        {
            var characterPanel = Instantiate(character.characterData.encounterCharacterInfoPrefab, partyPanel.transform);
            var characterInfo = characterPanel.GetComponent<CharacterPanel>();
            characterInfo.SetCharacterInfo(character.GetCharacterName(), character.GetHitPoints());
            characterPanels.Add(characterPanel);

            characterToPanelMap.Add(character, characterPanel);
        }
    }

    public void ShowCharacterAsSelectedInPartyPanel(Character character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsSelected();
    }

    public void ShowCharacterAsDeselectedInPartyPanel(Character character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsDeselected();
    }

    public void ShowCharacterAsDeadInPartyPanel(Character character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsDead();
    }

    public void ShowCharacterAsTargetedInPartyPanel(Character character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsTargeted();
    }

    /// <summary>
    /// Updates the Party Panel with the current hit points of the characters in the party
    /// </summary>
    /// <param name="party">A list of <see cref="Character"/> representing the party in the battle</param>
    public void UpdatePartyPanel(List<Character> party)
    {
        foreach (var character in party)
        {
            if (characterToPanelMap.ContainsKey(character))
            {
                GameObject characterPanel = characterToPanelMap[character];
                var characterInfo = characterPanel.GetComponent<CharacterPanel>();
                characterInfo.SetCharacterInfo(character.GetCharacterName(), character.GetHitPoints());
            }
            else
            {
                Debug.LogError($"Character {character.GetCharacterName()} not found in character info map.");
            }
        }
    }

    /// <summary>
    /// Deletes all children of the given game object
    /// </summary>
    /// <param name="parent">The parent <see cref="GameObject"/> from which to delete children</param>
    private void DeleteChildrenOfGameObject(GameObject parent)
    {
        foreach (Transform child in partyPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Deactivates all menus and clears the currently selected game object
    /// </summary>
    private void CloseAllBattleMenus()
    {
        Debug.Log("Closing all menus");
        battleMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
