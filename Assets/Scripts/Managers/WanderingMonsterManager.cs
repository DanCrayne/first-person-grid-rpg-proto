using System.Collections.Generic;
using UnityEngine;

public class WanderingMonsterManager : MonoBehaviour
{
    public int maxNumberOfMonsters;
    public string [] possibleMonsters = { "Goblin" };
    public Vector3 currentPosition;
    public Vector3 spawnOffset = new Vector3(0, 3, 0);

    private void OnEnable()
    {
        EncounterEventNotifier.OnEncounterStart += OnEncounterStart;
        EncounterEventNotifier.OnEncounterEnd += OnEncounterEnd;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnEncounterStart -= OnEncounterStart;
        EncounterEventNotifier.OnEncounterEnd -= OnEncounterEnd;
    }

    private void OnEncounterStart()
    {
        Debug.Log("Monster is starting an encounter!");
        PrepareForBattle();
    }

    private void OnEncounterEnd()
    {
        Debug.Log("Monster is ending an encounter!");
        // TODO: see if monster is dead, etc
    }

    private void PrepareForBattle()
    {
        Debug.Log("Monster is preparing for battle!");
        SpawnMonstersForEncounter();
    }

    /// <summary>
    /// Spawns the individual monsters that will be used in an encounter
    /// based on the possible monsters that can be generated
    /// </summary>
    public List<GameObject> SpawnMonstersForEncounter()
    {
        Debug.Log("Spawning monsters for encounter!");
        var monsters = new List<GameObject>();

        for (int i = 0; i < maxNumberOfMonsters; i++)
        {
            var chosenMonsterName = possibleMonsters[Random.Range(0, possibleMonsters.Length)];
            var monsterPrefab = Resources.Load<GameObject>($"Monsters/{chosenMonsterName}");
            monsters.Add(monsterPrefab);
        }

        return monsters;
    }
}
