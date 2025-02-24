using System.Collections.Generic;
using System.Linq;
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
    public GameObject defaultMonsterSelectControl;
    public TMP_Text battleLogText;
    public BattleManager battleManager;

    /// <summary>
    /// A map of <see cref="Character"/> to their corresponding character panels
    /// </summary>
    private Dictionary<Creature, GameObject> characterToPanelMap = new Dictionary<Creature, GameObject>();

    private Dictionary<Creature, MonsterSelectControl> monsterToActionControlMap = new Dictionary<Creature, MonsterSelectControl>();

    private List<GameObject> actionControls = new List<GameObject>();

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
        ActivateActionsPanel();
    }

    /// <summary>
    /// Exits the main menu and resumes the game
    /// </summary>
    public void CloseBattleUI()
    {
        CloseAllBattleUIElements();
    }

    public void LogBattleMessage(string message)
    {
        if (battleLogText.text == string.Empty)
        {
            battleLogText.text = message;
        }
        else
        {
            battleLogText.text += "\n" + message;
        }
    }

    public void ClearBattleLog()
    {
        battleLogText.text = string.Empty;
    }

    public void DeactivateActionsPanel()
    {
        actionsPanel.SetActive(false);
    }

    public void ActivateActionsPanel()
    {
        monsterSelectionPanel.SetActive(false);

        ClearActionsPanel();
        InstantiateActionControlsForCharacter(battleManager.GetActiveCharacter());
        // set currently selected game object to the first action control
        EventSystem.current.SetSelectedGameObject(actionControls.FirstOrDefault());

        actionsPanel.SetActive(true);
    }

    private void InstantiateActionControlsForCharacter(Creature character)
    {
        var characterActionTypes = character.GetClassData().actionTypes;
        if (characterActionTypes != null)
        {
            foreach (var actionType in characterActionTypes)
            {
                var actionControl = Instantiate(actionType.actionControlPrefab, actionsPanel.transform);
                actionControl.GetComponent<ActionButton>().actionData = actionType;
                actionControl.GetComponent<ActionButton>().SetActionName(actionType.actionName);
                actionControl.GetComponent<ActionButton>().SetupOnClick(() => battleManager.SetupMonsterSelectionPanel());
                actionControls.Add(actionControl);
            }
        }
    }

    public void ClearActionsPanel()
    {
        foreach (var actionControl in actionControls)
        {
            Destroy(actionControl);
        }

        actionControls.Clear();
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
    public void PopulatePartyPanel(List<Creature> party)
    {
        // clear the PartyPanel
        DeleteChildrenOfGameObject(partyPanel);
        characterToPanelMap.Clear();

        // add characters to party panel
        foreach (var character in party)
        {
            var characterPrefab = character.creatureUIPanelPrefab;
            var characterPanel = Instantiate(characterPrefab, partyPanel.transform);
            var characterInfo = characterPanel.GetComponent<CharacterPanel>();
            characterInfo.SetCharacterInfo(character.GetName(), character.GetHitPoints());

            characterToPanelMap.Add(character, characterPanel);
        }
    }

    public void PopulateMonsterSelectionPanel(List<Creature> monsters)
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
                var monsterUIComponent = monster.GetComponent<MonsterUI>();

                if (monsterSelectControlComponent != null && monsterUIComponent != null)
                {
                    Debug.Log("MonsterSelectControl component found on instantiated prefab.");
                    monsterSelectControlComponent.SetMonsterNameOnControl(monster.GetName());
                    monsterSelectControlComponent.SetOnClick(() => OnMonsterChosenForAttack(monster));
                    monsterSelectControlComponent.SetMonsterUI(monsterUIComponent);

                    monsterToActionControlMap.Add(monster, monsterSelectControlComponent);
                }
                else
                {
                    Debug.LogError("MonsterSelectControl or MonsterUI component not found on instantiated prefab.");
                }
            }
        }

        // set the first item in the control map as the selected monster
        ActivateMonsterSelectionPanel();
        EventSystem.current.SetSelectedGameObject(monsterToActionControlMap.FirstOrDefault().Value.gameObject);
    }

    private void OnMonsterChosenForAttack(Creature selectedMonster)
    {
        Debug.Log($"Monster {selectedMonster.GetName()} was selected!");
        // hide the selection indicator on the monster control
        monsterToActionControlMap[selectedMonster].GetMonsterUI().HideSelectionIndicator();

        battleManager.AddCurrentCharacterAttackToActionsQueue(selectedMonster);
        ActivateActionsPanel();
    }

    public void RemoveMonsterSelectionControlFromMonsterSelectionPanel(Creature monsterToRemove)
    {
        monsterToActionControlMap.Remove(monsterToRemove);
    }

    public void ShowCharacterAsSelectedInPartyPanel(Creature character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsSelected();
    }

    public void ShowCharacterAsDeselectedInPartyPanel(Creature character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsDeselected();
    }

    public void ShowCharacterAsDeadInPartyPanel(Creature character)
    {
        var characterPanel = characterToPanelMap[character];
        if (characterPanel == null)
        {
            Debug.Log("Character panel doesn't exist!");
        }

        characterPanel.GetComponent<CharacterPanel>().ShowCharacterPanelAsDead();
    }

    public void ShowCharacterAsTargetedInPartyPanel(Creature character)
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
    public void UpdatePartyPanel(List<Creature> party)
    {
        foreach (var character in party)
        {
            if (characterToPanelMap.ContainsKey(character))
            {
                GameObject characterPanel = characterToPanelMap[character];
                var characterInfo = characterPanel.GetComponent<CharacterPanel>();
                characterInfo.SetCharacterInfo(character.GetName(), character.GetHitPoints());
            }
            else
            {
                Debug.LogError($"Character {character.GetName()} not found in character info map.");
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
