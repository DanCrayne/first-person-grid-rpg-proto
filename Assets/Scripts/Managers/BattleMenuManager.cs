using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class BattleMenuManager : MonoBehaviour
{
    public GameObject rootMenuObject;
    public GameObject battleMenuCanvas;
    public GameObject partyPanel;
    public GameObject actionsPanel;

    public GameObject firstSelectedActionsButton;

    public List<GameObject> characterPanels;

    private Dictionary<Character, GameObject> _characterInfoMap = new Dictionary<Character, GameObject>();

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

    public GameObject GetPartyPanel()
    {
        return partyPanel;
    }

    public GameObject GetActionsPanel()
    {
        return actionsPanel;
    }

    public void PopulatePartyPanel(List<Character> party)
    {
        // clear the PartyPanel
        DeleteChildrenOfGameObject(partyPanel);
        _characterInfoMap.Clear();

        // add characters to party panel
        foreach (var character in party)
        {
            var characterPanel = Instantiate(character.characterData.encounterCharacterInfoPrefab, partyPanel.transform);
            var characterInfo = characterPanel.GetComponent<EncounterCharacterInfo>();
            characterInfo.SetCharacterInfo(character.GetCharacterName(), character.GetHitPoints());
            characterPanels.Add(characterPanel);

            _characterInfoMap.Add(character, characterPanel);
        }
    }

    public void UpdatePartyPanel(List<Character> party)
    {
        foreach (var character in party)
        {
            if (_characterInfoMap.ContainsKey(character))
            {
                GameObject characterPanel = _characterInfoMap[character];
                var characterInfo = characterPanel.GetComponent<EncounterCharacterInfo>();
                characterInfo.SetCharacterInfo(character.GetCharacterName(), character.GetHitPoints());
            }
            else
            {
                Debug.LogError($"Character {character.GetCharacterName()} not found in character info map.");
            }
        }
    }

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
