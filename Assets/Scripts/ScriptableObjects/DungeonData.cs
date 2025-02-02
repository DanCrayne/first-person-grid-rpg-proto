using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeonData", menuName = "Dungeon/Create New Dungeon Data")]
public class DungeonData : ScriptableObject
{
    public List<GameObject> possibleMonsters;
}
