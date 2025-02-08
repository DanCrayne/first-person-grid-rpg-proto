using System;
using UnityEngine;

/// <summary>
/// Notifies listeners of general game events
/// </summary>
public class GeneralNotifier : MonoBehaviour
{
    public static event Action OnResetGame;
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;

    /// <summary>
    /// Notify listeners that the game should be reset
    /// </summary>
    public static void ResetGame()
    {
        OnResetGame?.Invoke();
    }

    /// <summary>
    /// Notify listeners that the game should be paused
    /// </summary>
    public static void PauseGame()
    {
        OnPauseGame?.Invoke();
    }

    /// <summary>
    /// Notify listeners that the game should be resumed
    /// </summary>
    public static void ResumeGame()
    {
        OnResumeGame?.Invoke();
    }
}
