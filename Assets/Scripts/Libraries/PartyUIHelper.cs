using System.Collections.Generic;
using UnityEngine;

public class PartyUIHelper : MonoBehaviour
{
    public GameObject partyPanel;
    public GameObject partyPanelPrefab;
    public PartyManager playerPartyManager;

    private Dictionary<Creature, GameObject> _characterToPanelMap = new Dictionary<Creature, GameObject>();
    private bool isPartyPanelPopulated = false;

    private void OnEnable()
    {
        playerPartyManager.OnPartyLoaded += PopulatePartyPanelUsingPartyManagerData;
        UiNotifier.OnCreatureSelected += ShowCharacterAsSelectedInPartyPanel;
        UiNotifier.OnCreatureDeselected += ShowCharacterAsDeselectedInPartyPanel;
        EncounterEventNotifier.OnEncounterEnd += ShowAllCharactersAsDeselected;
    }

    private void OnDisable()
    {
        playerPartyManager.OnPartyLoaded -= PopulatePartyPanelUsingPartyManagerData;
        UiNotifier.OnCreatureSelected -= ShowCharacterAsSelectedInPartyPanel;
        UiNotifier.OnCreatureDeselected -= ShowCharacterAsDeselectedInPartyPanel;
        EncounterEventNotifier.OnEncounterEnd -= ShowAllCharactersAsDeselected;
    }

    void Update()
    {
        var partyMembers = playerPartyManager.GetPartyMembers();

        foreach (var partyMember in partyMembers)
        {
            UpdateCharacterPanelWithCurrentStats(partyMember);
        }
    }

    public void UpdateCharacterPanelWithCurrentStats(Creature creature)
    {
        if (!isPartyPanelPopulated)
        {
            PopulatePartyPanelUsingPartyManagerData();
            return;
        }

        if (creature.IsDead())
        {
            ShowCharacterAsDeadInPartyPanel(creature);
        }
        else
        {
            if (_characterToPanelMap.TryGetValue(creature, out var characterPanel))
            {
                characterPanel.GetComponent<CharacterPanel>().SetCharacterInfo(creature.GetName(), creature.GetHitPoints());
            }
            else
            {
                Debug.LogError($"Could not find character panel for {creature.GetName()}");
            }
        }
    }

    /// <summary>
    /// Populates the Party Panel with the given list of characters
    /// </summary>
    /// <param name="party">A list of <see cref="Character"/> representing the party in the battle</param>
    private void PopulatePartyPanelUsingPartyManagerData()
    {
        // clear the PartyPanel
        GameObjectHelper.DeleteChildrenOfGameObject(partyPanel);
        _characterToPanelMap.Clear();

        var partyMembers = playerPartyManager.GetPartyMembers();

        // add characters to party panel
        foreach (var character in partyMembers)
        {
            var characterPrefab = character.creatureUIPanelPrefab;
            var characterPanel = Instantiate(characterPrefab, partyPanel.transform);
            var characterInfo = characterPanel.GetComponent<CharacterPanel>();
            characterInfo.SetCharacterInfo(character.GetName(), character.GetHitPoints());
            ShowCharacterStatusInCharacterPanel(character, characterPanel.GetComponent<CharacterPanel>());

            _characterToPanelMap.Add(character, characterPanel);
        }

        isPartyPanelPopulated = true;
        Debug.Log("Done populating party panels");
    }

    public void ShowCharacterStatusInCharacterPanel(Creature character, CharacterPanel panel)
    {
        if (character.IsDead())
        {
            panel.ShowCharacterPanelAsDead();
        }
    }

    public void ShowCharacterAsSelectedInPartyPanel(Creature character)
    {
        TryToGetCharacterPanelAndHandleFailure(character).ShowCharacterPanelAsSelected();
    }

    public void ShowCharacterAsDeselectedInPartyPanel(Creature character)
    {
        TryToGetCharacterPanelAndHandleFailure(character).ShowCharacterPanelAsDeselected();
    }

    public void ShowAllCharactersAsDeselected()
    {
        var partyMembers = playerPartyManager.GetPartyMembers();

        foreach (var character in partyMembers)
        {
            ShowCharacterAsDeselectedInPartyPanel(character);
        }
    }

    public void ShowCharacterAsDeadInPartyPanel(Creature character)
    {
        TryToGetCharacterPanelAndHandleFailure(character).ShowCharacterPanelAsDead();
    }

    public void ShowCharacterAsTargetedInPartyPanel(Creature character)
    {
        TryToGetCharacterPanelAndHandleFailure(character).ShowCharacterPanelAsTargeted();
    }

    private CharacterPanel TryToGetCharacterPanelAndHandleFailure(Creature character)
    {
        if (_characterToPanelMap.TryGetValue(character, out var characterPanelGameObject))
        {

            var characterPanel = characterPanelGameObject.GetComponent<CharacterPanel>();
            if (characterPanel != null)
            {
                return characterPanel;
            }
        }

        Debug.LogError($"Could not find character panel for {character.GetName()}");
        return null;
    }
}
