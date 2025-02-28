using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the party of characters in the game
/// </summary>
public class PartyManager : MonoBehaviour
{
    public PartyData partyStaticData;
    public int maxPartySize = 3;

    private List<Creature> partyMembers = new List<Creature>();

    private void Start()
    {
        // load party data if available
        if (partyStaticData?.creatures != null)
        {
            LoadPartyData();
        }
    }

    public void LoadPartyData()
    {
        if (partyStaticData != null)
        {
            foreach (var creature in partyStaticData.creatures)
            {
                AddCreatureToParty(Creature.LoadPlayerCreature(creature, transform));
            }
        }
    }

    public List<Creature> GetPartyMembers()
    {
        return partyMembers;
    }

    public bool IsPartyWiped()
    {
        return !partyMembers.Any(c => c.GetHitPoints() > 0);
    }

    public void AddCreatureToParty(Creature creature)
    {
        if (partyMembers.Count >= maxPartySize)
        {
            Debug.Log("Can't add anymore creatures to party - party is full");
        }

        partyMembers.Add(creature);
    }

    public void AddCreatureRangeToParty(IEnumerable<Creature> creatures)
    {
        if (partyMembers.Count() + creatures.Count() >= maxPartySize)
        {
            Debug.Log($"Can't that many creatures to party since it would exceed the max size of {maxPartySize}");
        }

        partyMembers.AddRange(creatures);
    }

    public void RemoveCreatureFromParty(Creature creature)
    {
        partyMembers.Remove(creature);
    }

    public PartyDefeatRewards GetDefeatRewards()
    {
        return new PartyDefeatRewards { items = GetPartyTotalItems(), totalGold = GetPartyTotalGold(), totalXp = GetPartyTotalXp() };
    }

    public int GetPartyTotalGold()
    {
        var totalGold = 0;

        foreach (var member in partyMembers)
        {
            totalGold += member.GetGold();
        }

        return totalGold;
    }

    public int GetPartyTotalXp()
    {
        int totalXp = 0;

        foreach (var member in partyMembers)
        {
            totalXp += member.creatureStaticData != null ? member.creatureStaticData.xp : 0;
        }

        return totalXp;
    }

    public List<ItemData> GetPartyTotalItems()
    {
        var partyItems = new List<ItemData>();

        foreach (var member in partyMembers)
        {
            var inventoryManager = member.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                partyItems.AddRange(inventoryManager.items);
            }
        }

        return partyItems;
    }
}

