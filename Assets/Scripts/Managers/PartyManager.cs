using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public string partyName;
    public List<Creature> members;
    public EncounterManager encounterManager;
    public TurnBasedPlayerInputHandler playerInputHandler;
    public GameObject partyCamera;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"{other.name} GameObject collided with party!");
            EncounterEventNotifier.MonsterCollision();
        }
    }

    private void OnEncounterStart()
    {
        Debug.Log("PartyManager OnEncounterStart");
        DisableCameraAndControls();
    }

    private void OnEncounterEnd()
    {
        Debug.Log("PartyManager OnEncounterEnd");
        EnableCameraAndControls();
    }

    public void DisableCameraAndControls()
    {
        DisableCamera();
        DisableControls();
    }

    public void EnableCameraAndControls()
    {
        EnableCamera();
        EnableControls();
    }

    public void DisableCamera()
    {
        partyCamera.SetActive(false);
    }

    public void EnableCamera()
    {
        partyCamera.SetActive(true);
    }

    public void DisableControls()
    {
        //playerInputHandler.DisableControls();
    }

    public void EnableControls()
    {
        playerInputHandler.EnableControls();
    }
}
