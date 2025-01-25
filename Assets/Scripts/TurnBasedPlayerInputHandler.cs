using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Debug.Log("TurnBasedPlayerInputHandler OnEnable");
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("TurnBasedPlayerInputHandler OnDisable");
        _inputActions.Player.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log("TurnBasedPlayerInputHandler Awake");
        _inputActions = new InputSystem_Actions();
        _playerRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody
        _playerRigidbody.position = playerSpawnPoint;

        _inputActions.Player.Reset.performed += ctx => ResetToSpawnPoint();
        _inputActions.Player.StepForward.performed += ctx => OnStepForward();
        _inputActions.Player.StepBackward.performed += ctx => OnStepBackward();
        _inputActions.Player.StrafeLeft.performed += ctx => OnStrafeLeft();
        _inputActions.Player.StrafeRight.performed += ctx => OnStrafeRight();
        _inputActions.Player.RotateLeft.performed += ctx => OnRotateLeft();
        _inputActions.Player.RotateRight.performed += ctx => OnRotateRight();
    }

    private void ResetToSpawnPoint()
    {
        Debug.Log("ResetToSpawnPoint");
        _playerRigidbody.position = playerSpawnPoint;
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

    private IEnumerator MoveStep(Vector3 direction)
    {

        var startPosition = transform.position;
        var targetPosition = transform.position + transform.TransformDirection(direction) * moveDistance;

        if (IsGridCellAccessible(targetPosition))
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

            TurnManager.PlayerMoved();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Debug.Log("Wall collision detected, backing up.");
            //StartCoroutine(BackUpSmoothlyForCollision());
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

        transform.rotation = targetRotation; // Snap to the final rotation
        _isActionInProgress = false;
    }

    private bool IsGridCellAccessible(Vector3 gridPosition)
    {
        // Cast a ray from the current position to the grid position
        Vector3 direction = (gridPosition - transform.position).normalized;

        // Perform a raycast to check if the path is blocked
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, gridSize))
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
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Vector3.forward, Vector3.forward.normalized * 4);
    }
}
