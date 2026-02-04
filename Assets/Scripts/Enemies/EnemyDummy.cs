using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Knockback")]
    public float knockbackForce = 4f;
    public float knockbackDuration = 0.1f;

    private Rigidbody2D rb;
    private bool isKnockedBack;
    private float knockbackTimer;
    private Vector2 knockbackVelocity;


    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        currentHealth -= damage;

        ApplyKnockback(attacker);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback(Transform attacker)
    {
        Vector2 direction = (transform.position - attacker.position).normalized;
        knockbackVelocity = direction * knockbackForce;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;
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
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Die()
    {
        HitPause.Freeze(0.06f);
        Destroy(gameObject);
    }
}