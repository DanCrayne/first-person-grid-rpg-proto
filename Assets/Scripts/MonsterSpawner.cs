using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public MonsterData monsterData;

    public GameObject SpawnMonster(Transform parent, Vector3 position)
    {
        if (monsterData == null || monsterData.MonsterPrefab == null)
        {
            Debug.LogError("MonsterData or monsterPrefab is missing.");
            return null;
        }

        GameObject monster = Instantiate(monsterData.MonsterPrefab, position, Quaternion.identity, parent);
        Monster monsterComponent = monster.GetComponent<Monster>();

        if (monsterComponent != null)
        {
            monsterComponent.monsterData = monsterData;
        }

        return monster;
    }
}
