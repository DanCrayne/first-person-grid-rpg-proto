using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Scriptable Objects/Creature")]
public class Creature : ScriptableObject
{
    public string creatureName;
    public int maxHp;
    public int currentHp;
    public int attack;
    public int defense;
    public int speed;
    public int level;
    public int experience;
}
