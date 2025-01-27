using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Example shared data
    public string monsterName;
    public int playerHealth;

    private void Awake()
    {
        // Singleton pattern with persistence
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}

/* example retrieve data */
/*
public class EncounterController : MonoBehaviour
{
    private void Start()
    {
        // Access data from the GameManager
        string monsterName = GameManager.Instance.monsterName;
        int playerHealth = GameManager.Instance.playerHealth;

        Debug.Log($"Monster Name: {monsterName}");
        Debug.Log($"Player Health: {playerHealth}");
    }
}
*/

/* example update data */
/*
public class DungeonController : MonoBehaviour
{
    private void Start()
    {
        // Set data in the GameManager
        GameManager.Instance.monsterName = "Goblin";
        GameManager.Instance.playerHealth = 100;
    }
}
*/