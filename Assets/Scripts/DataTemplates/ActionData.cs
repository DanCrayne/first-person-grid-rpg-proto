using UnityEngine;

[CreateAssetMenu(fileName = "NewActionData", menuName = "Action/New Action Data")]
public class ActionData : ScriptableObject
{
    public string actionName;
    public string actionDescription;
    public GameObject actionControlPrefab;
}
