using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public WanderingMonsterGroupManager wanderingMonsterGroupManager;
    public bool isPlayerTurn;

    void Start()
    {
        var monsterSpawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
        SpawnDungeonMonsterGroups(monsterSpawnPoints);
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
