using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class CharacterData : ScriptableObject
{
    public RaceData race;
    public ClassData characterClass;
    public string background;
    public char alignment = 'N';

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public ItemData[] inventory;
    public WeaponData equippedWeapon;
    public ArmorData[] equippedArmor;

    public GameObject encounterCharacterInfoPrefab;
    public GameObject actionControlPrefab;
}
