using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float moveSpeed = 10f;
    private InputSystem_Actions _inputActions;
    private Vector2 moveInput;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        var movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
