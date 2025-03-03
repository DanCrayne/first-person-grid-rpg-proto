using UnityEngine;

/// <summary>
/// Manages input from the player and sends notifications for player actions to other systems
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public delegate void InputAction();
    public event InputAction OnStepForward;
    public event InputAction OnStepBackward;
    public event InputAction OnStrafeLeft;
    public event InputAction OnStrafeRight;
    public event InputAction OnRotateLeft;
    public event InputAction OnRotateRight;

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _inputActions = new InputSystem_Actions();

        _inputActions.Player.StepForward.performed += ctx => OnStepForward?.Invoke();
        _inputActions.Player.StepBackward.performed += ctx => OnStepBackward?.Invoke();
        _inputActions.Player.StrafeLeft.performed += ctx => OnStrafeLeft?.Invoke();
        _inputActions.Player.StrafeRight.performed += ctx => OnStrafeRight?.Invoke();
        _inputActions.Player.RotateLeft.performed += ctx => OnRotateLeft?.Invoke();
        _inputActions.Player.RotateRight.performed += ctx => OnRotateRight?.Invoke();
        _inputActions.Player.OpenMainMenu.performed += ctx => MenuNotifier.ToggleMenu(MenuTypes.main);
        _inputActions.Player.OpenCharacterMenu.performed += ctx => MenuNotifier.ToggleMenu(MenuTypes.character);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}
