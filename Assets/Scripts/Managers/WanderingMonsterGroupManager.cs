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

    private List<GameObject> monsters = new List<GameObject>();

    private void OnEnable()
    {
        TurnManager.OnPlayerMoved += PerformMonsterActions;
    }

    private void OnDisable()
    {
        TurnManager.OnPlayerMoved -= PerformMonsterActions;
    }

    public void PerformMonsterActions()
    {
        foreach (var monster in monsters)
        {
            var aiScript = monster.GetComponent<ManagedGridMovementAi>();
            if (aiScript != null)
            {
                aiScript.PerformActions();
            }
        }
    }

    public void SpawnWanderingMonsterGroups(List<Vector3> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var monster = Instantiate(monsterPrefab, spawnPoint, Quaternion.identity);
            var aiScript = monster.GetOrAddComponent<ManagedGridMovementAi>();
            aiScript.encounterManager = encounterManager;
            aiScript.player = playerTransform.transform;
            aiScript.gridSize = 10;
            aiScript.detectionRange = 20;
            // TODO: detection shape (e.g. cone, column)
            monsters.Add(monster);
        }
    }
}
