using System;
using System.Collections.Generic;

public class FleeAction : ICreatureAction
{
    public ICreature source;
    public List<ICreature> targets;

    public FleeAction(ICreature source, List<ICreature> targets)
    {
        this.source = source;
        this.targets = targets;
    }

    public Func<string>[] Perform()
    {
        var resultMessage = $"{source.GetName()} tries to flee... but is blocked!";

        // TODO: Implement this method
        Func<string>[] effects = new Func<string>[]
        {
            () => { return resultMessage; }
        };

        return effects;
    }
}
