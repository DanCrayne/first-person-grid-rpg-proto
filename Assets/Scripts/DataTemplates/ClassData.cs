using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "Class/Create New Class")]
public class ClassData : ScriptableObject
{
    /// <summary>
    /// The name of the class
    /// </summary>
    public string className;

    /// <summary>
    /// The hit die for the class (e.g. 6 for a d6)
    /// </summary>
    public int hitDie;

    /// <summary>
    /// The amount of experience necessary to go up a level where the index is the level
    /// </summary>
    public int[] XpPerLevel;

    /// <summary>
    /// The number of spell slots per level for this class where the first index is the level and the second index is the number of slots
    /// </summary>
    public int[][] spellSlotsPerLevel;

    public ActionData[] actionTypes;

    public EquipmentSlot[] restrictedEquipmentSlots;
    public ArmorType[] restrictedArmorTypes;
    public WeaponType[] restrictedWeaponTypes;
}
