using System;
using UnityEngine;

public class ItemAction : ICreatureAction
{
    public ICreature source;
    public ICreature target;
    public ItemData itemData;

    // TODO : Implement this method
    public Func<string>[] Perform()
    {
        var resultMessage = $"{source.GetName()} uses {itemData?.itemName ?? "some item"}";

        // TODO: Implement this method
        Func<string>[] effects = new Func<string>[]
        {
            () => { return resultMessage; }
        };

        return effects;
    }
}
