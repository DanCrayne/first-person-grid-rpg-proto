using System.Collections;
using UnityEngine;

public class WanderingMonsterMovement : MonoBehaviour
{
    public float gridSize;          // Size of each grid cell
    public float detectionRange;    // Range to detect the player
    public Transform player;        // Reference to the player's transform
    public EncounterManager encounterManager;
    public bool isCurrentTurn = false;
    public float rotationSpeed = 200f;
    public float movementSpeed = 15f;

    private Vector3 currentGridPosition; // Current grid position
    private Vector3 targetGridPosition; // Current target grid position
    private bool isChasing = false;     // Whether the monster is chasing the player
    private Vector3 currentFacingDirection = Vector3.forward;


    private void Start()
    {
        // Initialize position on the grid
        SnapToGrid();
        targetGridPosition = transform.position;
        currentGridPosition = transform.position;
    }

    private void OnEnable()
    {
        EncounterEventNotifier.OnEncounterStart += OnEncounterStart;
        EncounterEventNotifier.OnEncounterEnd += OnEncounterEnd;
    }

    private void OnDisable()
    {
        EncounterEventNotifier.OnEncounterStart -= OnEncounterStart;
        EncounterEventNotifier.OnEncounterEnd -= OnEncounterEnd;
    }

    private void OnEncounterStart()
    {
        Debug.Log("ManagedGridMovementAi: Encounter started!");
    }

    private void OnEncounterEnd()
    {
        Debug.Log("ManagedGridMovementAi: Encounter ended!");
    }

    public void PerformActions()
    {
        StartCoroutine(PerformActionsCoroutine());
    }

    public void MoveBackToLastPosition()
    {
        targetGridPosition = currentGridPosition;
        StartCoroutine(MoveToTargetCoroutine());
    }

    private IEnumerator PerformActionsCoroutine()
    {
        if (isChasing)
        {
            DetermineNextChaseMove();
        }
        else
        {
            DetermineNextMove();
            isChasing = IsPlayerDetected();
        }

        yield return StartCoroutine(MoveToTargetCoroutine());
    }

    private bool IsPlayerDetected()
    {
        // TODO: how would we implement coneshaped vision?
        //       Putting the distance out front might look something like this:
        //if (Vector3.Distance(transform.position + currentFacingDirection * 5, player.position) < detectionRange)
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            return true;
        }

