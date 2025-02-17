using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackAction : ICreatureAction
{
    public ICreature source;
    public ICreature target;
    public List<ICreature> fallbackTargets;

    public AttackAction(ICreature source, ICreature target, List<ICreature> fallbackTargets = null)
    {
        this.source = source;
        this.target = target;
        this.fallbackTargets = fallbackTargets;
    }

    //public void SetFallbackAttackTargets(List<ICreature> fallbackTargets)
    //{
    //    this.fallbackTargets = fallbackTargets;
    //}

    private (bool, int, string) TryToHitTargetAndCalculateDamage(ICreature originalOrFallbackTarget)
    {
        if (originalOrFallbackTarget == null)
        {
            return (false, 0, "Nothing to attack");
        }

        // TODO: get character AC and calculate attack and damage

        bool didAttackHit = true;
        var damage = 0;
        var resultMessage = $"{source.GetName()} attacks {originalOrFallbackTarget.GetName()} and misses!";

        if (didAttackHit)
        {
            var equippedWeapon = source.GetEquippedWeapon();
            //randomDamage = Random.Range(equippedWeapon.minDamage, equippedWeapon.maxDamage);
            damage = Random.Range(1, 20);
            resultMessage = $"{source.GetName()} attacks {originalOrFallbackTarget.GetName()} and deals {damage} damage";
        }

        return (didAttackHit, damage, resultMessage);
    }

    public Func<string>[] Perform()
    {
        Func<string>[] effects = new Func<string>[]
        {
            () =>
            {
                var resultMessage = "";
                var currentTarget = target != null && !target.IsDead() ? target : fallbackTargets?.FirstOrDefault(t => t != null && !t.IsDead());

                if (currentTarget != null)
                {
                    (var didAttackHit, var damage, var actualResultMessage) = TryToHitTargetAndCalculateDamage(currentTarget);

                    currentTarget.TakeDamage(damage);
                    if (currentTarget.IsDead())
                    {
                        actualResultMessage += $" ... target is defeated!";
                    }

                    resultMessage = actualResultMessage;
                }
                else
                {
                    resultMessage = "No valid targets to attack.";
                }

                return resultMessage;
            }
        };

        return effects;
    }
}
