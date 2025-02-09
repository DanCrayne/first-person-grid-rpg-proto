using System;
using UnityEngine;

/// <summary>
/// Represents a character in the game
/// </summary>
/// <remarks>We're using a class instead of a scriptable object here since the character data will change frequently at run-time.</remarks>
[Serializable]
public class Character
{
    private CharacterData characterData;
    private EncounterCharacterInfo encounterCharacterInfo;

    public Character(CharacterData characterData)
    {
        this.characterData = characterData;
    }

    public Character(CharacterData characterData, EncounterCharacterInfo encounterCharacterInfo)
    {
        this.characterData = characterData;
        this.encounterCharacterInfo = encounterCharacterInfo;
    }

    public void SetEncounterCharacterInfo(EncounterCharacterInfo encounterCharacterInfo)
    {
        this.encounterCharacterInfo = encounterCharacterInfo;
    }

    public void SetCharacterInfo()
    {
        encounterCharacterInfo.SetCharacterInfo(characterData.characterName, characterData.hitPoints);
    }


    public void TakeDamage(int damage)
    {
        characterData.hitPoints -= damage;
        encounterCharacterInfo.SetCharacterInfo(characterData.characterName, characterData.hitPoints);
    }

    public void Heal(int healing)
    {
        characterData.hitPoints += healing;
        encounterCharacterInfo.SetCharacterInfo(characterData.characterName, characterData.hitPoints);
    }
}
