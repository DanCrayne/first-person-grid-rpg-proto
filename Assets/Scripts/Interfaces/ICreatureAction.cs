using System;

public interface ICreatureAction
{
    public Func<string>[] Perform();
}
