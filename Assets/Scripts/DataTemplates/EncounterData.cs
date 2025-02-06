using UnityEngine;

[CreateAssetMenu(fileName = "NewEncounterData", menuName = "Scriptable Objects/EncounterData")]
public class EncounterData : ScriptableObject
{
    public string encounterName;
    public string encounterDescription;
    public string encounterObjectName;
}
