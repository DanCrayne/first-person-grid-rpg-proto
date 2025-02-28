using UnityEngine;

[CreateAssetMenu(fileName = "DungeonAreaData", menuName = "Scriptable Objects/DungeonAreaData")]
public class DungeonAreaData : ScriptableObject
{
    public string areaName;
    public PartyData[] partiesInArea; // e.g. parties of wandering monsters
}