        return false;
    }

    private void DetermineNextMove()
    {
        if (IsAtGridPosition())
        {
            // Pick a random unblocked direction (grid-based)
            Vector3[] directions = {
                new Vector3(gridSize, 0, 0),    // Right
                new Vector3(-gridSize, 0, 0),   // Left
                new Vector3(0, 0, gridSize),    // Up
                new Vector3(0, 0, -gridSize)    // Down
            };

            // Filter directions to only include unblocked ones
            Vector3[] validDirections = FilterUnblockedDirections(directions);

            if (validDirections.Length > 0)
            {
                // Pick a random valid direction
                int randomIndex = Random.Range(0, validDirections.Length);
                targetGridPosition = transform.position + validDirections[randomIndex];
            }
            else
            {
                Debug.Log("No valid moves available!");
            }
        }
    }

    private void DetermineNextChaseMove()
    {
        if (IsAtGridPosition())
        {
            // Calculate the difference between the enemy's position and the player's position
            Vector3 difference = player.position - transform.position;

            // Determine the dominant axis (the larger absolute difference)
            Vector3 nextStep = Vector3.zero;
            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z))
            {
                // Move horizontally toward the player
                nextStep = new Vector3(Mathf.Sign(difference.x) * gridSize, 0, 0);
            }
            else
            {
                // Move vertically toward the player
                nextStep = new Vector3(0, 0, Mathf.Sign(difference.z) * gridSize);
            }

            // Calculate the next grid-aligned position
            Vector3 potentialPosition = transform.position + nextStep;

            // Check if the next position is unblocked
            if (IsGridCellUnblocked(potentialPosition))
            {
                targetGridPosition = potentialPosition;
            }

            // Stop chasing if the player is out of range
            if (Vector3.Distance(transform.position, player.position) > detectionRange * 1.5f)
            {
                isChasing = false;
            }
        }
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        var rotationAngle = 0f;
        var newFacingDirection = currentFacingDirection;

        // Determine the new facing direction and how far we need to rotate
        newFacingDirection = (targetGridPosition - transform.position).normalized;
        rotationAngle = Vector3.SignedAngle(currentFacingDirection, newFacingDirection, Vector3.up);
        // Handle near-180 degree flips (sometimes can make the monster "walk backwards")
        if (Mathf.Abs(rotationAngle) > 179f)
        {
            rotationAngle = rotationAngle > 0 ? 180f : -180f;
        }

        // Rotate monster
        yield return StartCoroutine(RotateMonster(rotationAngle));

        // Move toward the target grid position
        while (Vector3.Distance(transform.position, targetGridPosition) >= 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetGridPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to target position when close enough
        if (Vector3.Distance(transform.position, targetGridPosition) < 1f)
        {
            transform.position = targetGridPosition;
        }

        SnapToGrid(); // ensure alignment
        currentFacingDirection = newFacingDirection;
        currentGridPosition = transform.position;
    }

    private IEnumerator RotateMonster(float angle)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        float elapsedTime = 0f;
        float duration = Mathf.Abs(angle) / rotationSpeed; // Calculate duration based on rotation speed

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Snap to the final rotation
    }

    /// <summary>
    /// Checks if the monster is at its current target grid position
    /// </summary>
    /// <returns>True if at its current target grid position and false otherwise</returns>
    private bool IsAtGridPosition()
    {
        return Vector3.Distance(transform.position, targetGridPosition) < 1f;
    }

    /// <summary>
    /// Aligns the monster's position to the nearest grid point
    /// </summary>
    private void SnapToGrid()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        transform.position = position;
        targetGridPosition = position; // Ensure alignment
    }

    /// <summary>
    /// Checks whether the given grid position is unblocked by certain object types (right now just checks for walls).
    /// </summary>
    /// <param name="gridPosition">The grid position to verify.</param>
    /// <returns>True if the grid position is unblocked and false otherwise</returns>
    private bool IsGridCellUnblocked(Vector3 gridPosition)
    {
        // Cast a ray from the current position to the grid position
        Vector3 direction = (gridPosition - transform.position).normalized;

        // Perform a raycast to check if the path is blocked
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, gridSize))
        {
            // Check if the hit object is a wall
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                return false;
            }
        }

        return true;
    }

    private Vector3[] FilterUnblockedDirections(Vector3[] directions)
    {
        // Return only directions that lead to unblocked grid cells
        System.Collections.Generic.List<Vector3> validDirections = new System.Collections.Generic.List<Vector3>();

        foreach (Vector3 direction in directions)
        {
            Vector3 potentialPosition = transform.position + direction;
            if (IsGridCellUnblocked(potentialPosition))
            {
                validDirections.Add(direction);
            }
        }

        return validDirections.ToArray();
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize the target grid position
        //Gizmos.color = Color.blue;
        //Gizmos.DrawCube(targetGridPosition, Vector3.one * gridSize * 0.5f);

        // Visualize the target grid path
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetGridPosition);

        // Visualize the direction of monster
        Gizmos.color = Color.cyan;
        // Starting position is the monster's current position
        Vector3 start = transform.position;

        // End position is in the facing direction (e.g., 2 units ahead)
        Vector3 end = start + currentFacingDirection * 2f;

        // Draw the line
        Gizmos.DrawLine(start, end);

        // Draw an arrowhead or sphere at the end to make the direction clearer
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end, 0.1f); // Small sphere as an arrowhead
    }
}
