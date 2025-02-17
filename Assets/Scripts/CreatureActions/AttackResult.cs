public class AttackResult
{
    public bool didAttackHit;
    public string attackName;
    public int damage;
    public AttackTypeData attackTypeData;

    public AttackResult(bool didAttackHit, AttackTypeData attackTypeData, int randomDamage)
    {
        this.didAttackHit = didAttackHit;
        this.attackTypeData = attackTypeData;
        this.damage = randomDamage;
    }
}
