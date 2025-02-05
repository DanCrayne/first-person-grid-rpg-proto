using UnityEngine;

[CreateAssetMenu(fileName = "NewActionType", menuName = "Action/New Action Type")]
public class ActionTypes : ScriptableObject
{
    public string actionName;
    public AttackTypeData[] associatedAttackTypes;
    public MageSpellData[] associatedMageSpells;
    public ClericSpellData[] associatedClericSpells;
}
