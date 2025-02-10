using UnityEngine;

/// <summary>
/// Spawns a monster
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    public MonsterData monsterData;

    public MonsterSpawner(MonsterData monster)
    {
        this.monsterData = monster;
    }

    /// <summary>
    /// Spawns a monster at the given position
    /// </summary>
    /// <param name="parent">The <see cref="GameObject"/> to use as the monster's parent</param>
    /// <param name="position">The position relative to the parent to spawn the monster</param>
    /// <returns>The spawned monster <see cref="GameObject"/></returns>
    public GameObject SpawnMonster(Transform parent, Vector3 position)
    {
        if (monsterData == null || monsterData.monsterPrefab == null)
        {
            Debug.LogError("MonsterData or monsterPrefab is missing.");
            return null;
        }

        GameObject monster = Instantiate(monsterData.monsterPrefab, position, Quaternion.identity);

        Monster monsterComponent = monster.GetComponent<Monster>();

        if (monsterComponent != null)
        {
            monsterComponent.monsterData = monsterData;
        }

        return monster;
    }
}
