using System.Collections.Generic;
using UnityEngine;

public class WanderingMonster : MonoBehaviour
{
    public int maxNumberOfMonsters;
    public List<GameObject> possibleMonsters;
    public Vector3 currentPosition;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Monster collided with player!");
            // Notify the EncounterManager of the collision
        }
    }

    /// <summary>
    /// Spawns the individual monsters that will be used in an encounter
    /// based on the possible monsters that can be generated
    /// </summary>
    public void SpawnMonstersForEncounter()
    {
        Debug.Log("Spawning monsters for encounter!");
    }
}
