using UnityEngine;

public enum CreatureActionType
{
    ATTACK, DEFEND, ITEM, CAST, FLEE
}

public class CreatureAction
{
    public ICreature source;
    public ICreature target;
    public CreatureActionType actionType;
    public AttackTypeData attackType;
    public ClericSpellData spellData;
    public MageSpellData mageSpellData;
    public ItemData itemData;
}
