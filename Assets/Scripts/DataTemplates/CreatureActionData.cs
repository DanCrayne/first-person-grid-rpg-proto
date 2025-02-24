using UnityEngine;

[CreateAssetMenu(fileName = "CreatureActionData", menuName = "Scriptable Objects/CreatureActionData")]
public class CreatureActionData : ScriptableObject
{
    public Creature source;
    public Creature target;
    public ClericSpellData clericSpell;
    public MageSpellData mageSpell;
    public ItemData item;
}
