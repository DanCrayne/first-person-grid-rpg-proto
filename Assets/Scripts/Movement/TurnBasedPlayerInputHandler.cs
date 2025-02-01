using System.Collections;
using UnityEngine;


public class TurnBasedPlayerInputHandler : MonoBehaviour
{
    public float moveDistance; // Distance to move forward or backward
    public float moveDuration; // Time it takes to complete the movement
    public float rotationSpeed; // Speed of rotation transition
    public float gridSize; // size of grid in Unity space
    public Vector3 playerSpawnPoint;

    private InputSystem_Actions _inputActions;
    private Rigidbody _playerRigidbody;
    private bool _isActionInProgress = false; // Flag to prevent overlapping actions that would cause the player to be in an invalid state (e.g. moving forward while turning)
    private Vector3 _collisionVectorOffset = new Vector3(0, 5, 0); // Offset to move the collision raycast to a more appropriate position (e.g. up from the center of the player)
    private Vector3 _currentFacingDirection = Vector3.forward;
    private Animator _movementAnimator;
    private bool isPlayerTurn = true;

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    public void DisableControls()
    {
        _inputActions.Player.Disable();
    }

    public void EnableControls()
    {
        _inputActions.Player.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log("TurnBasedPlayerInputHandler Awake");
        _inputActions = new InputSystem_Actions();
        _playerRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody
        _playerRigidbody.position = playerSpawnPoint;
        _movementAnimator = GetComponent<Animator>();

        _inputActions.Player.Reset.performed += ctx => HandleResetGame();
        _inputActions.Player.StepForward.performed += ctx => OnStepForward();
        _inputActions.Player.StepBackward.performed += ctx => OnStepBackward();
        _inputActions.Player.StrafeLeft.performed += ctx => OnStrafeLeft();
        _inputActions.Player.StrafeRight.performed += ctx => OnStrafeRight();
        _inputActions.Player.RotateLeft.performed += ctx => OnRotateLeft();
        _inputActions.Player.RotateRight.performed += ctx => OnRotateRight();

        TurnNotifier.OnPlayerMoved += OnMonsterMoved;
    }

    private void HandleResetGame()
    {
        Debug.Log("ResetToSpawnPoint");
        _playerRigidbody.position = playerSpawnPoint;
        GeneralNotifier.ResetGame();
    }

    private void OnStepForward()
    {
        Debug.Log("OnStepForward");
        if (!_isActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.forward));
        }
    }

    private void OnStepBackward()
    {
        if (!_isActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.back));
        }
    }

    private void OnMonsterMoved()
    {
        isPlayerTurn = true;
    }

    private IEnumerator MoveStep(Vector3 direction)
    {
        if (!isPlayerTurn)
            yield return null;

        TurnNotifier.PlayerMoved();

        var startPosition = transform.position;
        var targetPosition = transform.position + transform.TransformDirection(direction) * moveDistance;

        if (IsGridCellAccessible(targetPosition))
        {
            _isActionInProgress = true;
            _movementAnimator.SetBool("isStepping", true);

            Debug.Log($"start pos: {startPosition}; target pos: {targetPosition}");

            var elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                var t = elapsedTime / moveDuration;
                var newPosition = Vector3.Lerp(startPosition, targetPosition, t);

                _playerRigidbody.MovePosition(newPosition);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move to final position
            _playerRigidbody.MovePosition(targetPosition);

            // ensure the player is on-grid after movement
            SnapToGrid();
            _movementAnimator.SetBool("isStepping", false);

            _isActionInProgress = false;
            isPlayerTurn = false;
            TurnNotifier.PlayerMoved();
        }
    }

    public void OnStrafeLeft()
    {
        if (!_isActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.left)); // Strafe left
        }
    }

    public void OnStrafeRight()
    {
        if (!_isActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.right)); // Strafe right
        }
    }

    public void OnRotateLeft()
    {
        if (!_isActionInProgress)
        {
            StartCoroutine(RotatePlayer(-90f));
        }
    }

    public void OnRotateRight()
    {
        if (!_isActionInProgress)
        {
            StartCoroutine(RotatePlayer(90f));
        }
    }

    private void SnapToGrid()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize;  // Snap to grid on the x-axis
        position.z = Mathf.Round(position.z / gridSize) * gridSize;  // Snap to grid on the z-axis
                                                                     // Keep the y position as is (no rounding)
        transform.position = position;
    }

    private IEnumerator RotatePlayer(float angle)
    {
        _isActionInProgress = true;

        float currentAngle = 0f; // Track rotation progress
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        while (currentAngle < Mathf.Abs(angle))
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            currentAngle += rotationStep;

            if (currentAngle > Mathf.Abs(angle)) currentAngle = Mathf.Abs(angle); // Prevent overshoot

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, currentAngle / Mathf.Abs(angle));
            yield return null;
        }

        _currentFacingDirection = targetRotation * Vector3.forward; // Update the facing direction
        transform.rotation = targetRotation; // Snap to the final rotation
        _isActionInProgress = false;
    }

    private bool IsGridCellAccessible(Vector3 gridPosition)
    {
        // Cast a ray from the current position to the grid position
        Vector3 direction = (gridPosition - transform.position).normalized;

        // Perform a raycast to check if the path is blocked
        if (Physics.Raycast(transform.position + _collisionVectorOffset, direction, out RaycastHit hit, gridSize))
        {
            // Check if the hit object is a wall
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                // There's a wall in the way
                return false;
            }
        }

        // No obstacles detected
        return true;
    }

    private void OnDrawGizmos()
    {
        // Visualize the target grid path
        var startPosition = transform.position + _collisionVectorOffset;
        var targetPosition = startPosition + _currentFacingDirection * moveDistance;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, targetPosition);

        // Visualize the current facing direction
        Gizmos.color = Color.blue;
        float directionLength = 5.0f; // Length of the direction line
        var directionEndPosition = transform.position + _currentFacingDirection * directionLength;
        Gizmos.DrawLine(transform.position, directionEndPosition);
    }
}
