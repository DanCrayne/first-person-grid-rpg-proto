using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a character in the game
/// </summary>
/// <remarks>We're using a class instead of a scriptable object here since the character data will change frequently at run-time.</remarks>
[Serializable]
public class Character : MonoBehaviour, ICreature
{
    /// <summary>
    /// The basic static data for this character
    /// </summary>
    public CharacterData characterData;

    private string _characterName;
    private int _currentHitPoints;
    private int _level = 1;
    private int _experience = 0;
    private ItemData[] inventory;
    private WeaponData equippedWeapon;
    private ArmorData[] equippedArmor;

    private void Start()
    {
        _currentHitPoints = characterData.characterClass.hitDie * _level;
    }

    public string GetName()
    {
        return _characterName;
    }

    public void Defend()
    {
        // TODO: implement
    }

    public ICreatureAction Attack(ICreature creature, List<ICreature> fallbackCreatures)
    {
        var attackAction = new AttackAction(this, creature, fallbackCreatures);
        return attackAction;
    }

    public WeaponData GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public bool IsDead()
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

    public void SetName(string name)
    {
        _characterName = name;
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

    public int GetArmorClass()
    {
        var equippedArmorBonus = equippedArmor.Select(a => a.ArmorClassBonus).Sum();
        return equippedArmorBonus;
    }

    public AttackTypeData[] AttackTypeData { get { return equippedWeapon.attackTypeData; } }
}
