using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChaserMovement : EnemyMovementBase
{
    [Header("Chase")]
    public float moveSpeed = 2.5f;
    public float chaseDuration = 1.2f;
    public float idleDuration = 0.6f;

    [Header("Avoidance")]
    public LayerMask obstacleMask;       // set to InnerWall (and optionally OuterWall)
    public float probeDistance = 0.6f;    // how far ahead to raycast
    public float steerStrength = 1.0f;    // how strongly it turns away

    Rigidbody2D rb;

    float stateTimer;
    bool isChasing = true;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        stateTimer = chaseDuration;
        isChasing = true;
    }

    public override void Tick()
    {
        if (core == null || core.player == null) return;

        // If attack (or other system) locked movement, stop.
        if (core.IsMovementLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Update chase/idle timer
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            isChasing = !isChasing;
            stateTimer = isChasing ? chaseDuration : idleDuration;
        }

        if (!isChasing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Base desired direction
        Vector2 toPlayer = (core.player.position - transform.position);
        Vector2 desiredDir = toPlayer.sqrMagnitude > 0.0001f ? toPlayer.normalized : Vector2.zero;

        // Simple avoidance: if forward is blocked, steer left/right
        Vector2 finalDir = GetSteeredDirection(desiredDir);

        rb.linearVelocity = finalDir * moveSpeed;
    }

    Vector2 GetSteeredDirection(Vector2 desiredDir)
    {
        if (desiredDir == Vector2.zero) return Vector2.zero;

        // Raycast forward
        bool blockedForward = Physics2D.Raycast(transform.position, desiredDir, probeDistance, obstacleMask);

        if (!blockedForward)
            return desiredDir;

        // Try left/right alternatives (perpendicular directions)
        Vector2 left = new Vector2(-desiredDir.y, desiredDir.x);
        Vector2 right = new Vector2(desiredDir.y, -desiredDir.x);

        bool blockedLeft = Physics2D.Raycast(transform.position, left, probeDistance, obstacleMask);
        bool blockedRight = Physics2D.Raycast(transform.position, right, probeDistance, obstacleMask);

        // Pick the unblocked side if possible
        if (!blockedLeft && blockedRight)
            return (desiredDir + left * steerStrength).normalized;

        if (!blockedRight && blockedLeft)
            return (desiredDir + right * steerStrength).normalized;

        if (!blockedLeft && !blockedRight)
        {
            // both open: choose the side that points more toward player by default (arbitrary = left)
            return (desiredDir + left * steerStrength).normalized;
        }

        // Fully boxed in: stop
        return Vector2.zero;
    }

    // Optional: debug rays
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 0.3f);
    }
}