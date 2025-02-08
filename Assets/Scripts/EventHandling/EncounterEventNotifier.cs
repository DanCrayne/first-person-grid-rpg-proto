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
}
