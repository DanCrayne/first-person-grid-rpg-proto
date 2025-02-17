using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "Attack Type/Create New Attack Type")]
public class AttackTypeData : ScriptableObject
{
    public string attackName;
    public int numberOfAttacks;
}
