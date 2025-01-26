using UnityEngine;

public class GridMovementAi : MonoBehaviour
{
    public float gridSize;          // Size of each grid cell
    public float detectionRange;    // Range to detect the player
    public Transform player;        // Reference to the player's transform
    public EncounterManager encounterManager;
    public bool canAct;            // whether the monster can act

    private Vector3 targetGridPosition; // Current target grid position
    private bool isChasing = false;     // Whether the monster is chasing the player
    private float wanderTimer = 2f;     // Time between random wander movements
    private float wanderElapsed = 0f;   // Timer to track wander intervals
    private Vector3 currentFacingDirection = Vector3.forward;

    private void Start()
    {
        // Initialize position on the grid
        SnapToGrid();
        targetGridPosition = transform.position;
        RandomizeWanderTimer();
    }

    public void StopMovement()
    {
        isChasing = false;
    }

    public void TurnAroundAndStep()
    {
        currentFacingDirection = -currentFacingDirection;
        targetGridPosition = transform.position + currentFacingDirection * gridSize;
        MoveToTarget();
    }

    public void MovePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void RandomizeWanderTimer()
    {
        wanderTimer = Random.Range(2, 4);
    }

    private void Update()
    {
        if (isChasing)
        {
            // Chasing the player
            ChasePlayer();
        }
        else
        {
            // Wandering
            Wander();

            // Check for the player
            if (Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                isChasing = true; // Switch to chasing state
            }
        }

        MoveToTarget(); // Move toward the current grid target position
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collision triggered!");
        }
        if (other.CompareTag("Enemy"))
        {
            TurnAroundAndStep();
        }
    }

    private void Wander()
    {
        wanderElapsed += Time.deltaTime;

        if (wanderElapsed > wanderTimer && IsAtGridPosition())
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
                currentFacingDirection = validDirections[randomIndex].normalized;
            }
            else
            {
                Debug.Log("No valid moves available!");
            }

            wanderElapsed = 0f;
        }
    }

    private void ChasePlayer()
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

    private void MoveToTarget()
    {
        // Move toward the target grid position
        transform.position = Vector3.MoveTowards(transform.position, targetGridPosition, gridSize * Time.deltaTime);

        // Snap to target position when close enough
        if (Vector3.Distance(transform.position, targetGridPosition) < 1f)
        {
            transform.position = targetGridPosition;
        }
    }

    private bool IsAtGridPosition()
    {
        // Check if the monster is at its current target grid position
        return Vector3.Distance(transform.position, targetGridPosition) < 1f;
    }

    private void SnapToGrid()
    {
        // Align the monster's position to the nearest grid point
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        transform.position = position;
    }

    private bool IsGridCellUnblocked(Vector3 gridPosition)
    {
        // Cast a ray from the current position to the grid position
        Vector3 direction = (gridPosition - transform.position).normalized;

        // Perform a raycast to check if the path is blocked
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, gridSize))
        {
            // Check if the hit object has the tag "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // There's an enemy in the way
                return false;
            }

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
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(targetGridPosition, Vector3.one * gridSize * 0.5f);

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
