using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the party of characters in the game
/// </summary>
public class PartyManager : MonoBehaviour
{
    /// <summary>
    /// The prefab for the <see cref="CharacterPanel"/> used for each character
    /// </summary>
    public GameObject encounterCharacterInfoPanelPrefab;

    /// <summary>
    /// The <see cref="Transform"/> for the party member display panel
    /// </summary>
    public Transform partyPanel;

    /// <summary>
    /// The <see cref="Transform"/> for the actions panel (e.g. the panel for attack, cast spell, etc. controls)
    /// </summary>
    public Transform actionsPanel;

    public GameObject defaultCharacterPrefab;
    public Transform partyGameObject;
    public CharacterData defaultCharacterData;
    public List<Character> partyMembers = new List<Character>();

    private List<GameObject> encounterCharacterInfoPanels = new List<GameObject>();

    private void Start()
    {
        CreateExampleParty();
    }

    public void CreateExampleParty()
    {
        // Create a couple of example characters - normally this would be done through a character creation screen
        // This is just for testing purposes
        var character = Instantiate(defaultCharacterPrefab, partyGameObject);
        var characterComponent = character.GetComponent<Character>();
        characterComponent.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent.SetCharacterName("Biff");
        partyMembers.Add(characterComponent);

        var character1 = Instantiate(defaultCharacterPrefab, partyGameObject);
        var characterComponent1 = character1.GetComponent<Character>();
        characterComponent1.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent1.SetCharacterName("Marty");
        partyMembers.Add(characterComponent1);
    }

    public Character GetRandomCharacter()
    {
        if (partyMembers.Count == 0)
        {
            Debug.LogError("No party members found");
            return null;
        }
        return partyMembers[Random.Range(0, partyMembers.Count)];
    }

    public bool IsPartyWiped()
    {
        return !partyMembers.Any(c => c.GetHitPoints() > 0);
    }

    public Character GetWeakestCharacter()
    {
        if (partyMembers.Count == 0)
        {
            Debug.LogError("No party members found");
            return null;
        }

        Character weakestCharacter = partyMembers.FirstOrDefault();
        foreach (var character in partyMembers)
        {
            if (character.GetHitPoints() < weakestCharacter.GetHitPoints())
            {
                weakestCharacter = character;
            }
        }
        return weakestCharacter;
    }

    public List<Character> GetPartyMembers()
    {
        return partyMembers;
    }

    public void RemoveCharacterFromParty(Character character)
    {
        partyMembers.Remove(character);
    }

    /// <summary>
    /// Instantiates the <see cref="CharacterPanel"/> prefab and adds it to the party panel UI
    /// </summary>
    /// <param name="parent">The <see cref="Transform"> to set as the parent of the instance</param>
    /// <param name="infoPanelPrefab">The prefab <see cref="GameObject"/> to instantiate</param>
    /// <returns>The instantiated <see cref="GameObject"/></returns>
    private GameObject InstantiateEncounterCharacterInfoPrefab(Transform parent, GameObject infoPanelPrefab)
    {
        return Instantiate(infoPanelPrefab, parent);
    }
}

