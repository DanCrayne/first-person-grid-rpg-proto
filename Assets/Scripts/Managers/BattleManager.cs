using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;

    private List<ICreature> partyMembersInEncounter = new List<ICreature>();
    private List<ICreature> monstersInEncounter = new List<ICreature>();

    private int currentCharacterIndex = 0;
    private List<ICreatureAction> battleActionQueue = new List<ICreatureAction>();

    private void OnEnable()
    {
        currentCharacterIndex = 0;
    }

    private void OnDisable()
    {
    }

    public void SetPartyMembersInEncounter(List<ICreature> partyMembers)
    {
        partyMembersInEncounter = partyMembers;
    }

    public void SetMonstersInEncounter(List<ICreature> monsters)
    {
        monstersInEncounter = monsters;
    }

    public Character GetActiveCharacter()
    {
        return (Character) partyMembersInEncounter[currentCharacterIndex];
    }

    public void StartBattle()
    {
        battleUIManager.OpenBattleUI();
        battleUIManager.LogBattleMessage($"The monsters attack!");
        battleUIManager.PopulatePartyPanel(partyMembersInEncounter);

        StartPlayerTurn();
    }

    /// <summary>
    /// Starts the turn for the next character in the party
    /// </summary>
    private void StartPlayerTurn()
    {
        currentCharacterIndex = 0;
        ShowCurrentCharacterAsSelected();
        battleUIManager.ActivateActionsPanel(GetActiveCharacter());
    }

    public void StartNextCharacterTurn()
    {
        ShowCurrentCharacterAsDeselected();

        if (!TryToIncrementToNextCharacter())
        {
            StartMonstersTurn();
        }

        ShowCurrentCharacterAsSelected();
        battleUIManager.ActivateActionsPanel(GetActiveCharacter());
    }

    /// <summary>
    /// Starts the turn for the next monster in the encounter
    /// </summary>
    private void StartMonstersTurn()
    {
        StartCoroutine(MonstersTurnCoroutine());
        ExecuteActionsInQueue();
    }

    /// <summary>
    /// Coroutine to handle the turn for each monster in the encounter
    /// </summary>
    /// <returns>An enumerator representing the progress of the monsters turns</returns>
    private IEnumerator MonstersTurnCoroutine()
    {
        yield return null;
        //foreach (var monster in monstersInEncounter)
        //{
        //    if (monster != null)
        //    {
        //        var characterToAttack = ((Monster)monster).SelectAttackTarget(partyMembersInEncounter);
        //        ((Monster)monster).CalculateAttackResult(characterToAttack);

        //        yield return new WaitForSeconds(1);
        //    }
        //}
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
    public void AddCurrentCharacterAttackToActionsQueue(Monster targetedMonster)
    {

        if (targetedMonster != null)
        {
            var attackResult = partyMembersInEncounter[currentCharacterIndex].Attack(targetedMonster, monstersInEncounter);
            battleActionQueue.Add(attackResult);
        }

        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterDefend()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetName()} is defending");

        // TODO: defend logic
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterFlee()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetName()} is trying to flee");

        // TODO: flee logic
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

    /// <summary>
    /// Handles the attack for the current monster by reducing the targeted character's hit points
    /// </summary>
    public void ExecuteMonsterAttack(Monster monster)
    {
        //if (monster == null)
        //{
        //    return;
        //}

        //var targetedCharacter = ((Monster)monster).SelectAttackTarget(partyMembersInEncounter);

        //var attackResult = monster.Attack(targetedCharacter);
        //if (attackResult.didAttackHit)
        //{
        //    targetedCharacter.TakeDamage(attackResult.damage);
        //    battleUIManager.LogBattleMessage($"Monster dealt {attackResult.damage} damage to {((Character)targetedCharacter).GetName()}");

        //    if (targetedCharacter.IsDead())
        //    {
        //        battleUIManager.ShowCharacterAsDeadInPartyPanel((Character) targetedCharacter);
        //        battleUIManager.LogBattleMessage($"{((Character)targetedCharacter).GetName()} dies!");
        //    }
        //}

        // TODO: handle party wipe - should this be calculated in the PartyManager?
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
        var character = (Character)partyMembersInEncounter[currentCharacterIndex];
        battleUIManager.ShowCharacterAsSelectedInPartyPanel(character);
    }

    private void ShowCurrentCharacterAsDeselected()
    {
        var character = (Character)partyMembersInEncounter[currentCharacterIndex];
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
