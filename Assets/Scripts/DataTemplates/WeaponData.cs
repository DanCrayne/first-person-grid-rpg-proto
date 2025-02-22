using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int minDamage;
    public int maxDamage;
    public AttackTypeData[] attackTypeData;
    public WeaponType weaponType;
    public EquipmentSlot slotWhereEquipped;
}
