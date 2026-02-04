using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float joystickRadius = 100f; // in screen pixels

    [Header("Room Bounds")]
    [SerializeField] private RoomBounds roomBounds;
    [SerializeField] private float wallPadding = 0.5f;

    [Header("Combat")]
    public int bumpDamage = 1;
    public float knockbackForce = 6f;
    public float knockbackDuration = 0.12f;

    private Rigidbody2D rb;

    private Vector2 touchStartPos;
    private Vector2 moveInput;
    private bool isTouching;

    private bool isKnockedBack;
    private float knockbackTimer;
    private Vector2 knockbackVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
        }
        else
        {
            rb.linearVelocity = moveInput * maxSpeed;
        }
    }


    void HandleInput()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isTouching = false;
            moveInput = Vector2.zero;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isTouching = true;
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 delta = touch.position - touchStartPos;
            moveInput = Vector2.ClampMagnitude(delta / joystickRadius, 1f);
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isTouching = false;
            moveInput = Vector2.zero;
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isTouching)
        {
            Vector2 delta = (Vector2)Input.mousePosition - touchStartPos;
            moveInput = Vector2.ClampMagnitude(delta / joystickRadius, 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
            moveInput = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyDummy enemy = collision.gameObject.GetComponent<EnemyDummy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bumpDamage, transform);
            }

            ApplyKnockback(collision.transform);
        }
    }

    void ApplyKnockback(Transform enemy)
    {
        Vector2 direction = (transform.position - enemy.position).normalized;

        knockbackVelocity = direction * knockbackForce;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;
    }


    void LateUpdate()
    {
        if (roomBounds == null) return;

        float halfWidth = roomBounds.width / 2f;
        float halfHeight = roomBounds.height / 2f;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(
            pos.x,
            roomBounds.transform.position.x - halfWidth + wallPadding,
            roomBounds.transform.position.x + halfWidth - wallPadding
        );

        pos.y = Mathf.Clamp(
            pos.y,
            roomBounds.transform.position.y - halfHeight + wallPadding,
            roomBounds.transform.position.y + halfHeight - wallPadding
        );

        transform.position = pos;
    }
}
