using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public TMP_Text actionName;

    public static ActionButton Create(Transform parent, GameObject prefab)
    {
        var actionButton = Instantiate(prefab, parent);
        return actionButton.GetComponent<ActionButton>();
    }

    public static void Destroy(Transform parent, GameObject prefab)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
