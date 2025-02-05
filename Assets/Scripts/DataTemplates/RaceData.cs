using UnityEngine;

[CreateAssetMenu(fileName = "NewRaceData", menuName = "Race/Create New Race")]
public class RaceData : ScriptableObject
{
    public string raceName;
    public int baseMovementInFeet;
    public uint maxAge;
    public bool hasDarkvision;
    public int baseHeightInInches;
    public int baseWeightInPounds;
    public int breathSavingThrowBonus;
    public int poisonDeathSavingThrowBonus;
    public int paralyzationSavingThrowBonus;
    public int wandsSavingThrowBonus;
    public int spellsSavingThrowBonus;
}
