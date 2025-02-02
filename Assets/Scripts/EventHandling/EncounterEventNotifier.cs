using System;
using UnityEngine;

public class EncounterEventNotifier : MonoBehaviour
{
    public static event Action OnEncounterStart;
    public static event Action OnEncounterEnd;
    public static event Action<GameObject> OnMonsterDefeated;


    public static void EncounterStart()
    {
        OnEncounterStart?.Invoke();
    }

    public static void EncounterEnd()
    {
        OnEncounterEnd?.Invoke();
    }

    public static void MonsterDefeated(GameObject monster)
    {
        OnMonsterDefeated?.Invoke(monster);
    }
}
