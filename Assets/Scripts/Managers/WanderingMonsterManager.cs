using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WanderingMonsterManager : MonoBehaviour
{
    public int maxNumberOfMonsters = 3;
    public string [] possibleMonsters = { "Goblin" };
    public Vector3 currentPosition;
    public Vector3 spawnOffset = new Vector3(0, 3, 0);

    private List<GameObject> _monsters;

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

    public void DestroySingleMonster(GameObject monster)
    {
        _monsters.Remove(monster);
        Destroy(monster);
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
    }

    public void SpawnMonstersForEncounter(Vector3[] monsterPositions, Vector3 encounterStartingPosition)
    {
        Debug.Log("Spawning monsters for encounter!");
        _monsters = new List<GameObject>();
        var index = 0;

        foreach (var position in monsterPositions)
        {
            var monsterPrefabName = possibleMonsters[Random.Range(0, possibleMonsters.Length)];
            var monsterPrefab = Resources.Load<GameObject>($"Monsters/{monsterPrefabName}");
            var monsterInstance = Instantiate(monsterPrefab, monsterPositions[index % maxNumberOfMonsters], Quaternion.identity);

            // Set this *encounter monster*'s parent object as the *wandering monster* for better organization and so that when the parent is destroyed, the child will be too
            monsterInstance.transform.SetParent(transform);

            // Calculate the direction to the battle starting position
            Vector3 directionToBattleStart = (encounterStartingPosition - monsterInstance.transform.position).normalized;
            // Calculate the rotation to face the battle starting position
            Quaternion lookRotation = Quaternion.LookRotation(directionToBattleStart);
            // Set the monster's rotation
            monsterInstance.transform.rotation = lookRotation;

            _monsters.Add(monsterInstance);
            index++;
        }
    }

    public void ShowMonsters()
    {
        foreach (var monster in _monsters)
        {
            monster.SetActive(true);
        }
    }

    public void HideMonsters()
    {
        foreach (var monster in _monsters)
        {
            monster.SetActive(false);
        }
    }
}
