using System;
using UnityEngine;

public class FleeAction : ICreatureAction
{
    public ICreature source;
    public ICreature[] targets;

    public Func<string>[] Perform()
    {
        var resultMessage = $"{source.GetName()} successfully flees!";

        // TODO: Implement this method
        Func<string>[] effects = new Func<string>[]
        {
            () => { return resultMessage; }
        };

        return effects;
    }
}
