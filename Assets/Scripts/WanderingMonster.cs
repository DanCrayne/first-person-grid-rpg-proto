using System.Collections.Generic;
using UnityEngine;

public class WanderingMonster : MonoBehaviour
{
    public int maxNumberOfMonsters;
    public List<GameObject> possibleMonsters;
    public Vector3 currentPosition;

    /// <summary>
    /// Spawns the individual monsters that will be used in an encounter
    /// based on the possible monsters that can be generated
    /// </summary>
    public void SpawnMonstersForEncounter()
    {

    }
}
