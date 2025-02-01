using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages / generates all wandering monsters in the dungeon
/// </summary>
public class WanderingMonsterGroupManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform playerTransform;

    private List<GameObject> _wanderingMonsters = new List<GameObject>();

    private void OnEnable()
    {
        TurnNotifier.OnPlayerMoved += PerformMonsterActions;
        EncounterEventNotifier.OnMonsterDefeated += HandleMonsterDefeated;
    }

    private void OnDisable()
    {
        TurnNotifier.OnPlayerMoved -= PerformMonsterActions;
        EncounterEventNotifier.OnMonsterDefeated += HandleMonsterDefeated;
    }

    public void HideAndDisableWanderingMonsters()
    {

    }

    private void DeleteWanderingMonster(GameObject monster)
    {
        _wanderingMonsters.Remove(monster);
        Destroy(monster);
    }

    private void HandleMonsterDefeated(GameObject monster)
    {
        Debug.Log($"Wandering monster defeated: {monster.name}");
        DeleteWanderingMonster(monster);
    }

    public void PerformMonsterActions()
    {
        foreach (var monster in _wanderingMonsters)
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
            
            // Add monster to the parent GameObject
            monster.transform.SetParent(transform);

            // Add monster management script
            var monsterManagerScript = monster.GetOrAddComponent<WanderingMonsterManager>();
            monsterManagerScript.maxNumberOfMonsters = 3;
            monsterManagerScript.possibleMonsters = new[] { "Goblin" };
            monsterManagerScript.currentPosition = monster.transform.position;

            // Add movement (ai) script
            var aiScript = monster.GetOrAddComponent<WanderingMonsterMovement>();
            aiScript.player = playerTransform.transform;
            aiScript.gridSize = 10;
            aiScript.detectionRange = 20;
            // TODO: detection shape (e.g. cone, column)

            _wanderingMonsters.Add(monster);
        }
    }
}
