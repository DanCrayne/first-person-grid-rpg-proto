using UnityEngine;

/// <summary>
/// Spawns a monster
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    public GameObject monster;

    /// <summary>
    /// Spawns a monster at the given position
    /// </summary>
    /// <param name="parent">The <see cref="GameObject"/> to use as the monster's parent</param>
    /// <param name="position">The position relative to the parent to spawn the monster</param>
    /// <returns>The spawned monster <see cref="GameObject"/></returns>
    public GameObject SpawnMonster(Transform parent, Vector3 position)
    {
        var monsterComponent = monster.GetComponent<Monster>();

        if (monsterComponent == null || monsterComponent.monsterData.monsterPrefab == null)
        {
            Debug.LogError("MonsterData or monsterPrefab is missing.");
            return null;
        }

        // instantiate monster and setup initial values
        var monsterInstance = Instantiate(monsterComponent.monsterData.monsterPrefab, position, Quaternion.identity);

        // Set initial stats for this monster
        var spawnedMonsterComponent = monsterInstance.GetComponent<Monster>();
        spawnedMonsterComponent.monsterData = monsterComponent.monsterData;
        spawnedMonsterComponent.RollAndSetStats();

        return monsterInstance;
    }
}
