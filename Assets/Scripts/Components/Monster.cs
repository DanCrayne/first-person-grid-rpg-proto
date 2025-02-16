using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a monster in the game
/// </summary>
public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    //public MonsterUI monsterUI;

    private int _currentHitPoints;

    void Start()
    {
        //monsterUI = transform.GetComponent<MonsterUI>();
    }

    public int GetMaxHitPoints()
    {
        return monsterData.hitDice * 6;
    }

    public int GetHitPoints()
    {
        return _currentHitPoints;
    }

    public bool IsMonsterDead()
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

    /// <summary>
    /// Calculates attack damage for the given character
    /// </summary>
    /// <param name="character">The character being attacked</param>
    /// <returns>An <see cref="AttackResult"/> representing the randomized outcome</returns>
    public AttackResult CalculateRandomAttackResult(Character character)
    {
        var randomAttack = Random.Range(0, monsterData.attacks.Length);

        // TODO: get character AC and calculate attack and damage
        bool didAttackHit = true;
        var randomDamage = Random.Range(monsterData.attacks[randomAttack].minDamage, monsterData.attacks[randomAttack].maxDamage);

        return new AttackResult(didAttackHit, monsterData.attacks[randomAttack].attackName, randomDamage);
    }

    public Character SelectAttackTarget(List<Character> characters)
    {
        // just attack the first character for now
        return characters.FirstOrDefault();
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
        {
            Die();
        }
    }

    //public void ShowMonsterAsSelected()
    //{
    //    monsterUI.ShowMonsterAsSelected();
    //}

    //public void ShowMonsterAsDeselected()
    //{
    //    monsterUI.ShowMonsterAsDeselected();
    //}

    private void Die()
    {
        Destroy(gameObject);
    }

    private int RollHp()
    {
        return Random.Range(1, (monsterData.hitDice * 6) + 1);
    }
}