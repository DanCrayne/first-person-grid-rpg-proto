using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject encounterObject;
    public GameObject dungeonObject;

    private GameObject _selectedMonster;
    private WanderingMonsterManager _wanderingMonsterManager;
    private Vector3 _battleStartingPosition;
    private List<GameObject> _monsters = new List<GameObject>();
    private GameObject _wanderingMonsterParent; // Reference to the parent WanderingMonster object

    void Start()
    {
        //if (battleMenuPanel != null)
        //    battleMenuPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EncounterEventNotifier.OnMonsterCollision += EncounterStart;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnMonsterCollision -= EncounterStart;
    }

    public void SetupEncounter(GameObject wanderingMonsterManager)
    {
        _wanderingMonsterManager = wanderingMonsterManager.GetComponent<WanderingMonsterManager>();
        _monsters = _wanderingMonsterManager.SpawnMonstersForEncounter();
        _wanderingMonsterParent = wanderingMonsterManager; // Store the reference to the parent WanderingMonster object
        LayoutBattle();
    }

    /// <summary>
    /// Lays out battle by placing monsters and other characters
    /// </summary>
    /// <remarks>maximum of 3 members</remarks>
    private void LayoutBattle()
    {
        var offCenterAdjustment = new Vector3(0, 0, 10);
        var monsterStartingPosition = _battleStartingPosition + offCenterAdjustment;
        var monsterSideOffsetAdjustment = new Vector3(5, 0, 0);
        Vector3[] monsterPositions = { monsterStartingPosition, monsterStartingPosition - monsterSideOffsetAdjustment, monsterStartingPosition + monsterSideOffsetAdjustment };

        for (int i = 0; i < _monsters.Count; i++)
        {
            var monsterPrefab = _monsters[i];
            var monsterInstance = Instantiate(monsterPrefab, monsterPositions[i % 3], Quaternion.identity);
            monsterInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Calculate the direction to the battle starting position
            Vector3 directionToBattleStart = (_battleStartingPosition - monsterInstance.transform.position).normalized;

            // Calculate the rotation to face the battle starting position
            Quaternion lookRotation = Quaternion.LookRotation(directionToBattleStart);

            // Set the monster's rotation
            monsterInstance.transform.rotation = lookRotation;

            // Update the list with the instantiated monster
            _monsters[i] = monsterInstance;
        }
    }

    private void EncounterStart()
    {
        Debug.Log("Encounter manager: encounter started!");
        EncounterEventNotifier.EncounterStart();
        encounterObject.SetActive(true);
        dungeonObject.SetActive(false);
    }

    public void HandleAttack()
    {
        EndBattle();
    }

    public void EndBattle()
    {
        // Destroy each monster GameObject
        foreach (var monster in _monsters)
        {
            if (monster != null && monster.scene.IsValid()) // Ensure it's an instance in the scene
            {
                Destroy(monster);
            }
        }

        // Clear the list of monsters
        _monsters.Clear();

        // Destroy the parent WanderingMonster object
        if (_wanderingMonsterParent != null && _wanderingMonsterParent.scene.IsValid()) // Ensure it's an instance in the scene
        {
            Destroy(_wanderingMonsterParent);
            _wanderingMonsterParent = null; // Clear the reference
        }

        // Disable the battle camera and hide the battle menu
        battleMenuPanel.SetActive(false);

        // Notify that the encounter has ended
        EncounterEventNotifier.EncounterEnd();

        encounterObject.SetActive(false);
        dungeonObject.SetActive(true);
    }


    private void PositionMonsters(List<GameObject> monsters)
    {

    }

    private void PositionPlayerParty(List<GameObject> partyMembers)
    {

    }
}
