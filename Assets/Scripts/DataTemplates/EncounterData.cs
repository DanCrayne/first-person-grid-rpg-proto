using UnityEngine;

[CreateAssetMenu(fileName = "NewEncounterData", menuName = "Scriptable Objects/EncounterData")]
public class EncounterData : ScriptableObject
{
    public string encounterName;
    public string encounterDescription;
    public GameObject encounterPrefab;
    public string encounterSceneName;
    public string encounterObjectName;
}
