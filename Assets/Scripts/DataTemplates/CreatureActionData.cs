using UnityEngine;

[CreateAssetMenu(fileName = "CreatureActionData", menuName = "Scriptable Objects/CreatureActionData")]
public class CreatureActionData : ScriptableObject
{
    public ICreature source;
    public ICreature target;
    public ClericSpellData clericSpell;
    public MageSpellData mageSpell;
    public ItemData item;
}
