using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject encounterObject;
    public GameObject dungeonObject;

    private GameObject _wanderingMonsterContainer; // Reference to the parent WanderingMonster object
    private WanderingMonsterManager _wanderingMonsterManager;
    private Vector3 _battleStartingPosition;

    void Start()
    {
    }

    private void OnEnable()
    {
        EncounterEventNotifier.OnMonsterCollision += EncounterStart;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnMonsterCollision -= EncounterStart;
    }

    public void SetupEncounter(GameObject wanderingMonster)
    {
        _wanderingMonsterManager = wanderingMonster.GetComponent<WanderingMonsterManager>();
        _wanderingMonsterContainer = wanderingMonster;
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
        _wanderingMonsterManager.SpawnMonstersForEncounter(monsterPositions, _battleStartingPosition);
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
        // Let subscribers know to deal with the defeated wandering monster
        EncounterEventNotifier.MonsterDefeated(_wanderingMonsterContainer);

        // Disable the battle camera and hide the battle menu
        //battleMenuPanel.SetActive(false);

        Debug.Log("Encounter manager: encounter ended!");
        encounterObject.SetActive(false);
        dungeonObject.SetActive(true);

        // Notify that the encounter has ended - note this needs to be done after activating the dungeon object or else it won't get the message
        EncounterEventNotifier.EncounterEnd();
    }
}
