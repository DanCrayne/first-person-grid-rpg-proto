using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public WanderingMonsterGroupManager wanderingMonsterGroupManager;
    public bool isPlayerTurn;

    private GameObject[] _monsterSpawnPoints; 

    void Start()
    {
        _monsterSpawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
        SpawnDungeonMonsterGroups(_monsterSpawnPoints);
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
        SpawnDungeonMonsterGroups(_monsterSpawnPoints);

    }

    public void SpawnDungeonMonsterGroups(GameObject[] spawnPoints)
    {
        var spawnPointPositions = new List<Vector3>();
        foreach (var spawnPoint in spawnPoints)
        {
            var spawnPointTransform = spawnPoint.GetComponent<Transform>();
            spawnPointPositions.Add(spawnPointTransform.position);
        }
        
        wanderingMonsterGroupManager.SpawnWanderingMonsterGroups(spawnPointPositions);
    }
}
