using System;
using UnityEngine;

/// <summary>
/// Notifies listeners of player actions
/// </summary>
public class PlayerActionNotifier : MonoBehaviour
{
    public static event Action OnPlayerMoved;
    public static event Action<int> OnPlayerMadeNoise; // noise made at specific noise-level

    /// <summary>
    /// Notify listeners that the player has moved
    /// </summary>
    public static void PlayerMoved()
    {
        OnPlayerMoved?.Invoke();
    }

    /// <summary>
    /// Notify listeners that the player made noise and at what level
    /// </summary>
    /// <param name="noiseLevel">An arbitrary noise level to indicate how loud the noise was</param>
    public static void PlayerMadeNoise(int noiseLevel)
    {
        OnPlayerMadeNoise?.Invoke(noiseLevel);
    }
}
