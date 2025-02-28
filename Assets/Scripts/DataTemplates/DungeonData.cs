using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeonData", menuName = "Dungeon/Create New Dungeon Data")]
public class DungeonData : ScriptableObject
{
    public string dungeonName;
    public string dungeonDescription;
    public string dungeonObjectName;
    public string dungeonSceneName;
    public int dungeonLevel;
    public int dungeonSize;
    public int dungeonDifficulty;
    public GameObject entrance;
    public GameObject exit;
    public PartyData[] partiesInDungeon; // e.g. parties of wandering monsters appearing here
    public DungeonAreaData[] dungeonAreas; // not used yet... but could be used for defining specfic rooms, etc
}
