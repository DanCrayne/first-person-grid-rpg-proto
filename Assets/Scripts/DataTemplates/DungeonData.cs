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
    public MonsterSpawner[] monsterSpawners;
}
