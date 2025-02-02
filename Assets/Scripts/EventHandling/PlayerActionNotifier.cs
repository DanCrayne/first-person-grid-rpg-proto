using System;
using UnityEngine;

public class PlayerActionNotifier : MonoBehaviour
{
    public static event Action OnPlayerMoved;
    public static event Action OnPlayerMadeQuietNoise;
    public static event Action OnPlayerMadeMediumNoise;
    public static event Action OnPlayerMadeLoudNoise;
    public static event Action<int> OnPlayerMadeNoise; // noise made at specific noise-level

    public static void PlayerMoved()
    {
        OnPlayerMoved?.Invoke();
    }

    public static void PlayerMadeQuietNoise()
    {
        OnPlayerMadeQuietNoise?.Invoke();
    }

    public static void PlayerMadeMediumNoise()
    {
        OnPlayerMadeMediumNoise?.Invoke();
    }

    public static void PlayerMadeLoudNoise()
    {
        OnPlayerMadeLoudNoise?.Invoke();
    }

    public static void PlayerMadeNoise(int noiseLevel)
    {
        OnPlayerMadeNoise?.Invoke(noiseLevel);
    }
}
