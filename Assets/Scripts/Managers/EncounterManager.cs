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
    /// The tag name for the monster position placeholders in the encounter.
    /// A prefab with this tag will be instantiated in the encounter scene to indicate where monsters will spawn.
    /// </summary>
    public const string MonsterPositionSlotTagName = "EncounterMonsterPositionSlot";

    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;
    private const int MOST_FREQUENT_ENCOUNTER_RATE = 5;

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

    /// <summary>
    /// Sets up the encounter by spawning monsters and starting the battle
    /// </summary>
    public void SetupEncounter()
    {
        battleMenuManager.OpenBattleMenu();

        // Spawn random monsters (for this dungeon) across the encounter's monster position slots
        var monsterPositions = GetMonsterPositions();
        var spawnedMonsters = SpawnMonstersAtPositions(monsterPositions);

        var party = GameManager.Instance.GetComponent<PartyManager>().GetPartyMembers();

        var battleManager = GameManager.Instance.GetComponent<BattleManager>();
        battleManager.partyMembersInEncounter = party;
        battleManager.monstersInEncounter = spawnedMonsters;
        battleManager.StartBattle();
    }

    /// <summary>
    /// Spawns monsters at the given positions using the monster spawners defined in the dungeon data
    /// </summary>
    /// <param name="positions">The <see cref="Vector3"/> positions to spawn monsters</param>
    /// <returns>The spawned monsters</returns>
    private List<Monster> SpawnMonstersAtPositions(IEnumerable<Vector3> positions)
    {
        var spawnedMonsters = new List<Monster>();
        foreach (var position in positions)
        {
            // pick a random monster spawner and spawn a monster at the given position
            var numberOfMonsterSpawnersForDungeon = GameManager.Instance.DungeonData.monsterSpawners.Length;
            if (numberOfMonsterSpawnersForDungeon <= 0)
            {
                Debug.Log("No monster spawners found in dungeon data");
                return null;
            }
            var monsterSpawner = GameManager.Instance.DungeonData.monsterSpawners[Random.Range(0, numberOfMonsterSpawnersForDungeon)];
            var spawnedMonster = monsterSpawner.SpawnMonster(GameManager.Instance.GetEncounterGameObject().transform, position);
            spawnedMonsters.Add(spawnedMonster.GetComponent<Monster>());
        }

        return spawnedMonsters;
    }

    public void EndBattle()
    {
        Debug.Log("Encounter ended");
        battleMenuManager.ExitBattleMenu();
    }

    /// <summary>
    /// Called when the player moves - increments the player's total steps and checks if an encounter should be triggered
    /// </summary>
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

    /// <summary>
    /// Called when the player makes noise - adjusts the current encounter rate based on the noise level
    /// </summary>
    /// <param name="noiseLevel">A numerical representation of the amount of noise made by the player</param>
    private void OnPlayerMadeNoise(int noiseLevel)
    {
        Debug.Log($"Player made noise at level {noiseLevel}");
        _currentEncounterRate = baseEncounterRate - noiseLevel;
    }

    private bool DetermineIfEncountered()
    {
        Debug.Log($"Determining if encounter should happen. Total steps {_playerTotalSteps} % current encounter rate {_currentEncounterRate}");
        if (_currentEncounterRate <= MOST_FREQUENT_ENCOUNTER_RATE)
        {
            _currentEncounterRate = MOST_FREQUENT_ENCOUNTER_RATE;
        }

        if (_playerTotalSteps % _currentEncounterRate == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the positions of the monster position slots in the encounter (where monsters will be spawned)
    /// </summary>
    /// <returns>A list of <see cref="Vector3"/> for each position</returns>
    /// <remarks>These positions are defined through the Unity editor by instantiating a prefab with a tag matching MonsterPositionSlotTagName</remarks>
    private IEnumerable<Vector3> GetMonsterPositions()
    {
        var positionSlots = GameManager.Instance.GetEncounterGameObject().GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(MonsterPositionSlotTagName))
            .Select(t => t.position);
        return positionSlots;
    }
}
