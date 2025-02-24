using System;
using System.Collections.Generic;

public class FleeAction : ICreatureAction
{
    public Creature source;
    public List<Creature> targets;

    public FleeAction(Creature source, List<Creature> targets)
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
