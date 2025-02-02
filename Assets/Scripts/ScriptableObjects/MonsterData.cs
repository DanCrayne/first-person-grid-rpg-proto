using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    public string Name;
    public int ArmorClass;
    public int HitDice;
    public int MaxMovementInFeet;
    public int Morale;
    public int Xp;
    public char Alignment = 'N';
    public char TreasureType = 'U';
    public int MinNumberAppearing = 1;
    public int MaxNumberAppearing = 4;
    public AttackType[] Attacks;
    public GameObject MonsterPrefab;
}

