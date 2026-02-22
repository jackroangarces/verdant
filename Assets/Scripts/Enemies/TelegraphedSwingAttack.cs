using UnityEngine;
using System.Collections;

public class TelegraphedSwingAttack : EnemyAttackBase
{
    [Header("Trigger")]
    public float triggerRange = 1.6f;

    [Header("Timing")]
    public float windupTime = 0.35f;     // telegraph duration (animation later)
    public float activeTime = 0.12f;     // how long hitbox exists
    public float cooldown = 1.0f;

    [Header("Hitbox")]
    public GameObject hitboxPrefab;      // prefab with BoxCollider2D trigger + KillPlayerOnTouch
    public Vector2 hitboxSize = new Vector2(0.9f, 0.9f);
    public float hitboxOffset = 0.8f;    // distance from enemy center

    bool onCooldown;
    bool isAttacking;

    public override void Tick()
    {
        if (core == null || core.player == null) return;
        if (isAttacking || onCooldown) return;

        float dist = Vector2.Distance(transform.position, core.player.position);
        if (dist > triggerRange) return;

        // Start attack
        core.SetAttacking(true);
        isAttacking = true;

        // Cancel movement while winding up and during active frames
        core.LockMovement(windupTime + activeTime);

        // Launch the sequence
        // Note: EnemyAttackBase is a MonoBehaviour, so StartCoroutine is valid
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // Windup (telegraph)
        yield return new WaitForSeconds(windupTime);

        SpawnHitboxTowardPlayer();

        // Active frames
        yield return new WaitForSeconds(activeTime);

        isAttacking = false;
        core.SetAttacking(false);

        // Cooldown
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    void SpawnHitboxTowardPlayer()
    {
        if (hitboxPrefab == null)
        {
            Debug.LogError("TelegraphedSwingAttack: hitboxPrefab not assigned.");
            return;
        }

        Vector2 dir = (core.player.position - transform.position).normalized;
        if (dir.sqrMagnitude < 0.001f) dir = Vector2.right;

        Vector3 pos = transform.position + (Vector3)(dir * hitboxOffset);
        pos.z = 0f;

        GameObject hb = Instantiate(hitboxPrefab, pos, Quaternion.identity);

        // Resize collider to desired size
        var box = hb.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            box.size = hitboxSize;
            box.isTrigger = true;
        }

        // Optional: rotate hitbox to face direction (useful if you later make it rectangular)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        hb.transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(hb, activeTime);
    }
}