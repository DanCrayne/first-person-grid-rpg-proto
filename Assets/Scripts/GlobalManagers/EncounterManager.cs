using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    /// <summary>
    /// The base encounter rate for the dungeon (higher is less frequent)
    /// </summary>
    public int baseEncounterRate = 10;
    private uint _playerTotalSteps = 0;
    private int _currentEncounterRate;
    private const int MOST_FREQUENT_ENCOUNTER_RATE = 5;

    private void OnEnable()
    {
        PlayerActionNotifier.OnPlayerMadeNoise += OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved += OnPlayerMoved;
        _currentEncounterRate = baseEncounterRate;
    }

    private void OnDisable()
    {
        PlayerActionNotifier.OnPlayerMadeNoise -= OnPlayerMadeNoise;
        PlayerActionNotifier.OnPlayerMoved -= OnPlayerMoved;
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
}
