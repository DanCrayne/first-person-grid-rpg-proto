using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "Attack Type/Create New Attack Type")]
public class AttackTypeData : ScriptableObject
{
    public string attackName;
    public int minDamage;
    public int maxDamage;
    public int numberOfAttacks;
    public string damageDice = "1d6"; // use this instead of MinDamage, MaxDamage, and NumberOfAttacks?
}
