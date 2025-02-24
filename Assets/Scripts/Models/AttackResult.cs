using UnityEngine;

public class AttackResult
{
    public Creature source;
    public Creature target;
    public int damage;
    public bool didHit;
    public string resultMessage;

    public AttackResult(Creature source, Creature target, int damage, bool didHit, string resultMessage)
    {
        this.source = source;
        this.target = target;
        this.damage = damage;
        this.didHit = didHit;
        this.resultMessage = resultMessage;
    }
}
