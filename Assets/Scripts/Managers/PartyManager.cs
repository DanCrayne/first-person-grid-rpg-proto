using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public string partyName;
    public List<GameObject> partyMembers;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Start()
    {
        var characterNamesAndPositions = new Dictionary<string, Vector3>
        {
            { "Tim", new Vector3(-6.5f, 0, -6f) },
            { "Kim", new Vector3(0, 0, -6f) },
            { "Sim", new Vector3(6.5f, 0, -10f) }
        };

        CreateParty(characterNamesAndPositions);
    }

    public void CreateParty(Dictionary<string, Vector3> characterNamesAndPositions)
    {
        foreach (var character in characterNamesAndPositions)
        {
            var characterPrefabName = "Monk";
            var characterPrefab = Resources.Load<GameObject>($"Prefabs/{characterPrefabName}");
            var characterInstance = Instantiate(characterPrefab, character.Value, Quaternion.identity, transform.parent);
            characterInstance.name = character.Key;
            partyMembers.Add(characterInstance);
        }
    }

    public void OrientPartyMembersTowardsPosition(GameObject[] characters, Vector3 lookingAtPoint)
    {
        foreach (var character in characters)
        {
            // Calculate the direction to the battle starting position
            Vector3 directionToBattleStart = (lookingAtPoint - character.transform.position).normalized;
            // Calculate the rotation to face the battle starting position
            Quaternion lookRotation = Quaternion.LookRotation(directionToBattleStart);
            // Set the monster's rotation
            character.transform.rotation = lookRotation;
        }
    }
}
