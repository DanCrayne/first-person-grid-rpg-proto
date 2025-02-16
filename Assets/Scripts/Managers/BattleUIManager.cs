using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUIManager : MonoBehaviour
{
    public GameObject battleUIRoot;
    public GameObject battleUICanvas;
    public GameObject partyPanel;
    public GameObject actionsPanel;
    public GameObject monsterSelectionPanel;
    public GameObject castButton;
    public GameObject defaultMonsterSelectControl;
    public TMP_Text battleLogText;
    public BattleManager battleManager;

    /// <summary>
    /// The first selected button in the Actions Panel
    /// </summary>
    public GameObject firstSelectedActionsButton;

    /// <summary>
    /// A map of <see cref="Character"/> to their corresponding character panels
    /// </summary>
    private Dictionary<Character, GameObject> characterToPanelMap = new Dictionary<Character, GameObject>();

    private Dictionary<Monster, MonsterSelectControl> monsterToActionControlMap = new Dictionary<Monster, MonsterSelectControl>();

    void Start()
    {
        battleUIRoot.SetActive(false);
    }

    /// <summary>
    /// Opens the main menu and sets the first selected button
    /// </summary>
    public void OpenBattleUI()
    {
        CloseAllBattleUIElements();
        battleUIRoot.SetActive(true);
        battleUICanvas.SetActive(true);
        ActivateActionsPanel(battleManager.GetActiveCharacter());
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void CloseBattleUI()
    {
        CloseAllBattleUIElements();
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

    public void DeactivateActionsPanel()
    {
        actionsPanel.SetActive(false);
    }

    public void ActivateActionsPanel(Character currentCharacter)
    {
        castButton.SetActive(false);
        switch (currentCharacter.characterData.characterClass.className)
        {
            case "Cleric":
            case "Mage":
                castButton.SetActive(true);
                break;
        }

        monsterSelectionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelectedActionsButton);
        actionsPanel.SetActive(true);
    }

    public void DeactivateMonsterSelectionPanel()
    {
        monsterSelectionPanel.SetActive(false);
    }

    public void ActivateMonsterSelectionPanel()
    {
        actionsPanel.SetActive(false);
        monsterSelectionPanel.SetActive(true);
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
            //characterPanels.Add(characterPanel);

            characterToPanelMap.Add(character, characterPanel);
        }
    }

    public void PopulateMonsterSelectionPanel(List<Monster> monsters)
    {
        // clear the monster panel
        DeleteChildrenOfGameObject(monsterSelectionPanel);
        monsterToActionControlMap.Clear();

        foreach (var monster in monsters)
        {
            if (monster != null)
            {
                var monsterSelectControlInstance = Instantiate(defaultMonsterSelectControl, monsterSelectionPanel.transform);
                var monsterSelectControlComponent = monsterSelectControlInstance.GetComponent<MonsterSelectControl>();
                monsterSelectControlComponent.SetMonsterNameOnControl(monster.monsterData.monsterName);
                monsterSelectControlComponent.SetOnClick(() => OnMonsterSelected(monster));
                EventSystem.current.SetSelectedGameObject(monsterSelectControlInstance);

                monsterToActionControlMap.Add(monster, monsterSelectControlComponent);
            }
        }

        ActivateMonsterSelectionPanel();
    }

    private void OnMonsterSelected(Monster monster)
    {
        Debug.Log($"Monster {monster.monsterData.monsterName} was selected!");
        
        battleManager.ExecuteCurrentCharacterAttack(monster);
        ActivateActionsPanel(battleManager.GetActiveCharacter());
    }

    public void RemoveMonsterSelectionControlFromMonsterSelectionPanel(Monster monsterToRemove)
    {
        monsterToActionControlMap.Remove(monsterToRemove);
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
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Deactivates all menus and clears the currently selected game object
    /// </summary>
    private void CloseAllBattleUIElements()
    {
        battleUICanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
