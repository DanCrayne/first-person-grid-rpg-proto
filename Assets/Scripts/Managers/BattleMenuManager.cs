using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMenuManager : MonoBehaviour
{
    public GameObject rootMenuObject;
    public GameObject battleMenuCanvas;
    public GameObject partyPanel;
    public GameObject actionsPanel;

    /// <summary>
    /// The first selected button in the Actions Panel
    /// </summary>
    public GameObject firstSelectedActionsButton;

    /// <summary>
    /// A list of character panels in the Party Panel
    /// </summary>
    public List<GameObject> characterPanels;

    /// <summary>
    /// A map of <see cref="Character"/> to their corresponding <see cref="GameObject"/> in the Party Panel
    /// </summary>
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

    /// <summary>
    /// Returns the Party Panel game object
    /// </summary>
    /// <returns>The Party Panel <see cref="GameObject"/></returns>
    /// <remarks>The Party Panel is a Unity UI container where character info will be displayed</remarks>
    public GameObject GetPartyPanel()
    {
        return partyPanel;
    }

    /// <summary>
    /// Returns the Actions Panel game object
    /// </summary>
    /// <returns>The Actions Panel <see cref="GameObject"/></returns>
    /// <remarks>The Actions Panel is a Unity UI container where the list of character actions will be displayed</remarks>
    public GameObject GetActionsPanel()
    {
        return actionsPanel;
    }

    /// <summary>
    /// Populates the Party Panel with the given list of characters
    /// </summary>
    /// <param name="party">A list of <see cref="Character"/> representing the party in the battle</param>
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

    /// <summary>
    /// Updates the Party Panel with the current hit points of the characters in the party
    /// </summary>
    /// <param name="party">A list of <see cref="Character"/> representing the party in the battle</param>
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
