using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string DungeonSceneName = "Dungeon01";
    public string DungeonGameObjectName = "Dungeon";
    public string EncounterSceneName = "Encounter";
    public string EncounterGameObjectName = "Encounter";
    public GameObject CurrentlyAttackingWanderingMonster;
    public GameObject PlayerGameObject;

    /// <summary>
    /// Event when a scene is fully loaded - contains the scene name as a string
    /// </summary>
    public static event Action<string> OnSceneLoaded;

    private Scene EncounterScene;
    private Scene DungeonScene;
    private GameObject EncounterGameObject;
    private GameObject DungeonGameObject;

    private void Awake()
    {
        SetupSingletonInstance();
        SubscribeToEvents();

        InitializeDungeonSceneAndGameObject();
        InitializeEncounterSceneAndGameObject();
        ActivateDungeonGameObject();
    }

    private void SetupSingletonInstance()
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

    private void ActivateEncounterGameObject()
    {
        EncounterGameObject.SetActive(true);
        DungeonGameObject.SetActive(false);
    }

    private void ActivateDungeonGameObject()
    {
        EncounterGameObject.SetActive(false);
        DungeonGameObject.SetActive(true);
    }

    private void InitializeEncounterSceneAndGameObject()
    {
        LoadScene(EncounterSceneName);
        EncounterScene = SceneManager.GetSceneByName(EncounterSceneName);
        EncounterGameObject = FindRootGameObjectByName(EncounterScene, EncounterGameObjectName);
    }

    private void InitializeDungeonSceneAndGameObject()
    {
        LoadScene(DungeonSceneName);
        DungeonScene = SceneManager.GetSceneByName(DungeonSceneName);
        SceneManager.SetActiveScene(DungeonScene);
        DungeonGameObject = FindRootGameObjectByName(DungeonScene, DungeonGameObjectName);
    }

    private void SubscribeToEvents()
    {
        EncounterEventNotifier.OnEncounterStart += HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd += HandleEncounterEnded;
    }

    private void HandleEncounterStarted()
    {
        ActivateEncounterGameObject();
    }

    private void HandleEncounterEnded()
    {
        ActivateDungeonGameObject();
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

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
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

            // Optionally, perform additional setup after the scene is loaded
            Debug.Log($"{sceneName} loaded.");

            // Invoke the event to notify listeners that the scene has been loaded
            OnSceneLoaded?.Invoke(sceneName);
        }
    }

    private GameObject FindRootGameObjectByName(Scene scene, string rootGameObjectName)
    {
        if (!scene.IsValid())
        {
            Debug.LogError("Invalid scene.");
            return null;
        }

        // Find all root GameObjects in the scene
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        // Iterate through the root GameObjects to find the one with the specified name
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            if (rootGameObject.name == rootGameObjectName)
            {
                return rootGameObject;
            }
        }

        Debug.LogError($"Root GameObject with name '{rootGameObjectName}' not found in the scene.");
        return null;
    }
}