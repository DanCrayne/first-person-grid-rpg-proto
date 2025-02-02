using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonData dungeonData;

    void Start()
    {
    }

    private void OnEnable()
    {
        GeneralNotifier.OnResetGame += HandleGameReset;
        GeneralNotifier.OnPauseGame += PauseGame;
        GeneralNotifier.OnResumeGame += ResumeGame;
    }

    private void OnDisable()
    {
        GeneralNotifier.OnResetGame -= HandleGameReset;
        GeneralNotifier.OnPauseGame -= PauseGame;
        GeneralNotifier.OnResumeGame -= ResumeGame;
    }

    /// <summary>
    /// Pauses the game, including animations and anything that relies on Time.deltaTime
    /// such as physics and movement.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void HandleGameReset()
    {
    }
}
