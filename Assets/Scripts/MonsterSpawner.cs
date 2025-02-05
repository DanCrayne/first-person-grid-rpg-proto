using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public MonsterData monsterData;

    public MonsterSpawner(MonsterData monster)
    {
        this.monsterData = monster;
    }

    public GameObject SpawnMonster(Transform parent, Vector3 position)
    {
        if (monsterData == null || monsterData.monsterPrefab == null)
        {
            Debug.LogError("MonsterData or monsterPrefab is missing.");
            return null;
        }

        GameObject monster = Instantiate(monsterData.monsterPrefab, position, Quaternion.identity, parent);
        Monster monsterComponent = monster.GetComponent<Monster>();

        if (monsterComponent != null)
        {
            monsterComponent.monsterData = monsterData;
        }

        return monster;
    }
}
