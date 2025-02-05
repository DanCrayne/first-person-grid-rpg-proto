using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int armorClass;
    public int hitDice;
    public int maxMovementInFeet;
    public int morale;
    public int xp;
    public char alignment = 'N';
    public char treasureType = 'U';
    public int minNumberAppearing = 1;
    public int maxNumberAppearing = 4;
    public AttackTypeData[] attacks;
    public ItemData[] items;
    public GameObject monsterPrefab;
}

