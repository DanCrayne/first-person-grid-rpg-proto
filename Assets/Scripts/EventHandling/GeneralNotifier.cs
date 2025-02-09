using System;
using UnityEngine;

/// <summary>
/// Notifies listeners of general game events
/// </summary>
public class GeneralNotifier : MonoBehaviour
{
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;
    public static event Action OnDisableMovement;
    public static event Action OnEnableMovement;

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

    /// <summary>
    /// Notify listeners that movement should be disabled
    /// </summary>
    public static void DisableMovement()
    {
        OnDisableMovement?.Invoke();
    }

    /// <summary>
    /// Notify listeners that movement should be enabled
    /// </summary>
    public static void EnableMovement()
    {
        OnEnableMovement?.Invoke();
    }
}
