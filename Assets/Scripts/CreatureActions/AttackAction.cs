using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackAction : ICreatureAction
{
    public Creature source;
    public Creature target;
    public List<Creature> fallbackTargets;

    public AttackAction(Creature source, Creature target, List<Creature> fallbackTargets = null)
    {
        this.source = source;
        this.target = target;
        this.fallbackTargets = fallbackTargets;
    }

    private AttackResult TryToHitTargetAndCalculateDamage(Creature originalOrFallbackTarget)
    {
        if (originalOrFallbackTarget == null)
        {
            return new AttackResult(source, originalOrFallbackTarget, 0, false, $"{source.GetName()} cannot see a target to attack!");
        }

        // TODO: get character AC and calculate attack and damage

        bool didAttackHit = true;
        var damage = 0;
        var resultMessage = $"{source.GetName()} attacks {originalOrFallbackTarget.GetName()} and misses!";

        if (didAttackHit)
        {
            var equippedWeapon = source.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                damage = Random.Range(equippedWeapon.weaponData.minDamage, equippedWeapon.weaponData.maxDamage);
                resultMessage = $"{source.GetName()} attacks {originalOrFallbackTarget.GetName()} and deals {damage} damage";
            }
            else
            {
                Debug.LogError($"Could not get equipped weapon for {source.GetName()}'s attack");
            }
        }

        return new AttackResult(source, originalOrFallbackTarget, damage, didAttackHit, resultMessage);
    }

    public Func<string>[] Perform()
    {
        Func<string>[] effects = new Func<string>[]
        {
            () =>
            {
                var resultMessage = "";
                AttackResult attackResult = null;

                // if target is dead, try to attack a fallback target
                var currentTarget = target != null && !target.IsDead() ? target : fallbackTargets?.FirstOrDefault(t => t != null && !t.IsDead());

                if (currentTarget != null)
                {
                    attackResult = TryToHitTargetAndCalculateDamage(currentTarget);
                    EncounterEventNotifier.NotifyAttack(attackResult);

                    var actualResultMessage = attackResult.resultMessage;   

                    currentTarget.TakeDamage(attackResult.damage);
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
