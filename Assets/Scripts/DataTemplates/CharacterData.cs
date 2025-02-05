using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public RaceData race;
    public ClassData characterClass;
    public string background;

    public int level;
    public int experiencePoints;
    public int hitPoints;
    public int maxHitPoints;
    public int armorClass;
    public int movementSpeed;
    public int initiative;
    public char alignment = 'N';

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public int breathSavingThrow;
    public int poisonDeathSavingThrow;
    public int paralyzationSavingThrow;
    public int wandsSavingThrow;
    public int spellsSavingThrow;

    public ItemData[] inventory;
    public GameObject prefab;
}
