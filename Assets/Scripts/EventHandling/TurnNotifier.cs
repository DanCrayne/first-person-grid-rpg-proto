using System;
using UnityEngine;

public class TurnNotifier : MonoBehaviour
{
    public static event Action OnPlayerMoved;
    public static event Action<GameObject> OnMonsterMoved;

    public static void PlayerMoved()
    {
        OnPlayerMoved?.Invoke();
    }

    public static void MonsterMoved(GameObject monster)
    {
        OnMonsterMoved?.Invoke(monster);
    }
}
