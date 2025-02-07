using TMPro;
using UnityEngine;

public class EncounterCharacterInfo : MonoBehaviour
{
    public TMP_Text characterNameSlot;
    public TMP_Text characterHpSlot;

    public void SetCharacterInfo(string name, int hp)
    {
        characterNameSlot.text = name;
        characterHpSlot.text = $"HP: {hp}";
    }
}
