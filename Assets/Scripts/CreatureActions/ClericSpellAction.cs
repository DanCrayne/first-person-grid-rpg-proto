using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClericSpellAction : ICreatureAction
{
    public Creature source;
    public Creature target;
    public ClericSpellData spellData;

    public Func<string>[] Perform()
    {
        // TODO: implement

        bool didSpellWork = true;
        var healAmount = 5;

        var resultMessage = $"{source.GetName()} casts {spellData.name} to no effect!";

        if (didSpellWork)
        {
            resultMessage = $"{source.GetName()} casts {spellData.name} and deals {healAmount} damage!";
        }

        Func<string>[] effects = new Func<string>[]
        {
            () => { target.Heal(healAmount); return resultMessage; }
        };

        return effects;
    }
}
