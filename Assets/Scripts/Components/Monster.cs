using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    private int currentHealth;

    void Start()
    {
        if (monsterData != null)
        {
            currentHealth = RollHp();
            gameObject.name = monsterData.Name;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private int RollHp()
    {
        return Random.Range(1, (monsterData.HitDice * 6) + 1);
    }
}