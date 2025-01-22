using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public string partyName;
    public List<Creature> members;
    public EncounterManager encounterManager;
    public PlayerInputHandler playerInputHandler;
    public GameObject partyCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

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
        playerInputHandler.DisableControls();
    }

    public void EnableCameraAndControls()
    {
        EnableCamera();
        playerInputHandler.EnableControls();
    }

    public void DisableCamera()
    {
        partyCamera.SetActive(false);
    }

    public void EnableCamera()
    {
        partyCamera.SetActive(true);
    }
}
