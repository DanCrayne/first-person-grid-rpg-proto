using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a monster in the game
/// </summary>
public class Monster : MonoBehaviour, ICreature
{
    public MonsterData monsterData;

    private int _currentHitPoints;
    private int _level;

    void Start()
    {
    }

    public string GetName()
    {
        return monsterData.monsterName;
    }

    public WeaponData GetEquippedWeapon()
    {
        return monsterData.equippedWeapon;
    }

    public int GetMaxHitPoints()
    {
        return monsterData.hitDice * 6;
    }

    public int GetHitPoints()
    {
        return _currentHitPoints;
    }

    public bool IsDead()
    {
        return _currentHitPoints <= 0;
    }

    public void SetHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
    }

    public void RollAndSetStats()
    {
        _currentHitPoints = RollHp();
        // Set other stats if needed
        // For example, if you have other stats like strength, dexterity, etc.
        // You can roll and set them here

        // Example:
        // _strength = Random.Range(1, 20);
        // _dexterity = Random.Range(1, 20);
        // _constitution = Random.Range(1, 20);
        // _intelligence = Random.Range(1, 20);
        // _wisdom = Random.Range(1, 20);
        // _charisma = Random.Range(1, 20);
    }

    public ICreatureAction Attack(ICreature creature, List<ICreature> fallbackCreatures)
    {
        var attackAction = new AttackAction(this, creature, fallbackCreatures);
        return attackAction;
    }

    public void Defend()
    {
        // TODO: implement
    }

    public ICreature SelectAttackTarget(List<ICreature> creatures)
    {
        // just attack the first character for now
        return creatures.FirstOrDefault();
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private int RollHp()
    {
        return Random.Range(1, (monsterData.hitDice * 6) + 1);
    }

    public void Heal(int healing)
    {
        _currentHitPoints += healing;
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
        return monsterData.armorClass;
    }

    public AttackTypeData[] AttackTypeData { get { return AttackTypeData; } }
}