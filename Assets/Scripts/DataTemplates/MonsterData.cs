using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster/Create New Monster")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public RaceData race;
    public ClassData characterClass;
    public string background;

    public int armorClass;
    public int hitDice;
    public int maxMovementInFeet;
    public int morale;
    public int xp;
    public char alignment = 'N';
    public char treasureType = 'U';
    public int minNumberAppearing = 1;
    public int maxNumberAppearing = 4;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public GameObject monsterPrefab;

    public ItemData[] inventory;
    public WeaponData equippedWeapon;
    public ArmorData[] equippedArmor;
}
