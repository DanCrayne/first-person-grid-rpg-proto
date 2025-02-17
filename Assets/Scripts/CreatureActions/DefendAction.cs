using System;

public class DefendAction : ICreatureAction
{
    public ICreature source;

    public DefendAction(ICreature source)
    {
        this.source = source;
    }

    public Func<string>[] Perform()
    {
        var resultMessage = $"{source.GetName()} is blocking";

        // TODO: Implement this method

        Func<string>[] effects = new Func<string>[]
        {
            () => { source.Defend(); return resultMessage; }
        };

        return effects;
    }
}
