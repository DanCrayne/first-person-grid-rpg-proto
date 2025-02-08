using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game state and scene loading
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager
    /// </summary>
    public static GameManager Instance;
    public DungeonData DungeonData;
    public EncounterData EncounterData;

    /// <summary>
    /// Event when a scene is fully loaded - contains the scene name as a string
    /// </summary>
    public static event Action<string> OnSceneLoaded;

    private Scene DungeonScene;
    private GameObject EncounterGameObject;
    private GameObject DungeonGameObject;

    public GameObject GetEncounterGameObject()
    {
        return EncounterGameObject;
    }

    private void Awake()
    {
        SetupSingletonInstance();
        SubscribeToEvents();
        LoadScene(DungeonData.dungeonSceneName, InitializeDungeonSceneAndGameObjects);
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
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

    /// <summary>
    /// Activates the encounter game object and deactivates the dungeon game object
    /// </summary>
    private void ActivateEncounterGameObject()
    {
        EncounterGameObject.SetActive(true);
        DungeonGameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the dungeon game object and deactivates the encounter game object
    /// </summary>
    private void ActivateDungeonGameObject()
    {
        EncounterGameObject.SetActive(false);
        DungeonGameObject.SetActive(true);
    }

    private void InitializeDungeonSceneAndGameObjects()
    {
        DungeonScene = SceneManager.GetSceneByName(DungeonData.dungeonSceneName);
        if (DungeonScene.IsValid() == false)
        {
            Debug.LogError("Dungeon scene is invalid");
            return;
        }
        // activate dungeon object
        SceneManager.SetActiveScene(DungeonScene);
        DungeonGameObject = FindRootGameObjectByName(DungeonScene, DungeonData.dungeonObjectName);
        DungeonGameObject.SetActive(true); // The dungeon game object should be shown at first

        // active encounter object
        EncounterGameObject = FindRootGameObjectByName(DungeonScene, EncounterData.encounterObjectName);
        EncounterGameObject.SetActive(false);
    }

    private void SubscribeToEvents()
    {
        GeneralNotifier.OnResetGame += ResetGame;
        GeneralNotifier.OnPauseGame += PauseGame;
        GeneralNotifier.OnResumeGame += ResumeGame;
        EncounterEventNotifier.OnEncounterStart += HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd += HandleEncounterEnded;
    }

    private void UnsubscribeFromEvents()
    {
        GeneralNotifier.OnResetGame -= ResetGame;
        GeneralNotifier.OnPauseGame -= PauseGame;
        GeneralNotifier.OnResumeGame -= ResumeGame;
        EncounterEventNotifier.OnEncounterStart -= HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd -= HandleEncounterEnded;
    }

    private void HandleEncounterStarted()
    {
        ActivateEncounterGameObject();
    }

    private void HandleEncounterEnded()
    {
        ActivateDungeonGameObject();
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    private void LoadScene(string sceneName, Action onSceneLoadedCallback)
    {
        if (IsSceneLoaded(sceneName))
        {
            // If the scene is already loaded, directly invoke the callback (no need to load it again)
            onSceneLoadedCallback?.Invoke();
        }
        else
        {
            // If the scene is not loaded, load it asynchronously
            StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoadedCallback));
        }
    }

    /// <summary>
    /// Coroutine to load a scene additively and notify listeners when it is fully loaded
    /// </summary>
    /// <param name="sceneName">The name of the <see cref="Scene"/> to load</param>
    /// <param name="onSceneLoadedCallback">A callback method which will be invoked after the Scene is loaded</param>
    /// <returns></returns>
    private IEnumerator LoadSceneCoroutine(string sceneName, Action onSceneLoadedCallback)
    {
        if (!IsSceneLoaded(sceneName))
        {
            // Load the scene additively
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

            // Execute the callback
            onSceneLoadedCallback?.Invoke();
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

    /// <summary>
    /// Pauses the game, including animations and anything that relies on Time.deltaTime
    /// such as physics and movement.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
    }
}