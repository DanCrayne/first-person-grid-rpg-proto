using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public bool isAutoBattle = false;

    public PartyManager playerParty;
    public PartyManager opposingParty;

    private List<Creature> partyMembersInEncounter = new List<Creature>();
    private List<Creature> monstersInEncounter = new List<Creature>();

    private int currentCharacterIndex = 0;
    private List<ICreatureAction> battleActionQueue = new List<ICreatureAction>();

    private void OnEnable()
    {
        currentCharacterIndex = 0;
        EncounterEventNotifier.OnEncounterStart += StartBattle;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnEncounterStart -= StartBattle;
    }

    public Creature GetActiveCharacter()
    {
        if (partyMembersInEncounter.Count > 0)
        {
            return partyMembersInEncounter[currentCharacterIndex];
        }

        return null;
    }

    public void StartBattle()
    {
        // Set player party
        partyMembersInEncounter = playerParty.GetPartyMembers();

        // Determine random opposing party for this encounter and spawn them in the encounter screen
        var spawnedMonsters = battleUIManager.SpawnCreaturesAtPositions(GetWanderingPartyForEncounter().creatureSpawners);
        monstersInEncounter = spawnedMonsters;

        battleUIManager.OpenBattleUI();
        battleUIManager.LogBattleMessage($"The monsters attack!");
        battleUIManager.PopulatePartyPanel(partyMembersInEncounter);

        StartPlayerTurn();
    }

    private PartyData GetWanderingPartyForEncounter()
    {
        // get possible wandering parties and pick one for the opposing party
        var possibleWanderingParties = GameManager.Instance.DungeonData.partiesInDungeon;
        if (possibleWanderingParties == null || possibleWanderingParties.Length <= 0)
        {
            Debug.LogError("No wandering parties found in dungeon data");
            return null;
        }

        var selectedWanderingParty = possibleWanderingParties[Random.Range(0, possibleWanderingParties.Length)];

        return selectedWanderingParty;
    }

    /// <summary>
    /// Starts the turn for the next character in the party
    /// </summary>
    private void StartPlayerTurn()
    {
        currentCharacterIndex = 0;
        ShowCurrentCharacterAsSelected();
        battleUIManager.ActivateActionsPanel();

        // TODO: implement auto-battle logic
    }

    public void StartNextCharacterTurn()
    {
        ShowCurrentCharacterAsDeselected();

        if (!TryToIncrementToNextCharacter())
        {
            StartMonstersTurn();
        }

        ShowCurrentCharacterAsSelected();
        battleUIManager.ActivateActionsPanel();
    }

    /// <summary>
    /// Starts the turn for the next monster in the encounter
    /// </summary>
    private void StartMonstersTurn()
    {
        foreach (var monster in monstersInEncounter)
        {
            if (monster != null && !monster.IsDead())
            {
                var creatureAction = monster.DecideAction(partyMembersInEncounter);
                battleActionQueue.Add(creatureAction);
            }
        }

        ExecuteActionsInQueue();
    }

    private void ExecuteActionsInQueue()
    {
        foreach (var action in battleActionQueue)
        {
            var effects = action.Perform();
            foreach (var effect in effects)
            {
                var resultMessage = effect();
                battleUIManager.LogBattleMessage(resultMessage);
            }
        }

        battleActionQueue.Clear();
        CheckForEndOfEncounter();
    }

    private void SortBattleActionQueue()
    {
        //battleActionQueue = battleActionQueue.OrderBy(action => action.GetPriority()).ToList();
    }

    private void CheckForEndOfEncounter()
    {
        if (AreMonstersWiped())
        {
            battleUIManager.LogBattleMessage($"Monsters are defeated!");
            EndEncounter();
        }
        else if (IsPartyWiped())
        {
            // TODO: do something here to end the game
        }
    }

    /// <summary>
    /// Handles the attack for the current character by reducing the targeted monster's hit points
    /// and signalling the encounter end if the monster is dead
    /// </summary>
    public void AddCurrentCharacterAttackToActionsQueue(Creature targetedMonster)
    {

        if (targetedMonster != null)
        {
            var attackResult = GetActiveCharacter().Attack(targetedMonster, monstersInEncounter);
            battleActionQueue.Add(attackResult);
        }

        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterDefend()
    {
        // TODO: note that defend should probably go at the beginning of the queue so that attacks will not bypass it

        battleActionQueue.Add(GetActiveCharacter().Defend());
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterFlee()
    {
        // TODO: flee logic

        battleActionQueue.Add(GetActiveCharacter().Flee(monstersInEncounter));
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterUseItem()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetName()} is using an item");

        // TODO: open item use menu
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterCast()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetName()} is casting a spell");

        // TODO: open item use menu
        StartNextCharacterTurn();
    }

    private void EndEncounter()
    {
        battleUIManager.ClearBattleLog();
        battleUIManager.CloseBattleUI();
        EncounterEventNotifier.EncounterEnd();
    }

    public void SetupMonsterSelectionPanel()
    {
        battleUIManager.PopulateMonsterSelectionPanel(monstersInEncounter);
    }

    private void ShowCurrentCharacterAsSelected()
    {
        var character = partyMembersInEncounter[currentCharacterIndex];
        battleUIManager.ShowCharacterAsSelectedInPartyPanel(character);
    }

    private void ShowCurrentCharacterAsDeselected()
    {
        var character = partyMembersInEncounter[currentCharacterIndex];
        battleUIManager.ShowCharacterAsDeselectedInPartyPanel(character);
    }

    /// <summary>
    /// Increments the character index to the next character unless we're at the last character, in which case we loop back to 0
    /// </summary>
    /// <returns>True if the next character was incremented and false if they were the final party member in our party list</returns>
    private bool TryToIncrementToNextCharacter()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex >= partyMembersInEncounter.Count)
        {
            currentCharacterIndex = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the encounter is over by checking if there are no monsters or party members left
    /// </summary>
    /// <returns>True if the encounter should end and false otherwise</returns>
    private bool IsEncounterOver()
    {
        return AreMonstersWiped() || IsPartyWiped();
    }

    /// <summary>
    /// Checks if all party members are dead
    /// </summary>
    /// <returns>Returns true is all party members are dead and false otherwise</returns>
    private bool IsPartyWiped()
    {
        return !partyMembersInEncounter.Any(partyMember => partyMember.GetHitPoints() > 0);
    }

    private bool AreMonstersWiped()
    {
        return !monstersInEncounter.Any(m => m != null && m.GetHitPoints() > 0);
    }
}
