using System;

public class MageSpellAction : ICreatureAction
{
    public Creature source;
    public Creature target;
    public MageSpellData spellData;

    public Func<string>[] Perform()
    {
        // TODO: implement spell effects
        bool didSpellWork = true;
        var damage = 5;
        var resultMessage = $"{source.GetName()} casts {spellData.name} to no effect!";

        if (didSpellWork)
        {
            resultMessage = $"{source.GetName()} casts {spellData.name} and deals {damage} damage!";
        }


        Func<string>[] effects = new Func<string>[]
        {
            () => { target.TakeDamage(damage); return resultMessage; }
        };

        return effects;
    }
}
