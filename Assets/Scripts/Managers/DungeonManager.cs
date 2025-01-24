using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public WanderingMonsterGroupManager wanderingMonsterGroupManager;

    void Start()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        SpawnDungeonMonsterGroups(spawnPoints);
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
