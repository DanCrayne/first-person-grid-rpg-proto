using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the party of characters in the game
/// </summary>
public class PartyManager : MonoBehaviour
{
    // TODO: Delete - for example party only
    //public GameObject playerCharacterPrefab;
    //public GameObject mageCharacterPrefab;
    public CreatureStaticData defaultCharacterData;
    public ClassData fighterClassData;
    public ClassData mageClassData;
    public RaceData dwarfRaceData;
    public RaceData humanRaceData;

    public ItemData defaultWeaponData;
    public ItemData defaultArmorData;
    public EquipmentSlot defaultWeaponSlot;

    public Transform partyGameObject;
    public List<Creature> partyMembers = new List<Creature>();

    private void Start()
    {
        CreateExampleParty();
    }

    public void CreateExampleParty()
    {
        var pcPrefab = defaultCharacterData.creaturePrefab;

        foreach (var (prefab, name, classData, raceData) in new[] {
            (pcPrefab, "Balin", fighterClassData, dwarfRaceData),
            (pcPrefab, "Gimli", fighterClassData, dwarfRaceData),
            (pcPrefab, "Gandalf", mageClassData, humanRaceData)})
        {
            InstantiateExampleCharacterAndAddToParty(prefab, name, classData, raceData);
        }
    }

    public void InstantiateExampleCharacterAndAddToParty(GameObject prefab, string name, ClassData classData, RaceData raceData)
    {
        var character = Instantiate(prefab, partyGameObject);
        var creature = character.GetComponent<Creature>();
        creature.classData = classData;
        creature.raceData = raceData;
        creature.RollAndSetRandomStats();
        creature.SetName(name);
        creature.shouldDestroyOnDeath = false;

        var inventoryManager = character.GetComponent<InventoryManager>();
        inventoryManager.AddItem(defaultWeaponData);

        var equipmentManager = character.GetComponent<EquipmentManager>();
        equipmentManager.EquipWeapon(defaultWeaponData);
        equipmentManager.EquipArmor(defaultArmorData);

        partyMembers.Add(creature);
    }

    public bool IsPartyWiped()
    {
        return !partyMembers.Any(c => c.GetHitPoints() > 0);
    }

    public List<Creature> GetPartyMembers()
    {
        return partyMembers;
    }

    public void RemoveCharacterFromParty(Creature character)
    {
        partyMembers.Remove(character);
    }
}

