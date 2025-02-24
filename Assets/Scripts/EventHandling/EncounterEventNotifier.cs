using System;
using UnityEngine;

/// <summary>
/// Notifies listeners of encounter events
/// </summary>
public class EncounterEventNotifier : MonoBehaviour
{
    public static event Action OnEncounterStart;
    public static event Action OnEncounterEnd;
    public static event Action<GameObject> OnMonsterDefeated;
    public static event Action OnPlayerSelectingTarget;
    public static event Action<Transform> OnPlayerSelectedTarget;
    public static event Action<AttackResult> OnAttack;

    /// <summary>
    /// Notify listeners that an encounter has started
    /// </summary>
    public static void EncounterStart()
    {
        OnEncounterStart?.Invoke();
    }

    /// <summary>
    /// Notify listeners that an encounter has ended
    /// </summary>
    public static void EncounterEnd()
    {
        OnEncounterEnd?.Invoke();
    }

    /// <summary>
    /// Notify listeners that a monster has been defeated
    /// </summary>
    /// <param name="monster">The GameObject of the defeated monster</param>
    public static void MonsterDefeated(GameObject monster)
    {
        OnMonsterDefeated?.Invoke(monster);
    }

    /// <summary>
    /// Notifies listeners that the player is currently selecting a target in battle
    /// </summary>
    public static void PlayerSelectingTarget()
    {
        OnPlayerSelectingTarget?.Invoke();
    }

    /// <summary>
    /// Notifies listeners that the player selected a target and provides the target for reference
    /// </summary>
    /// <param name="target">The selected target</param>
    public static void PlayerSelectedTarget(Transform target)
    {
        OnPlayerSelectedTarget?.Invoke(target);
    }

    /// <summary>
    /// Notifies listeners that a creature has been attacked
    /// </summary>
    /// <param name="attackResult">The <see cref="AttackResult"/> for the attack</param>
    public static void NotifyAttack(AttackResult attackResult)
    {
        OnAttack?.Invoke(attackResult);
    }
}
