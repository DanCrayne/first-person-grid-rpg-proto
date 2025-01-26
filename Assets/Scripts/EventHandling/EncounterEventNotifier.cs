using System;
using UnityEngine;

public class EncounterEventNotifier : MonoBehaviour
{
    public static event Action OnMonsterCollision;
    public static event Action OnEncounterStart;
    public static event Action OnEncounterEnd;

    public static void MonsterCollision()
    {
        OnMonsterCollision?.Invoke();
    }

    public static void EncounterStart()
    {
        OnEncounterStart?.Invoke();
    }

    public static void EncounterEnd()
    {
        OnEncounterEnd?.Invoke();
    }
}
