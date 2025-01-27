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
        TurnNotifier.OnPlayerMoved += PerformMonsterActions;
    }

    private void OnDisable()
    {
        TurnNotifier.OnPlayerMoved -= PerformMonsterActions;
    }

    public void PerformMonsterActions()
    {
        foreach (var monster in monsters)
        {
            var aiScript = monster.GetComponent<WanderingMonsterMovement>();
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

            // Add monster management script
            var monsterManagerScript = monster.GetOrAddComponent<WanderingMonsterManager>();
            monsterManagerScript.maxNumberOfMonsters = 4;
            monsterManagerScript.possibleMonsters = new[] { "Goblin" };
            monsterManagerScript.currentPosition = monster.transform.position;

            // Add movement (ai) script
            var aiScript = monster.GetOrAddComponent<WanderingMonsterMovement>();
            aiScript.encounterManager = encounterManager;
            aiScript.player = playerTransform.transform;
            aiScript.gridSize = 10;
            aiScript.detectionRange = 20;
            // TODO: detection shape (e.g. cone, column)

            monsters.Add(monster);
        }
    }
}
