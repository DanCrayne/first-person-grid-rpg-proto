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

    public void HandleAttackForMonster()
    {
        int monsterMinDamage = currentMonster.monsterData.attacks.FirstOrDefault().minDamage;
        int monsterMaxDamage = currentMonster.monsterData.attacks.FirstOrDefault().maxDamage;
        int damage = Random.Range(monsterMinDamage, monsterMaxDamage);
        targetedCharacter.TakeDamage(damage);

        Debug.Log($"Monster dealt {damage} damage to targeted character");
    }

    private Monster SelectMonsterToAttack()
    {
        Debug.Log("Selecting monster to attack");
        return monstersInEncounter.FirstOrDefault();
    }

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

    private void StartMonstersTurn()
    {
        if (IsEncounterOver())
        {
            return;
        }

        StartCoroutine(MonstersTurnCoroutine());
    }

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

    private bool IsEncounterOver()
    {
        return monstersInEncounter.Count <= 0 || partyMembersInEncounter.Count <= 0 || AreAllPartyMembersDead();
    }

    private bool AreAllPartyMembersDead()
    {
        return partyMembersInEncounter.Exists(partyMember => partyMember.GetHitPoints() > 0);
    }
}
