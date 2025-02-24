using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
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
            if (CanEquipWeapon(item))
            {
                UnequipCurrentWeapon();
                EquipWeapon(item);
            }
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
        foreach (var item in equipment)
        {
            if (item.Value.weaponData != null)
            {
                return item.Value;
            }
        }

        return null;
    }

    public IEnumerable<ItemData> GetEquippedArmor()
    {
        return GlobalGameObjectConfiguration.instance.validArmorEquipmentSlots.Select(slot => GetEquippedItem(slot));
    }

    public void EquipWeapon(ItemData weapon)
    {
        if (!CanEquipWeapon(weapon))
        {
            return;
        }

        if (weapon.weaponData != null)
        {
            if (weapon.weaponData.slotWhereEquipped != null)
            {
                equipment[weapon.weaponData.slotWhereEquipped] = weapon;
            }
        }
        else
        {
            Debug.LogError($"Could not equip weapon {weapon.name}");
        }
    }

    private ClassData GetCharacterClass()
    {
        var creature = this.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            return creature.classData;
        }

        return null;
    }

    public void UnequipCurrentWeapon()
    {
        if (GetEquippedWeapon() != null)
        {
            UnequipWeapon(GetEquippedWeapon());
        }
    }

    public void UnequipWeapon(ItemData item)
    {
        if (item.weaponData != null)
        {
            equipment.Remove(FindWhereWeaponIsEquipped(item));
        }
    }

    private EquipmentSlot FindWhereWeaponIsEquipped(ItemData item)
    {
        return equipment.FirstOrDefault(e => e.Value == item).Key;
    }

    public void EquipArmor(ItemData armor)
    {
        if (!CanEquipArmor(armor))
        {
            Debug.Log($"Character can't equip {armor}");
            return;
        }

        var slotWhereEquipped = armor.armorData?.slotWhereEquipped;

        if (slotWhereEquipped != null)
        {
            equipment.Add(slotWhereEquipped, armor);
        }
        else
        {
            Debug.Log($"Could not equip armor {armor} since armor slot where equipped was not given");
        }
    }

    public void UnequipArmor(ItemData item)
    {
        var slotToEquip = item.armorData?.slotWhereEquipped;

        if (slotToEquip != null)
        {
            equipment.Remove(slotToEquip);
        }
        else
        {
            Debug.Log($"Could not unequip armor {item}");
        }
    }

    private bool CanEquipWeapon(ItemData weapon)
    {
        var classData = GetCharacterClass();
        if (classData == null)
        {
            // if no class is set (maybe a monster) then assume equipping is fine
            return true;
        }

        var weaponType = weapon.weaponData?.weaponType;
        var canEquip = true;

        if (weaponType != null)
        {
            // if we didn't find this weapon type in the restricted types, then it's ok to wield
            canEquip = !classData.restrictedWeaponTypes.Any(w => w != weaponType);
        }

        return canEquip;
    }

    private bool CanEquipArmor(ItemData armor)
    {
        var classData = GetCharacterClass();
        if (classData == null)
        {
            // if no class is set (maybe a monster) then assume equipping is fine
            return true;
        }

        var armorType = armor.armorData?.armorType;
        var canEquip = true;

        if (armorType != null)
        {
            // if we didn't find this armorType in the restricted types, then it's ok to wear
            canEquip = !classData.restrictedArmorTypes.Any(w => w != armorType);
        }

        return canEquip;
    }
}
