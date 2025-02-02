using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public GameObject battleMenuPanel;
    public PartyManager partyManager;
    public GameObject encounterObject;
    public int baseEncounterRate = 10;

    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;


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
        _currentEncounterRate = baseEncounterRate + noiseLevel / 2;
    }

    private bool DetermineIfEncountered()
    {
        Debug.Log($"Determining if encounter should happen. Total steps {_playerTotalSteps} % current encounter rate {_currentEncounterRate}");
        if (_playerTotalSteps % _currentEncounterRate == 0)
        {
            return true;
        }

        return false;
    }

    public void SetupEncounter()
    {
        battleMenuPanel.SetActive(true);
    }

    public void HandleAttack()
    {
        EndBattle();
    }

    public void EndBattle()
    {
        // Disable the battle camera and hide the battle menu
        battleMenuPanel.SetActive(false);

        Debug.Log("Encounter manager: encounter ended!");

        // Notify that the encounter has ended - note this needs to be done after activating the dungeon object or else it won't get the message
        EncounterEventNotifier.EncounterEnd();
    }
}
