using UnityEngine;
//using GridRpgLibraries;

public class MonsterManager : MonoBehaviour
{
    public Creature Creature;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Creature.currentHp = Creature.maxHp;
        Debug.Log($"{Creature.creatureName} spawned with {Creature.currentHp} health!");
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Max(damage - Creature.defense, 0);
        Creature.currentHp -= damageTaken;
        Debug.Log($"{Creature.creatureName} took {damageTaken} damage. Health: {Creature.currentHp}/{Creature.maxHp}");

        if (Creature.currentHp <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHp()
    {
        return Creature.currentHp;
    }

    public bool IsMonsterDead()
    {
        if (Creature.currentHp <= 0)
            return true;

        return false;
    }

    private void Die()
    {
        Debug.Log($"{Creature.creatureName} has been defeated!");
        Destroy(gameObject);
    }
}
