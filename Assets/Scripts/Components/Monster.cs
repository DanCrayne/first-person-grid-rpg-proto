using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a monster in the game
/// </summary>
public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    private int currentHitPoints;

    void Start()
    {
        if (monsterData != null)
        {
            currentHitPoints = RollHp();
            gameObject.name = monsterData.monsterName;
        }
    }

    public int GetMaxHitPoints()
    {
        return monsterData.hitDice * 6;
    }

    public int GetHitPoints()
    {
        return currentHitPoints;
    }

    public void Attack(Character character)
    {
        var randomAttack = Random.Range(0, monsterData.attacks.Length);
        var randomDamage = Random.Range(monsterData.attacks[randomAttack].minDamage, monsterData.attacks[randomAttack].maxDamage);
        character.TakeDamage(randomDamage);
    }

    public Character SelectAttackTarget(List<Character> characters)
    {
        // just attack the first character for now
        return characters.FirstOrDefault();
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0)
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
}