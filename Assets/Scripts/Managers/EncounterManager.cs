using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject encounterObject;
    public int baseEncounterRate = 10;
    public static string PartyPositionSlotTagName = "EncounterPartyPositionSlot";
    public static string MonsterPositionSlotTagName = "EncounterMonsterPositionSlot";

    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;
    private List<GameObject> _monstersInEncounter = new List<GameObject>();

    void Start()
    {
        PlayerActionNotifier.OnPlayerMadeNoise += OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved += OnPlayerMoved;
        _currentEncounterRate = baseEncounterRate;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
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

    public void SetupEncounter()
    {
        battleMenuPanel.SetActive(true);
        var monsterSpawner = GetComponent<MonsterSpawner>();

        if (monsterSpawner == null)
        {
            Debug.LogError("Monster spawner doesn't exist");
            return;
        }

        var monsterPositions = GetMonsterPositions();
        for ( int i = 0; i < 6; i++)
        {
            var spawnedMonster = monsterSpawner.SpawnMonster(encounterObject.transform, monsterPositions.ElementAt(i));
            _monstersInEncounter.Add(spawnedMonster);
        }
    }

    public void HandleAttack()
    {
        if (_monstersInEncounter.Count < 1)
        {
            EndBattle();
        }

        var currentlyAttackeMonster = _monstersInEncounter.FirstOrDefault();
        currentlyAttackeMonster.GetComponent<Monster>().TakeDamage(100);
        _monstersInEncounter.Remove(currentlyAttackeMonster);
    }

    public void EndBattle()
    {
        // Disable the battle camera and hide the battle menu
        battleMenuPanel.SetActive(false);

        Debug.Log("Encounter manager: encounter ended!");

        // Notify that the encounter has ended - note this needs to be done after activating the dungeon object or else it won't get the message
        EncounterEventNotifier.EncounterEnd();
    }

    private IEnumerable<Vector3> GetMonsterPositions()
    {
        var positionSlots = encounterObject.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(MonsterPositionSlotTagName))
            .Select(t => t.position);
        return positionSlots;
    }
}
