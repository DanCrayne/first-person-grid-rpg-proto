using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static event Action OnPlayerMoved;
    public static event Action OnMonstersMoved;

    public static void PlayerMoved()
    {
        OnPlayerMoved?.Invoke();
    }

    public static void MonstersMoved()
    {
        OnMonstersMoved?.Invoke();
    }
}
