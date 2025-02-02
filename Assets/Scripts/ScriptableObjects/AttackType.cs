using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "Attack Type/Create New Attack Type")]
public class AttackType : ScriptableObject
{
    public string Name;
    public int MinDamage;
    public int MaxDamage;
    public int NumberOfAttacks;
    public string DamageDice = "1d6"; // use this instead of MinDamage, MaxDamage, and NumberOfAttacks?
}
