using System;
using UnityEngine;

public class UiNotifier : MonoBehaviour
{
    public static event Action<Creature> OnCreatureSelected;
    public static event Action<Creature> OnCreatureDeselected;
    public static event Action OnAllCreaturesDeselected;

    public static void CreatureSelected(Creature selectedCreature)
    {
        OnCreatureSelected?.Invoke(selectedCreature);
    }

    public static void CreatureDeselected(Creature deselectedCreature)
    {
        OnCreatureDeselected?.Invoke(deselectedCreature);
    }

    public static void DeselectAllCreatures()
    {
        OnAllCreaturesDeselected?.Invoke();
    }
}
