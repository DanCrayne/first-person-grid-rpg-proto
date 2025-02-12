using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public BattleMenuManager battleMenuManager;

    /// <summary>
    /// The base encounter rate for the dungeon (higher is less frequent)
    /// </summary>
    public int baseEncounterRate = 10;

    /// <summary>
    /// The tag name for the monster position placeholders in the encounter
    /// </summary>
    public static string MonsterPositionSlotTagName = "EncounterMonsterPositionSlot";

    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;
    private List<Monster> _monstersInEncounter = new List<Monster>();

    private void OnEnable()
    {
        PlayerActionNotifier.OnPlayerMadeNoise += OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved += OnPlayerMoved;
        EncounterEventNotifier.OnEncounterEnd += EndBattle;
        _currentEncounterRate = baseEncounterRate;
    }

    private void OnDisable()
    {
        PlayerActionNotifier.OnPlayerMadeNoise -= OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved -= OnPlayerMoved;
        EncounterEventNotifier.OnEncounterEnd -= EndBattle;
    }

    public void SetupEncounter()
    {
        battleMenuManager.OpenBattleMenu();

        // Spawn random monsters (for this dungeon) across the encounter's monster position slots
        var monsterPositions = GetMonsterPositions();
        SpawnMonstersAtPositions(monsterPositions);

        var battleManager = GameManager.Instance.GetComponent<BattleManager>();
        var party = GameManager.Instance.GetComponent<PartyManager>().GetPartyMembers();
        battleManager.StartBattle(party, _monstersInEncounter);
    }

    private void SpawnMonstersAtPositions(IEnumerable<Vector3> positions)
    {
        var ps = positions.ToList();

        foreach (var position in positions)
        {
            // pick a random monster spawner and spawn a monster at the given position
            var numberOfMonsterSpawnersForDungeon = GameManager.Instance.DungeonData.monsterSpawners.Length;
            if (numberOfMonsterSpawnersForDungeon <= 0)
            {
                Debug.Log("No monster spawners found in dungeon data");
                return;
            }
            var monsterSpawner = GameManager.Instance.DungeonData.monsterSpawners[Random.Range(0, numberOfMonsterSpawnersForDungeon)];
            var spawnedMonster = monsterSpawner.SpawnMonster(GameManager.Instance.GetEncounterGameObject().transform, position);
            _monstersInEncounter.Add(spawnedMonster.GetComponent<Monster>());
        }
    }

    private void DestroyMonstersInEncounter()
    {
        foreach (var monster in _monstersInEncounter)
        {
            Destroy(monster);
        }
        _monstersInEncounter.Clear();
    }

    public void EndBattle()
    {
        Debug.Log("Encounter ended");
        battleMenuManager.ExitBattleMenu();
    }

    private void OnPlayerMoved()
    {
        _playerTotalSteps += 1;
        Debug.Log($"Player stepped, total steps: {_playerTotalSteps}");
        if (DetermineIfEncountered())
        {
            Debug.Log($"Encounter manager: Encounter triggered! Total steps {_playerTotalSteps}, encounter rate: {_currentEncounterRate}");
            EncounterEventNotifier.EncounterStart();
            SetupEncounter();
        }
    }

    private void OnPlayerMadeNoise(int noiseLevel)
    {
        Debug.Log($"Player made noise at level {noiseLevel}");
        _currentEncounterRate = baseEncounterRate - noiseLevel;
    }

    private bool DetermineIfEncountered()
    {
        Debug.Log($"Determining if encounter should happen. Total steps {_playerTotalSteps} % current encounter rate {_currentEncounterRate}");
        if (_currentEncounterRate <= 5)
        {
            _currentEncounterRate = 5; // most frequent possible encounter rate
        }

        if (_playerTotalSteps % _currentEncounterRate == 0)
        {
            return true;
        }

        return false;
    }

    private IEnumerable<Vector3> GetMonsterPositions()
    {
        var positionSlots = GameManager.Instance.GetEncounterGameObject().GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(MonsterPositionSlotTagName))
            .Select(t => t.position);
        return positionSlots;
    }
}
