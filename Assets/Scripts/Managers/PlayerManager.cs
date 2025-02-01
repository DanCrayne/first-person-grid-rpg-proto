using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject playerObject;
    public TurnBasedPlayerInputHandler playerInputHandler;

    private bool hasEncounterStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasEncounterStarted)
        {
            Debug.Log($"{other.name} GameObject collided with party!");
            hasEncounterStarted = true;
            EncounterEventNotifier.MonsterCollision(this.gameObject, other.gameObject);
        }
    }

    private void OnEncounterStart()
    {
        Debug.Log("PartyManager OnEncounterStart");
        //DisableCameraAndControls();
    }

    private void OnEncounterEnd()
    {
        Debug.Log("PartyManager OnEncounterEnd");
        hasEncounterStarted = false;
        //EnableCameraAndControls();
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
        playerCamera.SetActive(false);
    }

    public void EnableCamera()
    {
        playerCamera.SetActive(true);
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
