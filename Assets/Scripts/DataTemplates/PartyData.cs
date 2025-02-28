using UnityEngine;

[CreateAssetMenu(fileName = "PartyData", menuName = "Scriptable Objects/PartyData")]
public class PartyData : ScriptableObject
{
    public string partyName;
    public CreatureStaticData[] creatures;
    public CreatureSpawner[] creatureSpawners;
}
