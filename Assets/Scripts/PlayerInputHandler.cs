using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float moveDistance; // Distance to move forward or backward
    public float moveDuration; // Time it takes to complete the movement
    public float rotationSpeed; // Speed of rotation transition
    public float gridSize; // size of grid in Unity space
    public Vector3 playerSpawnPoint;

    private InputSystem_Actions _inputActions;
    private Rigidbody _playerRigidbody;
    private bool _isBlockingActionInProgress = false; // Flag to prevent overlapping actions that would cause the player to be in an invalid state (e.g. moving forward while turning)
    private bool _isBackingUpAfterCollision = false;

    public void DisableControls()
    {
        _inputActions.Player.Disable();
    }

    public void EnableControls()
    {
        _inputActions.Player.Enable();
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
    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _playerRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody
        _playerRigidbody.position = playerSpawnPoint;

        _inputActions.Player.Reset.performed += ctx => ResetToSpawnPoint();
        _inputActions.Player.StepForward.started += ctx => OnStepForward(ctx);
        //_inputActions.Player.StepForward.canceled += ctx => OnStepForward(ctx);
        _inputActions.Player.StepBackward.performed += ctx => OnStepBackward();
        _inputActions.Player.StrafeLeft.performed += ctx => OnStrafeLeft();
        _inputActions.Player.StrafeRight.performed += ctx => OnStrafeRight();
        _inputActions.Player.RotateLeft.performed += ctx => OnRotateLeft();
        _inputActions.Player.RotateRight.performed += ctx => OnRotateRight();
    }

    private void ResetToSpawnPoint()
    {
        _playerRigidbody.position = playerSpawnPoint;
    }

    private void OnStepForward(InputAction.CallbackContext context)
    {
        if (!_isBlockingActionInProgress && !_isBackingUpAfterCollision)
            StartCoroutine(MoveStep(Vector3.forward));
    }

    private void OnStepBackward()
    {
        if (!_isBlockingActionInProgress && !_isBackingUpAfterCollision)
            StartCoroutine(MoveStep(Vector3.back));
    }

    private System.Collections.IEnumerator MoveStep(Vector3 direction)
    {
        var startPosition = transform.position;
        var targetPosition = transform.position + transform.TransformDirection(direction) * moveDistance;

        Debug.Log($"start pos: {startPosition}; target pos: {targetPosition}");

        var elapsedTime = 0f;
        var hitObject = false;

        // Use a layer mask for walls to d
        int wallLayerMask = LayerMask.GetMask("Walls");

        while (elapsedTime < moveDuration && !hitObject)
        {
            var t = elapsedTime / moveDuration;
            var newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Check for collision and smoothly backup if detected
            var collisionLayer = CheckCollisionAhead(transform.position, newPosition, direction, out Vector3 collisionPoint);
            if (collisionLayer == wallLayerMask)
            {
                Debug.Log($"Collision detected at {collisionPoint}, backing up.");
                hitObject = true;
                _isBackingUpAfterCollision = true;

                // Smoothly back up
                yield return StartCoroutine(BackUpSmoothlyForCollision(direction));

                _isBackingUpAfterCollision = false;

                break;
            }


            _playerRigidbody.MovePosition(newPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!hitObject)
        {
            // Move to final position if no collision
            _playerRigidbody.MovePosition(targetPosition);
        }

        // ensure the player is on-grid after movement
        SnapPlayerToGrid();
    }

    public void OnStrafeLeft()
    {
        if (!_isBlockingActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.left)); // Strafe left
        }
    }

    public void OnStrafeRight()
    {
        if (!_isBlockingActionInProgress)
        {
            StartCoroutine(MoveStep(Vector3.right)); // Strafe right
        }
    }

    public void OnRotateLeft()
    {
        if (!_isBlockingActionInProgress)
        {
            StartCoroutine(RotatePlayer(-90f));
        }
    }

    public void OnRotateRight()
    {
        if (!_isBlockingActionInProgress)
        {
            StartCoroutine(RotatePlayer(90f));
        }
    }

    /// <summary>
    /// Check for collisions in the direction of movement and return the layer mask of the object collided with
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="targetPosition"></param>
    /// <param name="direction"></param>
    /// <param name="collisionPoint"></param>
    /// <returns>The layer mask of the object collided with or Default </returns>
    /// <remarks>Only checks for walls and enemies</remarks>
    private int CheckCollisionAhead(Vector3 currentPosition, Vector3 targetPosition, Vector3 direction, out Vector3 collisionPoint)
    {
        collisionPoint = Vector3.zero;

        // Define a layer mask for walls
        int wallLayerMask = LayerMask.GetMask("Walls");
        int enemyLayerMask = LayerMask.GetMask("Enemy");
        float collisionBuffer = 0.10f; // Buffer distance to detect collisions early

        // Calculate ray direction and distance
        Vector3 rayDirection = (targetPosition - currentPosition).normalized;
        float rayDistance = Vector3.Distance(currentPosition, targetPosition) - collisionBuffer;

        // Perform raycast
        if (Physics.Raycast(currentPosition, rayDirection, out RaycastHit hit, rayDistance, wallLayerMask))
        {
            collisionPoint = hit.point;
            return wallLayerMask; // Collision detected
        }
        else if (Physics.Raycast(currentPosition, rayDirection, out hit, rayDistance, enemyLayerMask))
        {
            collisionPoint = hit.point;
            return enemyLayerMask; // Collision detected
        }

        return LayerMaskConstants.Default; // No collision
    }

    public System.Collections.IEnumerator BackUpSmoothlyForCollision(Vector3 forwardDirection)
    {
        float backupDuration = 0.5f; // Duration to move backward
        float elapsedTime = 0f;

        var backupStart = transform.position;
        var backupTarget = backupStart - transform.TransformDirection(forwardDirection).normalized * moveDistance * 0.5f;

        Debug.Log($"Backing up from {backupStart} to {backupTarget}");

        while (elapsedTime < backupDuration)
        {
            var t = elapsedTime / backupDuration;
            _playerRigidbody.MovePosition(Vector3.Lerp(backupStart, backupTarget, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _playerRigidbody.MovePosition(backupTarget);
        SnapPlayerToGrid();
    }

    private void SnapPlayerToGrid()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize;  // Snap to grid on the x-axis
        position.z = Mathf.Round(position.z / gridSize) * gridSize;  // Snap to grid on the z-axis
                                                                     // Keep the y position as is (no rounding)
        transform.position = position;
    }

    private System.Collections.IEnumerator RotatePlayer(float angle)
    {
        _isBlockingActionInProgress = true;

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

        transform.rotation = targetRotation; // Snap to the final rotation
        _isBlockingActionInProgress = false;
    }
}
