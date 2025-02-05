using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DungeonData DungeonData;
    public EncounterData EncounterData;
    public PartyManager Party;

    /// <summary>
    /// Event when a scene is fully loaded - contains the scene name as a string
    /// </summary>
    public static event Action<string> OnSceneLoaded;

    private const string RootEncounterObjectName = "Encounter";
    private Scene EncounterScene;
    private Scene DungeonScene;
    private GameObject EncounterGameObject;
    private GameObject DungeonGameObject;

    private void Awake()
    {
        SetupSingletonInstance();
        SubscribeToEvents();

        LoadScene(EncounterData.encounterSceneName, InitializeEncounterSceneAndGameObject);
        LoadScene(DungeonData.dungeonSceneName, InitializeDungeonSceneAndGameObject);
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
        EncounterScene = SceneManager.GetSceneByName(EncounterData.encounterSceneName);
        if (EncounterScene.IsValid() == false)
        {
            Debug.LogError("Enounter scene is invalid");
            return;
        }
        EncounterGameObject = FindRootGameObjectByName(EncounterScene, EncounterData.encounterObjectName);
        EncounterGameObject.SetActive(false);
    }

    private void InitializeDungeonSceneAndGameObject()
    {
        DungeonScene = SceneManager.GetSceneByName(DungeonData.dungeonSceneName);
        if (DungeonScene.IsValid() == false)
        {
            Debug.LogError("Dungeon scene is invalid");
            return;
        }
        SceneManager.SetActiveScene(DungeonScene);
        DungeonGameObject = FindRootGameObjectByName(DungeonScene, DungeonData.dungeonObjectName);
        DungeonGameObject.SetActive(true); // The dungeon game object should be shown at first
    }

    private void SubscribeToEvents()
    {
        GeneralNotifier.OnResetGame += HandleGameReset;
        GeneralNotifier.OnPauseGame += PauseGame;
        GeneralNotifier.OnResumeGame += ResumeGame;
        EncounterEventNotifier.OnEncounterStart += HandleEncounterStarted;
        EncounterEventNotifier.OnEncounterEnd += HandleEncounterEnded;
    }

    private void UnsubscribeFromEvents()
    {
        GeneralNotifier.OnResetGame -= HandleGameReset;
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

    private GameObject CloneGameObject(GameObject gameObject)
    {
        GameObject clonedGameObject = Instantiate(gameObject);
        DontDestroyOnLoad(clonedGameObject);
        return clonedGameObject;
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
            // If the scene is already loaded, directly invoke the callback
            onSceneLoadedCallback?.Invoke();
        }
        else
        {
            // If the scene is not loaded, load it asynchronously
            StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoadedCallback));
        }
    }

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

    public void HandleGameReset()
    {
    }
}