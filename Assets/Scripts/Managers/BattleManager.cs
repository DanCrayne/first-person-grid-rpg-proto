using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleMenuManager battleMenuManager;

    private List<Character> partyMembersInEncounter;
    private List<Monster> monstersInEncounter;
    private Character currentCharacter;
    private Monster targetedMonster;
    private Monster currentMonster;
    private Character targetedCharacter;

    public void StartBattle(List<Character> party, List<Monster> monsters)
    {
        partyMembersInEncounter = party;
        monstersInEncounter = monsters;

        currentCharacter = partyMembersInEncounter.FirstOrDefault();
        currentMonster = monstersInEncounter.FirstOrDefault();

        targetedMonster = monstersInEncounter.FirstOrDefault();
        targetedCharacter = partyMembersInEncounter.FirstOrDefault();

        battleMenuManager.PopulatePartyPanel(party);

        StartPlayerTurn();
    }

    /// <summary>
    /// Handles the attack for the current character by reducing the targeted monster's hit points
    /// and signalling the encounter end if the monster is dead
    /// </summary>
    public void HandleAttackForCharacter()
    {
        if (targetedMonster == null)
        {
            monstersInEncounter.Remove(targetedMonster);
        }

        targetedMonster = SelectMonsterToAttack();
        if (targetedMonster != null)
        {
            var damage = currentCharacter.GetEquippedWeaponAttack();
            targetedMonster.TakeDamage(damage);
            Debug.Log($"Character dealt {damage} to targeted monster");

            // Check if monster is dead
            if (targetedMonster.GetHitPoints() <= 0)
            {
                monstersInEncounter.Remove(targetedMonster);
                Debug.Log("Monster is dead; removing from encounter");

                if (monstersInEncounter.Count <= 0)
                {
                    EncounterEventNotifier.EncounterEnd();
                }
            }
        }
        else
        {
            Debug.Log("All monsters are dead; sending event to end encounter");
            EncounterEventNotifier.EncounterEnd();
        }
    }

    /// <summary>
    /// Handles the attack for the current monster by reducing the targeted character's hit points
    /// </summary>
    public void HandleAttackForMonster()
    {
        int monsterMinDamage = currentMonster.monsterData.attacks.FirstOrDefault().minDamage;
        int monsterMaxDamage = currentMonster.monsterData.attacks.FirstOrDefault().maxDamage;
        int damage = Random.Range(monsterMinDamage, monsterMaxDamage);
        targetedCharacter.TakeDamage(damage);
        Debug.Log($"Monster dealt {damage} damage to targeted character");
        // TODO check if character is dead and end encounter if so
    }

    private Monster SelectMonsterToAttack()
    {
        // just attack the first monster for now
        Debug.Log("Selecting monster to attack");
        return monstersInEncounter.FirstOrDefault();
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

        var character = partyMembersInEncounter[0];
        var actionsPanel = GameManager.Instance.GetPartyManager().actionsPanel;
        character.ShowCharacterAsSelectedInEncounter();
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
            var characterToAttack = monster.SelectAttackTarget(partyMembersInEncounter);
            characterToAttack.ShowCharacterAsTargetedInEncounter();
            monster.Attack(characterToAttack);
            if (characterToAttack.GetHitPoints() <= 0)
            {
                characterToAttack.ShowCharacterAsDeadInEncounter();
            }

            yield return new WaitForSeconds(1);

            characterToAttack.ShowCharacterAsDeselectedInEncounter();
        }
    }

    /// <summary>
    /// Checks if the encounter is over by checking if there are no monsters or party members left
    /// </summary>
    /// <returns>True if the encounter should end and false otherwise</returns>
    private bool IsEncounterOver()
    {
        return monstersInEncounter.Count <= 0 || partyMembersInEncounter.Count <= 0 || AreAllPartyMembersDead();
    }

    /// <summary>
    /// Checks if all party members are dead
    /// </summary>
    /// <returns>Returns true is all party members are dead and false otherwise</returns>
    private bool AreAllPartyMembersDead()
    {
        return partyMembersInEncounter.Exists(partyMember => partyMember.GetHitPoints() > 0);
    }
}
