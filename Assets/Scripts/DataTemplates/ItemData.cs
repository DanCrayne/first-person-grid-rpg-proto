using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public int weightInPounds;
    public int valueInGold;
    public bool isConsumable;
    public bool isStackable;

    public bool isWeapon;
    public WeaponData weaponData;

    public bool isArmor;
    public ArmorData armorData;
}
