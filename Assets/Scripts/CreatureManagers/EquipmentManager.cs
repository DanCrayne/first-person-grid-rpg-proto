using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public EquipmentSlot defaultWeaponSlot; // TODO: allow for multiple weapons
    public Dictionary<EquipmentSlot, ItemData> equipment = new Dictionary<EquipmentSlot, ItemData>();

    public ItemData GetEquippedItem(EquipmentSlot slot)
    {
        if (equipment.ContainsKey(slot))
        {
            return equipment[slot];
        }
        return null;
    }

    public void EquipItem(ItemData item)
    {
        if (item.weaponData != null)
        {
            EquipWeapon(item);
        }
        else if (item.armorData != null)
        {
            EquipArmor(item);
        }
    }

    public void UnequipItem(EquipmentSlot slot)
    {
        if (equipment.ContainsKey(slot))
        {
            equipment.Remove(slot);
        }
    }

    public ItemData GetEquippedWeapon()
    {
        // TODO: allow for multiple weapons
        return GetEquippedItem(defaultWeaponSlot);
    }

    public IEnumerable<ItemData> GetEquippedArmor()
    {
        return GlobalGameObjectConfiguration.instance.validArmorEquipmentSlots.Select(slot => GetEquippedItem(slot));
    }

    public void EquipWeapon(ItemData weapon)
    {
        equipment[defaultWeaponSlot] = weapon;
    }

    private bool CanEquipWeapon(ItemData weapon)
    {
        // TODO: class-based restrictions
        return true;
    }

    public void UnequipWeapon()
    {
        equipment.Remove(defaultWeaponSlot);
    }

    public void EquipArmor(ItemData armor)
    {
        // todo
    }

    private bool CanEquipArmor(ItemData armor)
    {
        // todo
        return true;
    }

    public void UnequipArmor()
    {
        // todo
    }
}
