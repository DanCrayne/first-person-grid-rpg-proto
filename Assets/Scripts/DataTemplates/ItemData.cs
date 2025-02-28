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

    public GameObject prefab;

    public WeaponData weaponData;
    public ArmorData armorData;
}
