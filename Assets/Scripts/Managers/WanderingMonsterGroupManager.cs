using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages / generates all wandering monsters in the dungeon
/// </summary>
public class WanderingMonsterGroupManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public EncounterManager encounterManager;
    public Transform playerTransform;

    public void SpawnWanderingMonsterGroups(List<Vector3> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var monster = Instantiate(monsterPrefab, spawnPoint, Quaternion.identity);
            var aiScript = monster.GetOrAddComponent<GridMovementAi>();
            aiScript.encounterManager = encounterManager;
            aiScript.player = playerTransform.transform;
            aiScript.gridSize = 10;
            aiScript.detectionRange = 20;
        }
    }
}
