using System;
using UnityEngine;

/// <summary>
/// Represents a character in the game
/// </summary>
/// <remarks>We're using a class instead of a scriptable object here since the character data will change frequently at run-time.</remarks>
[Serializable]
public class Character : MonoBehaviour
{
    /// <summary>
    /// The basic static data for this character
    /// </summary>
    public CharacterData characterData;

    /// <summary>
    /// The <see cref="CharacterPanel"/> for this character representing their current status as a UI panel during battle
    /// </summary>
    //private EncounterCharacterInfo _encounterCharacterInfo;

    private string _characterName;
    private int _currentHitPoints;
    private int _level = 1;
    private int _experience = 0;
    private ItemData[] inventory;

    private void Start()
    {
        _currentHitPoints = characterData.characterClass.hitDie * _level;
    }

    public int GetEquippedWeaponAttack()
    {
        // TODO: make this based on equipped weapon
        return UnityEngine.Random.Range(1, 20);
    }

    public bool IsCharacterDead()
    {
        if (_currentHitPoints <= 0)
        {
            return true;
        }

        return false;
    }

    public int GetHitPoints()
    {
        return _currentHitPoints;
    }

    public void SetHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
    }

    public int GetMaxHitPoints()
    {
        return characterData.characterClass.hitDie;
    }

    public void SetCharacterName(string name)
    {
        _characterName = name;
    }

    public string GetCharacterName()
    {
        return _characterName;
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;
    }

    public void Heal(int healing)
    {
        _currentHitPoints += healing;
    }

    public void LevelUp()
    {
        _level++;
        // TODO increase stats
    }

    public void GainExperience(int experience)
    {
        _experience += experience;
        if (_experience >= characterData.characterClass.XpPerLevel[_level])
        {
            // do some kind of fancy level up animation
            LevelUp();
        }
    }

    public void SetLevel(int level)
    {
        _level = level;
    }

    public int GetLevel()
    {
        return _level;
    }
}
