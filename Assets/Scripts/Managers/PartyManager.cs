using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject encounterCharacterInfoPanelPrefab;
    public Transform partyPanel;

    private string partyName;
    private List<GameObject> encounterCharacterInfoPanels = new List<GameObject>();
    private List<Character> partyMembers = new List<Character>();

    public void Start()
    {
        CreateExampleParty();
    }

    public void AddCharacterToParty(CharacterData characterData)
    {
        var character = new Character(characterData);
        partyMembers.Add(character);
        var characterPanel = InstantiateEncounterCharacterInfoPrefab(partyPanel, encounterCharacterInfoPanelPrefab);
        var encounterCharacterInfo = characterPanel.GetComponent<EncounterCharacterInfo>();
        encounterCharacterInfo.SetCharacterInfo(characterData.characterName, characterData.hitPoints);
        encounterCharacterInfoPanels.Add(characterPanel);
    }

    public void RemoveCharacterFromParty(Character character)
    {
        partyMembers.Remove(character);
    }

    public void SetPartyName(string name)
    {
        partyName = name;
    }

    private void CreateExampleParty()
    {
        var exampleCharacters = ExampleData.CreateExampleCharacters();
        foreach (var characterData in exampleCharacters)
        {
            AddCharacterToParty(characterData);
        }
    }

    /// <summary>
    /// Instantiates the <see cref="EncounterCharacterInfo"/> prefab and adds it to the party panel UI
    /// </summary>
    /// <param name="parent">The <see cref="Transform"> to set as the parent of the instance</param>
    /// <param name="infoPanelPrefab">The prefab <see cref="GameObject"/> to instantiate</param>
    /// <returns>The instantiated <see cref="GameObject"/></returns>
    private GameObject InstantiateEncounterCharacterInfoPrefab(Transform parent, GameObject infoPanelPrefab)
    {
        return Instantiate(infoPanelPrefab, parent);
    }
}

public class ExampleData
{
    public static List<CharacterData> CreateExampleCharacters()
    {
        List<CharacterData> characters = new List<CharacterData>();

        CharacterData aragorn = ScriptableObject.CreateInstance<CharacterData>();
        aragorn.characterName = "Aragorn";
        aragorn.race = ScriptableObject.CreateInstance<RaceData>();
        aragorn.race.raceName = "Human";
        aragorn.characterClass = ScriptableObject.CreateInstance<ClassData>();
        aragorn.characterClass.className = "Ranger";
        aragorn.background = "Wandering Warrior";
        aragorn.level = 10;
        aragorn.experiencePoints = 15000;
        aragorn.hitPoints = 85;
        aragorn.maxHitPoints = 85;
        aragorn.armorClass = 16;
        aragorn.movementSpeed = 30;
        aragorn.initiative = 3;
        aragorn.alignment = 'G';
        aragorn.strength = 18;
        aragorn.dexterity = 14;
        aragorn.constitution = 16;
        aragorn.intelligence = 12;
        aragorn.wisdom = 13;
        aragorn.charisma = 15;
        aragorn.breathSavingThrow = 10;
        aragorn.poisonDeathSavingThrow = 8;
        aragorn.paralyzationSavingThrow = 7;
        aragorn.wandsSavingThrow = 9;
        aragorn.spellsSavingThrow = 11;
        aragorn.inventory = new ItemData[]
        {
        ScriptableObject.CreateInstance<ItemData>(),
        ScriptableObject.CreateInstance<ItemData>(),
        ScriptableObject.CreateInstance<ItemData>()
        };
        aragorn.inventory[0].itemName = "Sword";
        aragorn.inventory[0].description = "A sharp blade.";
        aragorn.inventory[0].weightInPounds = 5;
        aragorn.inventory[0].valueInGold = 50;
        aragorn.inventory[0].isConsumable = false;

        aragorn.inventory[1].itemName = "Shield";
        aragorn.inventory[1].description = "A sturdy shield.";
        aragorn.inventory[1].weightInPounds = 8;
        aragorn.inventory[1].valueInGold = 40;
        aragorn.inventory[1].isConsumable = false;

        aragorn.inventory[2].itemName = "Healing Potion";
        aragorn.inventory[2].description = "Restores health.";
        aragorn.inventory[2].weightInPounds = 1;
        aragorn.inventory[2].valueInGold = 10;
        aragorn.inventory[2].isConsumable = true;

        aragorn.prefab = null;

        characters.Add(aragorn);

        CharacterData legolas = ScriptableObject.CreateInstance<CharacterData>();
        legolas.characterName = "Legolas";
        legolas.race = ScriptableObject.CreateInstance<RaceData>();
        legolas.race.raceName = "Elf";
        legolas.characterClass = ScriptableObject.CreateInstance<ClassData>();
        legolas.characterClass.className = "Archer";
        legolas.background = "Forest Dweller";
        legolas.level = 9;
        legolas.experiencePoints = 14000;
        legolas.hitPoints = 75;
        legolas.maxHitPoints = 75;
        legolas.armorClass = 18;
        legolas.movementSpeed = 35;
        legolas.initiative = 4;
        legolas.alignment = 'G';
        legolas.strength = 14;
        legolas.dexterity = 20;
        legolas.constitution = 15;
        legolas.intelligence = 13;
        legolas.wisdom = 12;
        legolas.charisma = 14;
        legolas.breathSavingThrow = 9;
        legolas.poisonDeathSavingThrow = 7;
        legolas.paralyzationSavingThrow = 8;
        legolas.wandsSavingThrow = 10;
        legolas.spellsSavingThrow = 12;
        legolas.inventory = new ItemData[]
        {
        ScriptableObject.CreateInstance<ItemData>(),
        ScriptableObject.CreateInstance<ItemData>(),
        ScriptableObject.CreateInstance<ItemData>()
        };
        legolas.inventory[0].itemName = "Bow";
        legolas.inventory[0].description = "A long-range weapon.";
        legolas.inventory[0].weightInPounds = 3;
        legolas.inventory[0].valueInGold = 70;
        legolas.inventory[0].isConsumable = false;

        legolas.inventory[1].itemName = "Quiver of Arrows";
        legolas.inventory[1].description = "Arrows for the bow.";
        legolas.inventory[1].weightInPounds = 2;
        legolas.inventory[1].valueInGold = 30;
        legolas.inventory[1].isConsumable = false;

        legolas.inventory[2].itemName = "Elven Cloak";
        legolas.inventory[2].description = "Provides stealth.";
        legolas.inventory[2].weightInPounds = 1;
        legolas.inventory[2].valueInGold = 100;
        legolas.inventory[2].isConsumable = false;

        legolas.prefab = null;

        characters.Add(legolas);

        return characters;
    }
}
