using UnityEngine;

public class Mummy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public int maxHealth = 50;
    private int currentHealth;

    [Header("AI Settings")]
    public Transform player;
    public float detectionRange = 10f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowards(player.position);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    public void TakeDamage(int damage, Vector3 attackerPosition, float knockbackForce = 0f)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Apply knockback if > 0
        if (knockbackForce > 0f)
        {
            Vector3 knockDir = (transform.position - attackerPosition).normalized;
            knockDir.y = 0f;
            transform.position += knockDir * knockbackForce;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Optional: detect melee attacks from player
    public void CheckMeleeHit(Transform playerWeapon, float attackRange, float attackAngle, int attackDamage, float knockback)
    {
        Vector3 dirToMob = transform.position - playerWeapon.position;
        dirToMob.y = 0f;

        if (dirToMob.sqrMagnitude <= attackRange * attackRange)
        {
            if (Vector3.Angle(playerWeapon.forward, dirToMob) <= attackAngle / 2f)
            {
                TakeDamage(attackDamage, playerWeapon.position, knockback);
            }
        }
    }
}