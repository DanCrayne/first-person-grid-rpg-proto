using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature : MonoBehaviour
{
    /// <summary>
    /// The basic static data for this character
    /// </summary>
    public CreatureStaticData creatureStaticData;
    public ClassData classData;
    public RaceData raceData;

    public GameObject creatureUIPanelPrefab;
    public GameObject actionControlPrefab;

    public bool shouldDestroyOnDeath = true;

    private string _creatureName;
    private int _currentHitPoints;
    private int _maxHitPoints;
    private int _level = 1;
    private int _experience = 0;

    private void Start()
    {
        if (creatureStaticData?.shouldDestroyOnDeath != null)
        {
            shouldDestroyOnDeath = creatureStaticData.shouldDestroyOnDeath;
        }
    }

    public string GetName()
    {
        return _creatureName;
    }

    public GameObject GetCreaturePrefab()
    {
        if (creatureStaticData.creaturePrefab != null)
        {
            return creatureStaticData.creaturePrefab;
        }
        else
        {
            return null;
        }
    }

    public CreatureStaticData GetCharacterData()
    {
        return creatureStaticData;
    }
    
    public ClassData GetClassData()
    {
        return classData;
    }

    public ICreatureAction Defend()
    {
        // TODO: implement
        return new DefendAction(this);
    }

    public ICreatureAction Attack(Creature creature, List<Creature> fallbackCreatures)
    {
        var attackAction = new AttackAction(this, creature, fallbackCreatures);
        return attackAction;
    }

    public ICreatureAction DecideAction(List<Creature> possibleTargets)
    {
        ICreatureAction action = null;
        if (GetHitPoints() < GetMaxHitPoints() / 2)
        {
            // TODO: implement this action - use a potion or something
            action = Defend();
        }
        else
        {
            Creature weakestCreature = FindCreatureWithLowestHp(possibleTargets);
            action = Attack(weakestCreature, possibleTargets);
        }

        return action;
    }

    private static Creature FindCreatureWithLowestHp(List<Creature> creatures)
    {
        Creature weakestCreature = null;
        int lowestHitPoints = int.MaxValue;
        foreach (var creature in creatures)
        {
            if (creature.GetHitPoints() < lowestHitPoints)
            {
                weakestCreature = creature;
                lowestHitPoints = creature.GetHitPoints();
            }
        }
        return weakestCreature;
    }

    public ItemData GetEquippedWeapon()
    {
        return GetEquipmentManager().GetEquippedWeapon();
    }

    public ICreatureAction Flee(List<Creature> possibleBlockers)
    {
        return new FleeAction(this, possibleBlockers);
    }

    public void EquipWeapon(ItemData weapon)
    {
        GetEquipmentManager().EquipWeapon(weapon);
    }

    public void EquipArmor(ItemData armor)
    {
        GetEquipmentManager().EquipArmor(armor);
    }

    public void UnequipArmor(ItemData armor)
    {
        GetEquipmentManager().UnequipArmor(armor);
    }

    public void UnequipWeapon()
    {
        GetEquipmentManager().UnequipWeapon(GetEquippedWeapon());
    }

    public void AddItemToInventory(ItemData item)
    {
        GetComponent<InventoryManager>().AddItem(item);
    }

    public void RemoveItemFromInventory(ItemData item)
    {
        GetComponent<InventoryManager>().RemoveItem(item);
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

    public int GetMaxHitPoints()
    {
        return creatureStaticData.hitDice;
    }

    public void RestoreAllHitPoints()
    {
        _currentHitPoints = _maxHitPoints;
    }

    public void SetName(string name)
    {
        _creatureName = name;
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
        {
            Die();
        }
    }

    public void Heal(int healing)
    {
        _currentHitPoints += healing;
    }

    public void LevelUp()
    {
        _level++;

        // TODO other level up stuff
    }

    private void Die()
    {
        Debug.Log($"{_creatureName} dies!");
        if (shouldDestroyOnDeath)
        {
            Debug.Log($"Destroying {_creatureName} GameObject");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Creature is configured to not destroy on death");
            // TODO: handle death
        }
    }

    public void GainExperience(int experience)
    {
        _experience += experience;
        if (_experience >= creatureStaticData.classData.XpPerLevel[_level])
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
        var equippedArmor = GetEquipmentManager().GetEquippedArmor();
        var totalArmorClass = equippedArmor.Select(a => a.armorData.armorClass).Sum();
        return totalArmorClass;
    }

    public void RollAndSetRandomStats()
    {
        CalculateAndSetMaxHp();

        // TODO: roll other stats
    }

    private InventoryManager GetInventoryManager()
    {
        return GetComponent<InventoryManager>();
    }

    private EquipmentManager GetEquipmentManager()
    {
        return GetComponent<EquipmentManager>();
    }

    private void CalculateAndSetMaxHp()
    {
        int hitDice = 1;

        if (creatureStaticData == null)
        {
            hitDice = classData.hitDie <= 0 ? classData.hitDie : hitDice;
        }
        else
        {
            hitDice = creatureStaticData.hitDice <= 0 ? creatureStaticData.hitDice : hitDice;
        }

        _maxHitPoints = Random.Range(1, (hitDice * 6) + 1);
        _currentHitPoints = _maxHitPoints;
    }

    public AttackTypeData[] AvailableAttackTypes { get { return GetEquipmentManager().GetEquippedWeapon().weaponData.attackTypeData; } }
}
