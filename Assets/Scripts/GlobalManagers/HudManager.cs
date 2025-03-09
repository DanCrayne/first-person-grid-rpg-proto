using UnityEngine;

public class HudManager : MonoBehaviour
{
    public Canvas HudCanvas;

    private void OnEnable()
    {
        EncounterEventNotifier.OnEncounterStart += HideHud;
        EncounterEventNotifier.OnEncounterEnd += ShowHud;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnEncounterStart -= HideHud;
        EncounterEventNotifier.OnEncounterEnd -= ShowHud;
    }

    private void HideHud()
    {

        HudCanvas.gameObject.SetActive(false);
    }

    private void ShowHud()
    {
        HudCanvas.gameObject.SetActive(true);
    }
}
