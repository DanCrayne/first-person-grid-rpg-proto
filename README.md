# Unity Grid-based RPG Prototype

## Overview

This is a Unity project that demonstrates a turn-based and grid-based RPG prototype (based on old-school RPGs like
[the Wizardry series](https://wizardry.wiki.gg/wiki/Wizardry_%28franchise%29) or [Might and Magic 1 & 2](https://en.wikipedia.org/wiki/Might_and_Magic_Book_One:_The_Secret_of_the_Inner_Sanctum)).
The project includes a simple grid movement control system, character movement, and basic combat mechanics which will be used as the groundwork for a more complex RPG game.

## Features
- **Grid Movement**: Move characters on a grid-based map using a gamepad or WASD keys.
- **Character Management**: Add and remove characters from the party.
- **UI Integration**: Display character information (name and HP) in the UI using TextMeshPro.
- **Dynamic UI**: Use layout groups to dynamically arrange character panels in the UI.
- **ScriptableObjects**: Utilize ScriptableObjects to improve performance and clarity for data that is more static in nature (e.g. classes, races, items, etc).
- **Event System**: Use Unity's event system to communicate between different components in the project.
- 

## Getting Started

### Prerequisites
- Unity 2020.3 or later
- TextMeshPro package

### Installation
1. Clone the repository:
2. Open the project in Unity.

### Usage

**Running the Project**:
 - Press Play in the Unity Editor to see the party manager in action.
 - The example characters will be displayed in the UI, and you can add or remove characters dynamically.

## Project Structure

This is the basic structure of the project to give you an idea of how the project is organized.

+---Resources
|   |   Data.meta
|   |   Materials.meta
|   |   Prefabs.meta
|   |
|   +---Data
|   |   |   AttackTypes.meta
|   |   |   Characters.meta
|   |   |   Classes.meta
|   |   |   Dungeons.meta
|   |   |   Encounters.meta
|   |   |   Monsters.meta
|   |   |   Parties.meta
|   |   |   Races.meta
|   |   |
|   |   +---AttackTypes
|   |   |       Bite.asset
|   |   |       Bite.asset.meta
|   |   |       Claw.asset
|   |   |       Claw.asset.meta
|   |   |       ...
|   |   |
|   |   +---Characters
|   |   |       Biff.asset
|   |   |       Biff.asset.meta
|   |   |       Fred.asset
|   |   |       Fred.asset.meta
|   |   |       ...
|   |   |
|   |   +---Classes
|   |   |       Fighter.asset
|   |   |       Fighter.asset.meta
|   |   |       ...
|   |   |
|   |   +---Dungeons
|   |   |       Dungeon01.asset
|   |   |       Dungeon01.asset.meta
|   |   |       ...
|   |   |
|   |   +---Encounters
|   |   |       Encounter01.asset
|   |   |       Encounter01.asset.meta
|   |   |       ...
|   |   |
|   |   +---Monsters
|   |   |       Centipede.asset
|   |   |       Centipede.asset.meta
|   |   |       Millipede.asset
|   |   |       Millipede.asset.meta
|   |   |       ...
|   |   |
|   |   +---Parties
|   |   |       ExampleParty.asset
|   |   |       ExampleParty.asset.meta
|   |   |       ...
|   |   |
|   |   \---Races
|   |           Dwarf.asset
|   |           Dwarf.asset.meta
|   |           Human.asset
|   |           Human.asset.meta
|   |           ...
|   |
|   +---Materials
|   |       CharacterSkin.mat
|   |       CharacterSkin.mat.meta
|   |       GoblinSkin.mat
|   |       GoblinSkin.mat.meta
|   |       ...
|   |
|   \---Prefabs
|           Block.prefab
|           Block.prefab.meta
|           Block_Corner.prefab
|           Block_Corner.prefab.meta
|           ...
|
+---Scenes
|       Dungeon01.unity
|       Dungeon01.unity.meta
|       Encounter.unity
|       Encounter.unity.meta
|       ...
|
+---Scripts
    |   Components.meta
    |   Constants.meta
    |   DataTemplates.meta
    |   EventHandling.meta
    |   Managers.meta
    |   Movement.meta
    |   UI.meta
    |
    +---Components
    |       Character.cs
    |       Character.cs.meta
    |       Monster.cs
    |       Monster.cs.meta
    |       ...
    |
    +---Constants
    |       LayerMaskConstants.cs
    |       LayerMaskConstants.cs.meta
    |       ...
    |
    +---DataTemplates
    |       ActionTypes.cs
    |       ActionTypes.cs.meta
    |       ArmorData.cs
    |       ArmorData.cs.meta
    |       ...
    |
    +---EventHandling
    |       EncounterEventNotifier.cs
    |       EncounterEventNotifier.cs.meta
    |       GeneralNotifier.cs
    |       GeneralNotifier.cs.meta
    |       ...
    |
    +---Managers
    |       EncounterManager.cs
    |       EncounterManager.cs.meta
    |       GameManager.cs
    |       GameManager.cs.meta
    |       ...
    |
    +---Movement
    |       TurnBasedPlayerInputHandler.cs
    |       TurnBasedPlayerInputHandler.cs.meta
    |       ...
    |
    \---UI
            EncounterCharacterInfo.cs
            EncounterCharacterInfo.cs.meta
            ...

### Project Organization

The key directories 

## Scripts

### PartyManager.cs
Manages the party of characters, including adding and removing characters, and updating the UI.

### ExampleData.cs
Provides example character data for testing purposes.

### EncounterCharacterInfo.cs
Handles the display of character information in the UI.

### CharacterData.cs
Defines the data structure for a character using ScriptableObject.

### ClassData.cs
Defines the data structure for a character class using ScriptableObject.

### ItemData.cs
Defines the data structure for an item using ScriptableObject.

### RaceData.cs
Defines the data structure for a character race using ScriptableObject.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements
- [Unity](https://unity.com/)
- [TextMeshPro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)

