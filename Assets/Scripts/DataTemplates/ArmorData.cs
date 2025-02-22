using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorData", menuName = "Armor/Create New Armor")]
public class ArmorData : ScriptableObject
{
    public ArmorType armorType;
    public int armorClass = 0;
    public EquipmentSlot slotWhereEquipped;
}
