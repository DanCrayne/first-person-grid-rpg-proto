using System.Collections.Generic;
using UnityEngine;

public class GlobalGameObjectConfiguration : MonoBehaviour
{
    public List<EquipmentSlot> validArmorEquipmentSlots;
    public List<EquipmentSlot> validWeaponEquipmentSlots;

    public static GlobalGameObjectConfiguration instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
