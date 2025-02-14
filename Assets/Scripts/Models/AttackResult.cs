using UnityEngine;

public class AttackResult
{
    public bool didAttackHit;
    public string attackName;
    public int damage;

    public AttackResult(bool didAttackHit, string attackName, int randomDamage)
    {
        this.didAttackHit = didAttackHit;
        this.attackName = attackName;
        this.damage = randomDamage;
    }
}
