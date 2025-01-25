using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public string partyName;
    public List<Creature> members;
    public EncounterManager encounterManager;
    public TurnBasedPlayerInputHandler playerInputHandler;
    public GameObject partyCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"{other.name} GameObject collided with party!");
            encounterManager.NotifyOfAiCollision(transform.position, other.gameObject);
            encounterManager.StartBattle(transform.position);
        }
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
        playerInputHandler.DisableControls();
    }

    public void EnableControls()
    {
        playerInputHandler.EnableControls();
    }
}
