using UnityEngine;

[CreateAssetMenu(fileName = "NewCreatureStaticData", menuName = "Creature Static Data/Create New Creature Static Data")]
public class CreatureStaticData : ScriptableObject
{
    public string creatureName;
    public int armorClass;
    public int hitDice;
    public int maxMovementInFeet;
    public int morale;
    public int xp;
    public char alignment = 'N';
    public char treasureType = 'U';
    public int minNumberAppearing = 1;
    public int maxNumberAppearing = 4;
    public bool shouldDestroyOnDeath = true;
    public bool isPlayerControlled = false;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public GameObject creaturePrefab;

    public ClassData classData;
    public RaceData raceData;

    public ItemData[] defaultInventory;
    public ItemData[] defaultEquipment;
}
