using System.Collections;
using UnityEngine;

public class Seth : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 1.5f; // Slower than Mummy (3f)
    public int maxHealth = 200; // 4x Mummy's health (50)
    private int currentHealth;

    [Header("AI Settings")]
    public Transform player; // Set by Spawner
    public float detectionRange = 15f; // Retained for potential future use
    public float obstacleAvoidanceDistance = 1.5f;
    public float obstacleAvoidanceAngle = 45f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab; // Projectile to instantiate
    public float projectileSpeed = 10f;
    public int projectileDamage = 15; // Damage dealt to player
    public float moveDuration = 3f; // Time to move after firing
    public float fireDelay = 1f; // Delay between each projectile
    private bool isFiring = false;

    [Header("Spawner Reference")]
    public Spawner spawner; // Set by Spawner

    private Vector3 targetDirection;

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(AttackPattern());
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned in ShooterEnemy script on " + gameObject.name);
        }
        if (spawner == null)
        {
            Debug.LogWarning("Spawner not assigned in ShooterEnemy script on " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null || isFiring) return;

        // Always chase the player
        targetDirection = (player.position - transform.position).normalized;
        targetDirection.y = 0f;

        // Move with obstacle avoidance
        Vector3 moveDirection = AvoidObstacles(targetDirection);
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }

    Vector3 AvoidObstacles(Vector3 desiredDirection)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, desiredDirection, out hit, obstacleAvoidanceDistance))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                Vector3 rightDir = Quaternion.Euler(0, obstacleAvoidanceAngle, 0) * desiredDirection;
                Vector3 leftDir = Quaternion.Euler(0, -obstacleAvoidanceAngle, 0) * desiredDirection;

                if (!Physics.Raycast(transform.position, rightDir, obstacleAvoidanceDistance))
                {
                    return rightDir.normalized;
                }
                else if (!Physics.Raycast(transform.position, leftDir, obstacleAvoidanceDistance))
                {
                    return leftDir.normalized;
                }
                return Vector3.zero;
            }
        }
        return desiredDirection.normalized;
    }

    IEnumerator AttackPattern()
    {
        while (true)
        {
            // Stop moving and fire 3 projectiles
            isFiring = true;
            for (int i = 0; i < 3; i++)
            {
                if (player != null)
                {
                    FireProjectile();
                }
                yield return new WaitForSeconds(fireDelay);
            }
            isFiring = false;

            // Move for moveDuration
            yield return new WaitForSeconds(moveDuration);
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null) return;

        // Spawn projectile at enemy's position, aimed at player's current position
        Vector3 spawnPos = transform.position + Vector3.up * 1f; // Slightly above enemy
        Vector3 direction = (player.position - spawnPos).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(direction, projectileSpeed, projectileDamage);
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

        if (knockbackForce > 0f)
        {
            Vector3 knockDir = (transform.position - attackerPosition).normalized;
            knockDir.y = 0f;
            transform.position += knockDir * knockbackForce;
        }
    }

    void Die()
    {
        if (spawner != null)
        {
            spawner.RegisterEnemyDeath(gameObject);
        }
        Destroy(gameObject);
    }
}