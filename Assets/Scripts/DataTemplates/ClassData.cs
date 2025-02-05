using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "Class/Create New Class")]
public class ClassData : ScriptableObject
{
    public string className;
    public string hitDice;
    public int[] XpPerLevel;
    public int[] spellSlotsPerLevel;
}
