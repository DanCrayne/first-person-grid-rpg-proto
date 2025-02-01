using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string CurrentArea = "Dungeon01";
    public string EncounterSceneName = "Encounter";
    public GameObject CurrentlyAttackingWanderingMonster;
    public GameObject PlayerGameObject;
    
    /// <summary>
    /// Event when a scene is fully loaded - contains the scene name as a string
    /// </summary>
    public static event Action<string> OnSceneLoaded;

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

        //EncounterEventNotifier.OnMonsterCollision += HandleMonsterCollision;
        //EncounterEventNotifier.OnEncounterEnd += HandleEncounterEnded;

        LoadScene(EncounterSceneName);
        LoadAndActivateScene(CurrentArea);
    }

    private void HandleEncounterEnded()
    {
        LoadAndActivateScene(CurrentArea);
    }

    private void HandleMonsterCollision(GameObject player, GameObject monster)
    {
        PlayerGameObject = CloneGameObject(player);
        CurrentlyAttackingWanderingMonster = CloneGameObject(monster);
        EncounterEventNotifier.EncounterStart();
        SetSceneAsActive(EncounterSceneName);
    }

    private GameObject CloneGameObject(GameObject gameObject)
    {
        GameObject clonedGameObject = Instantiate(gameObject);
        DontDestroyOnLoad(clonedGameObject);
        return clonedGameObject;
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    private void LoadAndActivateScene(string sceneName)
    {
        // If the scene is already loaded, then just activate it
        if (IsSceneLoaded(sceneName))
        {
            SetSceneAsActive(sceneName);
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneName, true));
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, false));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, bool activate = false)
    {
        if (!IsSceneLoaded(sceneName))
        {
            // Load the additively
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Optionally, activate the loaded scene
            if (activate)
            {
                Scene scene = SceneManager.GetSceneByName(sceneName);
                if (scene.IsValid())
                {
                    SceneManager.SetActiveScene(scene);
                }
            }

            // Optionally, perform additional setup after the scene is loaded
            Debug.Log($"{sceneName} loaded and activated.");

            // Invoke the event to notify listeners that the scene has been loaded
            OnSceneLoaded?.Invoke(sceneName);
        }
    }

    private void SetSceneAsActive(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    //private IEnumerator LoadAndActivateEncounterScene(GameObject player, GameObject monster)
    //{
    //    // Load the additively
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(EncounterSceneName, LoadSceneMode.Additive);

    //    // Wait until the scene is fully loaded
    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }

    //    // Optionally, activate the loaded scene
    //    Scene scene = SceneManager.GetSceneByName(EncounterSceneName);
    //    if (scene.IsValid())
    //    {
    //        SceneManager.SetActiveScene(scene);
    //    }

    //    // Optionally, perform additional setup after the scene is loaded
    //    Debug.Log($"{EncounterSceneName} loaded and activated.");

    //    // Invoke the event to notify listeners that the scene has been loaded
    //    OnSceneLoaded?.Invoke(EncounterSceneName);

    //    // Get the EncounterManager script component
    //    EncounterManager encounterManager = FindEncounterManagerInScene(scene);
    //    if (encounterManager != null)
    //    {
    //        Debug.Log("EncounterManager found in Encounter scene.");
    //        // Pass the player and monster references to the EncounterManager
    //        encounterManager.SetupEncounter(player, monster);
    //    }
    //}

    //private EncounterManager FindEncounterManagerInScene(Scene scene)
    //{
    //    if (!scene.IsValid())
    //    {
    //        Debug.LogError("Invalid scene.");
    //        return null;
    //    }

    //    // Find all root GameObjects in the scene
    //    GameObject[] rootGameObjects = scene.GetRootGameObjects();

    //    // Iterate through the root GameObjects to find the EncounterManager
    //    foreach (GameObject rootGameObject in rootGameObjects)
    //    {
    //        EncounterManager encounterManager = rootGameObject.GetComponent<EncounterManager>();
    //        if (encounterManager != null)
    //        {
    //            return encounterManager;
    //        }
    //    }

    //    Debug.LogError("EncounterManager not found in the scene.");
    //    return null;
    //}
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