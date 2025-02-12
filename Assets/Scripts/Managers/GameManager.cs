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

    private Scene _dungeonScene;
    private GameObject _encounterGameObject;
    private GameObject _dungeonGameObject;
    private PartyManager _partyManager;

    public GameObject GetEncounterGameObject()
    {
        return _encounterGameObject;
    }

    public PartyManager GetPartyManager()
    {
        return _partyManager;
    }

    /// <summary>
    /// Pauses the game, including animations and anything that relies on Time.deltaTime
    /// such as physics and movement.
    /// </summary>
    public void PauseGame()
    {
        GeneralNotifier.PauseGame();
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game, including animations and anything that relies on Time.deltaTime
    /// </summary>
    public void ResumeGame()
    {
        GeneralNotifier.ResumeGame();
        Time.timeScale = 1;
    }

    private void Awake()
    {
        SetupSingletonInstance();
        SubscribeToEvents();
        LoadScene(DungeonData.dungeonSceneName, InitializeDungeonSceneAndGameObjects);
        _partyManager = GetComponent<PartyManager>();
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
        _encounterGameObject.SetActive(true);
        _dungeonGameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the dungeon game object and deactivates the encounter game object
    /// </summary>
    private void ActivateDungeonGameObject()
    {
        _encounterGameObject.SetActive(false);
        _dungeonGameObject.SetActive(true);
    }

    /// <summary>
    /// Initializes the dungeon scene and game objects by finding and activating them as appropriate
    /// </summary>
    private void InitializeDungeonSceneAndGameObjects()
    {
        _dungeonScene = SceneManager.GetSceneByName(DungeonData.dungeonSceneName);
        if (_dungeonScene.IsValid() == false)
        {
            Debug.LogError("Dungeon scene is invalid");
            return;
        }

        // activate dungeon object
        SceneManager.SetActiveScene(_dungeonScene);
        _dungeonGameObject = FindRootGameObjectByName(_dungeonScene, DungeonData.dungeonObjectName);
        _dungeonGameObject.SetActive(true); // The dungeon game object should be shown at first

        // deactivate encounter object (it will be activated when an encounter starts)
        _encounterGameObject = FindRootGameObjectByName(_dungeonScene, EncounterData.encounterObjectName);
        _encounterGameObject.SetActive(false);
    }

    private void SubscribeToEvents()
    {
        EncounterEventNotifier.OnEncounterStart += HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd += HandleEncounterEnded;
    }

    private void UnsubscribeFromEvents()
    {
        EncounterEventNotifier.OnEncounterStart -= HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd -= HandleEncounterEnded;
    }

    private void HandleEncounterStarted()
    {
        GeneralNotifier.DisableMovement();
        ActivateEncounterGameObject();
    }

    private void HandleEncounterEnded()
    {
        ActivateDungeonGameObject();
        GeneralNotifier.EnableMovement();
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    /// <summary>
    /// Loads a scene additively and invokes a callback when the scene is fully loaded
    /// </summary>
    /// <param name="sceneName">The scene name to load</param>
    /// <param name="onSceneLoadedCallback">The callback to be invoked once the scene is loaded</param>
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
    /// <returns>An <see cref="IEnumerator"/> representing the load progress</returns>
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

    /// <summary>
    /// Finds a root GameObject in a scene by name
    /// </summary>
    /// <param name="scene">The scene containing the object</param>
    /// <param name="rootGameObjectName">The name of the object to find</param>
    /// <returns>The root <see cref="GameObject"/> if found and null otherwise</returns>
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