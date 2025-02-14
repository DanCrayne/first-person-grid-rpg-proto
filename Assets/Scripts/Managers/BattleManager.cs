using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleMenuManager battleMenuManager;

    public List<Character> partyMembersInEncounter = new List<Character>();
    public List<Monster> monstersInEncounter = new List<Monster>();

    private int currentCharacterIndex = 0;
    private bool isFirstRound = true;

    public void StartBattle()
    {
        battleMenuManager.LogBattleMessage($"The monsters attack!");
        battleMenuManager.PopulatePartyPanel(partyMembersInEncounter);

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

        HandleAttackForCharacter();
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
    private void HandleAttackForCharacter()
    {
        var targetedMonster = SelectMonsterToAttack();
        if (targetedMonster != null)
        {
            var damage = partyMembersInEncounter[currentCharacterIndex].GetEquippedWeaponAttack();
            targetedMonster.TakeDamage(damage);
            battleMenuManager.LogBattleMessage($"{partyMembersInEncounter[currentCharacterIndex].GetCharacterName()} dealt {damage} damage to {targetedMonster.monsterData.monsterName}");

            if (targetedMonster.IsMonsterDead())
            {
                battleMenuManager.LogBattleMessage($"\n{targetedMonster.monsterData.monsterName} is defeated!", true);

                if (AreMonstersWiped())
                {
                    EndEncounter();
                }
            }
        }
        else
        {
            battleMenuManager.LogBattleMessage($"Monsters are defeated!", true);
            EndEncounter();
        }
    }

    /// <summary>
    /// Handles the attack for the current monster by reducing the targeted character's hit points
    /// </summary>
    public void HandleAttackForMonster(Monster monster)
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
            battleMenuManager.LogBattleMessage($"Monster dealt {attackResult.damage} damage to {targetedCharacter.GetCharacterName()}");

            if (targetedCharacter.IsCharacterDead())
            {
                battleMenuManager.ShowCharacterAsDeadInPartyPanel(targetedCharacter);
                battleMenuManager.LogBattleMessage($"{targetedCharacter.GetCharacterName()} dies!");
            }
        }

        // TODO: handle party wipe - should this be calculated in the PartyManager?
    }

    private void EndEncounter()
    {
        isFirstRound = true;
        EncounterEventNotifier.EncounterEnd();
    }

    private Monster SelectMonsterToAttack()
    {
        // just attack the first non-null monster for now
        Debug.Log("Selecting monster to attack");
        foreach (var monster in monstersInEncounter)
        {
            if (monster != null)
            {
                return monster;
            }
        }

        return null;
    }

    /// <summary>
    /// Starts the turn for the next character in the party
    /// </summary>
    private void StartPlayerTurn()
    {
        if (IsEncounterOver())
        {
            return;
        }

        ShowCurrentCharacterAsSelected();
    }

    private void ShowCurrentCharacterAsSelected()
    {
        var character = partyMembersInEncounter[currentCharacterIndex];
        battleMenuManager.ShowCharacterAsSelectedInPartyPanel(character);
    }

    private void ShowCurrentCharacterAsDeselected()
    {
        var character = partyMembersInEncounter[currentCharacterIndex];
        battleMenuManager.ShowCharacterAsDeselectedInPartyPanel(character);
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
