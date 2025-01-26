using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject battleCam;
    public GameObject currentlySelectedMonster;

    private Vector3 battleStartingPosition;
    private List<GameObject> monsters = new List<GameObject>();

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

    private void EncounterStart()
    {
        Debug.Log("Encounter manager: encounter started!");
        EncounterEventNotifier.EncounterStart();
        MoveAndEnableBattleCam(battleStartingPosition);

        battleStartingPosition = partyManager.transform.position;
        var camPosition = battleStartingPosition + new Vector3(0, 15, 0); // Raise the camera above starting point
        MoveAndEnableBattleCam(camPosition);
        battleMenuPanel.SetActive(true);
    }

    public void HandleAttack()
    {
        // if the selected monster is dead, then select the next or end battle if all are dead
        if (currentlySelectedMonster == null)
        {
            if (monsters.Count > 0 && monsters is not null)
            {
                currentlySelectedMonster = monsters.FirstOrDefault();
            }
            else
            {
                EndBattle();
            }
        }

        var monsterManager = currentlySelectedMonster.GetComponent<CreatureBattleHelper>();
        monsterManager.TakeDamage(10);

        if (monsterManager.IsMonsterDead())
        {
            monsters.Remove(currentlySelectedMonster);
            if (monsters.Count <= 0)
            {
                EndBattle();
            }
        }
    }

    public void EndBattle()
    {
        DisableBattleCam();
        battleMenuPanel.SetActive(false);
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
