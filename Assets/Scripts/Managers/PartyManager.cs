using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the party of characters in the game
/// </summary>
public class PartyManager : MonoBehaviour
{
    // TODO: Delete - for example party only
    public GameObject fighterCharacterPrefab;
    public GameObject mageCharacterPrefab;
    public CharacterData defaultCharacterData;

    public Transform partyGameObject;
    public List<ICreature> partyMembers = new List<ICreature>();

    private void Start()
    {
        CreateExampleParty();
    }

    public void CreateExampleParty()
    {
        // Create a couple of example characters - normally this would be done through a character creation screen
        // This is just for testing purposes
        var character = Instantiate(fighterCharacterPrefab, partyGameObject);
        var characterComponent = character.GetComponent<Character>();
        characterComponent.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent.SetName("Balin");
        partyMembers.Add(characterComponent);

        var character1 = Instantiate(fighterCharacterPrefab, partyGameObject);
        var characterComponent1 = character1.GetComponent<Character>();
        characterComponent1.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent1.SetName("Gimli");
        partyMembers.Add(characterComponent1);

        var character2 = Instantiate(mageCharacterPrefab, partyGameObject);
        var characterComponent2 = character2.GetComponent<Character>();
        characterComponent2.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent2.SetName("Gandalf");
        partyMembers.Add(characterComponent2);
    }

    public bool IsPartyWiped()
    {
        return !partyMembers.Any(c => c.GetHitPoints() > 0);
    }

    public List<ICreature> GetPartyMembers()
    {
        return partyMembers;
    }

    public void RemoveCharacterFromParty(Character character)
    {
        partyMembers.Remove(character);
    }
}

