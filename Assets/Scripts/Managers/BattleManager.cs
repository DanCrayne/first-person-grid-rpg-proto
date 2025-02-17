using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;

    private List<Character> partyMembersInEncounter = new List<Character>();
    private List<Monster> monstersInEncounter = new List<Monster>();

    private int currentCharacterIndex = 0;
    private bool isFirstRound = true;

    private void OnEnable()
    {
        isFirstRound = true;
        currentCharacterIndex = 0;
    }

    private void OnDisable()
    {
        
    }

    public void SetPartyMembersInEncounter(List<Character> partyMembers)
    {
        partyMembersInEncounter = partyMembers;
    }

    public void SetMonstersInEncounter(List<Monster> monsters)
    {
        monstersInEncounter = monsters;
    }

    public Character GetActiveCharacter()
    {
        return partyMembersInEncounter[currentCharacterIndex];
    }

    public void StartBattle()
    {
        battleUIManager.OpenBattleUI();
        battleUIManager.LogBattleMessage($"The monsters attack!");
        battleUIManager.PopulatePartyPanel(partyMembersInEncounter);

        StartPlayerTurn();
    }

    public void StartNextCharacterTurn()
    {
        if (!isFirstRound)
        {
            ShowCurrentCharacterAsDeselected();

            if (!TryToIncrementToNextCharacter())
            {
                EndPlayerTurn();
            }
        }

        ShowCurrentCharacterAsSelected();

        if (isFirstRound)
        {
            isFirstRound = false;
        }
    }

    /// <summary>
    /// Handles the attack for the current character by reducing the targeted monster's hit points
    /// and signalling the encounter end if the monster is dead
    /// </summary>
    public void ExecuteCurrentCharacterAttack(Monster targetedMonster)
    {
        if (targetedMonster != null)
        {
            var damage = partyMembersInEncounter[currentCharacterIndex].GetEquippedWeaponAttack();
            targetedMonster.TakeDamage(damage);
            battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetCharacterName()} dealt {damage} damage to {targetedMonster.monsterData.monsterName}");

            if (targetedMonster.IsMonsterDead())
            {
                battleUIManager.LogBattleMessage($"{targetedMonster.monsterData.monsterName} is defeated!");

                if (AreMonstersWiped())
                {
                    EndEncounter();
                }
            }
        }
        else
        {
            battleUIManager.LogBattleMessage($"Monsters are defeated!");
            EndEncounter();
        }

        battleUIManager.ActivateActionsPanel(GetActiveCharacter());
    }

    public void ExecuteCurrentCharacterDefend()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetCharacterName()} is defending");

        // TODO: defend logic
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterFlee()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetCharacterName()} is trying to flee");

        // TODO: flee logic
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterUseItem()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetCharacterName()} is using an item");

        // TODO: open item use menu
        StartNextCharacterTurn();
    }

    public void ExecuteCurrentCharacterCast()
    {
        battleUIManager.LogBattleMessage($"{GetActiveCharacter().GetCharacterName()} is casting a spell");

        // TODO: open item use menu
        StartNextCharacterTurn();
    }

    /// <summary>
    /// Handles the attack for the current monster by reducing the targeted character's hit points
    /// </summary>
    public void ExecuteMonsterAttack(Monster monster)
    {
        if (monster == null)
        {
            return;
        }

        var targetedCharacter = monster.SelectAttackTarget(partyMembersInEncounter);

        var attackResult = monster.CalculateRandomAttackResult(targetedCharacter);
        if (attackResult.didAttackHit)
        {
            targetedCharacter.TakeDamage(attackResult.damage);
            battleUIManager.LogBattleMessage($"Monster dealt {attackResult.damage} damage to {targetedCharacter.GetCharacterName()}");

            if (targetedCharacter.IsCharacterDead())
            {
                battleUIManager.ShowCharacterAsDeadInPartyPanel(targetedCharacter);
                battleUIManager.LogBattleMessage($"{targetedCharacter.GetCharacterName()} dies!");
            }
        }

        // TODO: handle party wipe - should this be calculated in the PartyManager?
    }

    private void EndEncounter()
    {
        isFirstRound = true;
        battleUIManager.ClearBattleLog();
        battleUIManager.CloseBattleUI();
        EncounterEventNotifier.EncounterEnd();
    }

    public void SetupMonsterSelectionPanel()
    {
        battleUIManager.PopulateMonsterSelectionPanel(monstersInEncounter);
    }

    /// <summary>
    /// Starts the turn for the next character in the party
    /// </summary>
    private void StartPlayerTurn()
    {
        currentCharacterIndex = 0;

        if (IsEncounterOver())
        {
            Debug.Log("Encounter is over on first round - is the party / monster HP being set properly?");
            return;
        }

        ShowCurrentCharacterAsSelected();
        battleUIManager.ActivateActionsPanel(GetActiveCharacter());
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

    private void EndPlayerTurn()
    {
        StartMonstersTurn();
    }

    /// <summary>
    /// Starts the turn for the next monster in the encounter
    /// </summary>
    private void StartMonstersTurn()
    {
        if (IsEncounterOver())
        {
            return;
        }

        StartCoroutine(MonstersTurnCoroutine());
    }

    /// <summary>
    /// Coroutine to handle the turn for each monster in the encounter
    /// </summary>
    /// <returns>An enumerator representing the progress of the monsters turns</returns>
    private IEnumerator MonstersTurnCoroutine()
    {
        foreach (var monster in monstersInEncounter)
        {
            if (monster != null)
            {
                var characterToAttack = monster.SelectAttackTarget(partyMembersInEncounter);
                monster.CalculateRandomAttackResult(characterToAttack);

                yield return new WaitForSeconds(1);
            }
        }
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
