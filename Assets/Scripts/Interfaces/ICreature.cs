using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ICreature
{
    public string GetName();

    public ICreatureAction Attack(ICreature target, List<ICreature> fallbackTargets = null);

    public bool IsDead();

    public int GetHitPoints();

    public void SetHitPoints(int hitPoints);

    public int GetMaxHitPoints();

    public int GetArmorClass();

    public void TakeDamage(int damage);

    public void Heal(int healing);

    public void SetLevel(int level);

    public int GetLevel();

    public void Defend();

    public WeaponData GetEquippedWeapon();

    public AttackTypeData[] AttackTypeData { get; }
}
