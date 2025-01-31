using System;
using UnityEngine;

public class GeneralNotifier : MonoBehaviour
{
    public static event Action OnResetGame;
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;

    public static void ResetGame()
    {
        OnResetGame?.Invoke();
    }

    public static void PauseGame()
    {
        OnPauseGame?.Invoke();
    }

    public static void ResumeGame()
    {
        OnResumeGame?.Invoke();
    }
}
