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

    /// <summary>
    /// The default actions for all classes
    /// </summary>
    public string [] defaultActions = new [] { "Attack", "Defend",  "Item", "Flee" };

    /// <summary>
    /// The actions that are unique to this class
    /// </summary>
    public string [] classActions;

    /// <summary>
    /// The spells that are unique to this class
    /// </summary>
    public string [] spellList;

    /// <summary>
    /// The prefab for the action control for this class (e.g. the buttons for the actions)
    /// </summary>
    public GameObject actionControlPrefab;
}
