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
    public List<Character> partyMembers = new List<Character>();

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
        characterComponent.SetCharacterName("Balin");
        partyMembers.Add(characterComponent);

        var character1 = Instantiate(fighterCharacterPrefab, partyGameObject);
        var characterComponent1 = character1.GetComponent<Character>();
        characterComponent1.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent1.SetCharacterName("Gimli");
        partyMembers.Add(characterComponent1);

        var character2 = Instantiate(mageCharacterPrefab, partyGameObject);
        var characterComponent2 = character2.GetComponent<Character>();
        characterComponent2.SetHitPoints(defaultCharacterData.characterClass.hitDie);
        characterComponent2.SetCharacterName("Gandalf");
        partyMembers.Add(characterComponent2);
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
}

