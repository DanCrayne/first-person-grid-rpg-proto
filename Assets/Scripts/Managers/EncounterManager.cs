using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;

    public int baseEncounterRate = 10;
    public static string PartyPositionSlotTagName = "EncounterPartyPositionSlot";
    public static string MonsterPositionSlotTagName = "EncounterMonsterPositionSlot";

    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;
    private List<GameObject> _monstersInEncounter = new List<GameObject>();
    private List<MonsterSpawner> _monsterSpawners = new List<MonsterSpawner>();

    void Start()
    {
        PlayerActionNotifier.OnPlayerMadeNoise += OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved += OnPlayerMoved;
        _currentEncounterRate = baseEncounterRate;

        // Create monster spawners for each possible monster
        foreach (var monster in GameManager.Instance.DungeonData.possibleMonsters)
        {
            var monsterSpawnerObject = new GameObject("MonsterSpawner");
            var monsterSpawner = monsterSpawnerObject.AddComponent<MonsterSpawner>();
            monsterSpawner.monsterData = monster;
            _monsterSpawners.Add(monsterSpawner);
        }
    }

    public void SetupEncounter()
    {
        battleMenuPanel.SetActive(true);

        // Spawn random monsters (for this dungeon) across the encounter's monster position slots
        var monsterPositions = GetMonsterPositions();
        foreach (var position in monsterPositions)
        {
            // pick a random monster spawner to use
            var monsterSpawner = _monsterSpawners[Random.Range(0, _monsterSpawners.Count)];
            var spawnedMonster = monsterSpawner.SpawnMonster(GameManager.Instance.GetEncounterGameObject().transform, position);
            _monstersInEncounter.Add(spawnedMonster);
        }
    }

    public void HandleAttack()
    {
        if (_monstersInEncounter.Count >= 1)
        {
            var targetedMonster = _monstersInEncounter.FirstOrDefault();
            targetedMonster.GetComponent<Monster>().TakeDamage(100);
            _monstersInEncounter.Remove(targetedMonster);
        }

        if (_monstersInEncounter.Count < 1)
        {
            EndBattle();
        }
    }

    public void EndBattle()
    {
        // Disable the battle camera and hide the battle menu
        battleMenuPanel.SetActive(false);

        Debug.Log("Encounter manager: encounter ended!");

        // Notify that the encounter has ended - note this needs to be done after activating the dungeon object or else it won't get the message
        EncounterEventNotifier.EncounterEnd();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
        
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
