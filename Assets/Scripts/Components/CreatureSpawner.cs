using Unity.VisualScripting;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public CreatureStaticData creatureData;

    /// <summary>
    /// Spawns a creature at the given position
    /// </summary>
    /// <param name="parent">The <see cref="GameObject"/> to use as the creature's parent</param>
    /// <param name="position">The position relative to the parent to spawn the creature</param>
    /// <returns>The spawned creature <see cref="GameObject"/></returns>
    public GameObject SpawnCreature(Transform parent, Vector3 position)
    {
        var creatureInstance = Instantiate(creatureData.creaturePrefab, position, Quaternion.identity);
        var creatureComponent = creatureInstance.GetOrAddComponent<Creature>();
        creatureComponent.creatureStaticData = creatureData;

        creatureComponent.RollAndSetRandomStats();
        if (creatureComponent.creatureStaticData != null)
        {
            AddDefaultInventoryAndEquipmentToCreature(creatureComponent.creatureStaticData, creatureComponent);
            creatureComponent.SetName(creatureComponent.creatureStaticData.creatureName);
        }

        return creatureInstance;
    }

    private void AddDefaultInventoryAndEquipmentToCreature(CreatureStaticData creatureStaticData, Creature creatureInstance)
    {
        if (creatureStaticData.defaultInventory != null)
        {
            foreach (var item in creatureStaticData.defaultInventory)
            {
                creatureInstance.AddItemToInventory(item);
            }
        }

        // Equip default equipment (both armor and weapon)
        if (creatureStaticData.defaultEquipment != null)
        {
            foreach (var item in creatureStaticData.defaultEquipment)
            {
                if (item.armorData != null)
                {
                    creatureInstance.EquipArmor(item);
                }
                else if (item.weaponData != null)
                {
                    creatureInstance.EquipWeapon(item);
                }
            }
        }
    }
}
