using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject battleCam;
    public GameObject encounterObject;
    public GameObject dungeonObject;

    private GameObject _selectedMonster;
    private WanderingMonsterManager _wanderingMonsterManager;
    private Vector3 _battleStartingPosition;
    private List<GameObject> _monsters = new List<GameObject>();
    private GameObject _wanderingMonsterParent; // Reference to the parent WanderingMonster object

    void Start()
    {
        if (battleMenuPanel != null)
            battleMenuPanel.SetActive(false);
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

    private void LayoutBattle()
    {
        // Layout monsters in a grid
        int numRows = 2; // Number of rows in the grid
        int numCols = Mathf.CeilToInt((float)_monsters.Count / numRows); // Number of columns in the grid
        float spacing = 2.0f; // Spacing between monsters

        for (int i = 0; i < _monsters.Count; i++)
        {
            var monsterPrefab = _monsters[i];
            int row = i / numCols;
            int col = i % numCols;

            Vector3 positionOffset = new Vector3(col * spacing, 0, row * spacing);
            var monsterInstance = Instantiate(monsterPrefab, _battleStartingPosition + positionOffset, Quaternion.identity);
            monsterInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _monsters[i] = monsterInstance; // Update the list with the instantiated monster
        }
    }

    private void EncounterStart()
    {
        Debug.Log("Encounter manager: encounter started!");
        EncounterEventNotifier.EncounterStart();



        MoveAndEnableBattleCam(_battleStartingPosition);

        _battleStartingPosition = partyManager.transform.position;
        var camPosition = _battleStartingPosition + new Vector3(0, 15, 0); // Raise the camera above starting point
        MoveAndEnableBattleCam(camPosition);
        battleMenuPanel.SetActive(true);

        // Disable the parent WanderingMonster object
        if (_wanderingMonsterParent != null)
        {
            _wanderingMonsterParent.SetActive(false);
        }
    }

    public void HandleAttack()
    {
        EndBattle();

        // if the selected monster is dead, then select the next or end battle if all are dead
        //if (_selectedMonster == null)
        //{
        //    if (_monsters.Count > 0 && _monsters is not null)
        //    {
        //        _selectedMonster = _monsters.FirstOrDefault();
        //    }
        //    else
        //    {
        //        EndBattle();
        //    }
        //}

        //var monsterManager = _selectedMonster.GetComponent<CreatureBattleHelper>();
        //monsterManager.TakeDamage(10);

        //if (monsterManager.IsMonsterDead())
        //{
        //    _monsters.Remove(_selectedMonster);
        //    Destroy(_selectedMonster);
        //    if (_monsters.Count <= 0)
        //    {
        //        EndBattle();
        //    }
        //}
    }

    public void EndBattle()
    {
        // Destroy each monster GameObject
        foreach (var monster in _monsters)
        {
            Destroy(monster);
        }

        // Clear the list of monsters
        _monsters.Clear();

        // Disable the battle camera and hide the battle menu
        DisableBattleCam();
        battleMenuPanel.SetActive(false);

        // Re-enable the parent WanderingMonster object
        if (_wanderingMonsterParent != null)
        {
            _wanderingMonsterParent.SetActive(true);
        }

        // Notify that the encounter has ended
        EncounterEventNotifier.EncounterEnd();
    }

    /// <summary>
    /// Moves battle cam and enables it - the camera is always pointing straight down.
    /// </summary>
    /// <param name="position">The coordinates to move the camera to.</param>
    public void MoveAndEnableBattleCam(Vector3 position)
    {
        battleCam.SetActive(true);
        var transform = battleCam.GetComponent<Transform>();
        transform.position = position;
    }

    private void DisableBattleCam()
    {
        battleCam.SetActive(false);
    }

    private void PositionMonsters(List<GameObject> monsters)
    {

    }

    private void PositionPlayerParty(List<GameObject> partyMembers)
    {

    }
}
