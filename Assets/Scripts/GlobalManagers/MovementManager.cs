using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player input for turn-based movement and sending notifications for player actions
/// </summary>
public class MovementManager : MonoBehaviour
{
    /// <summary>
    /// The distance to move forward or backward
    /// </summary>
    public float moveDistance;

    /// <summary>
    /// The time it takes to complete the movement
    /// </summary>
    public float moveDuration;

    /// <summary>
    /// The speed of rotation transition
    /// </summary>
    public float rotationSpeed;

    /// <summary>
    /// The size of the grid in Unity space
    /// </summary>
    public float gridSize;

    private Rigidbody _playerRigidbody;
    private bool _isActionInProgress = false; // Flag to prevent overlapping actions that would cause the player to be in an invalid state (e.g. moving forward while turning)
    private Vector3 _collisionVectorOffset = new Vector3(0, 5, 0); // Offset to move the collision raycast to a more appropriate position (e.g. up from the center of the player)
    private Vector3 _currentFacingDirection = Vector3.forward;
    private bool movementEnabled = true;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        // Note that unsubscribing from events is important to prevent memory leaks.
        // Since this happens inside `OnDisable`, it will be called when the GameObject is disabled or destroyed
        // so there is no need to track whether movement is enabled for the player.
        UnsubscribeFromEvents();
    }

    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void SubscribeToEvents()
    {
        InputManager.Instance.OnStepForward += OnStepForward;
        InputManager.Instance.OnStepBackward += OnStepBackward;
        InputManager.Instance.OnStrafeLeft += OnStrafeLeft;
        InputManager.Instance.OnStrafeRight += OnStrafeRight;
        InputManager.Instance.OnRotateLeft += OnRotateLeft;
        InputManager.Instance.OnRotateRight += OnRotateRight;
        GeneralNotifier.OnPauseGame += OnPauseGame;
        GeneralNotifier.OnResumeGame += OnResumeGame;
    }

    private void UnsubscribeFromEvents()
    {
        InputManager.Instance.OnStepForward -= OnStepForward;
        InputManager.Instance.OnStepBackward -= OnStepBackward;
        InputManager.Instance.OnStrafeLeft -= OnStrafeLeft;
        InputManager.Instance.OnStrafeRight -= OnStrafeRight;
        InputManager.Instance.OnRotateLeft -= OnRotateLeft;
        InputManager.Instance.OnRotateRight -= OnRotateRight;
        GeneralNotifier.OnPauseGame -= OnPauseGame;
        GeneralNotifier.OnResumeGame -= OnResumeGame;
    }

    private void OnPauseGame()
    {
        DisableMovement();
    }

    private void OnResumeGame()
    {
        EnableMovement();
    }

    private void DisableMovement()
    {
        movementEnabled = false;
    }

    private void EnableMovement()
    {
        movementEnabled = true;
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
        Debug.Log("OnStepBackward");
        if (!_isActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.back));
        }
    }

    private IEnumerator MoveStep(Vector3 direction)
    {
        var startPosition = transform.position;
        var targetPosition = transform.position + transform.TransformDirection(direction) * moveDistance;

        if (IsGridCellAccessible(targetPosition) && movementEnabled)
        {
            _isActionInProgress = true;

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

            _isActionInProgress = false;
            // TODO: add logic for sending information about how loud the player is
            PlayerActionNotifier.PlayerMoved();
            PlayerActionNotifier.PlayerMadeNoise(10); // for example noise level 10
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

    /// <summary>
    /// Snap the player to the nearest grid cell to ensure they are always on the grid
    /// as inaccuracies in floating point arithmetic can cause the player to drift off the grid.
    /// </summary>
    private void SnapToGrid()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize;  // Snap to grid on the x-axis
        position.z = Mathf.Round(position.z / gridSize) * gridSize;  // Snap to grid on the z-axis
                                                                     // Keep the y position as is (no rounding)
        transform.position = position;
    }

    /// <summary>
    /// Rotates the player by a specified angle over time.
    /// This method is a coroutine and will yield control back to the Unity engine each frame until the rotation is complete.
    /// </summary>
    /// <param name="angle">The angle to rotate the player, in degrees.</param>
    /// <returns>An IEnumerator that performs the rotation over time.</returns>
    private IEnumerator RotatePlayer(float angle)
    {
        if (movementEnabled)
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
    }

    /// <summary>
    /// Checks if the grid cell at the specified position is accessible.
    /// </summary>
    /// <param name="gridPosition">The grid position/cell to check</param>
    /// <returns>True if the cell can be accessed and false otherwise</returns>
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

    /// <summary>
    /// Visualize various elements in the scene view for debugging purposes.
    /// </summary>
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
