using System.Collections;
using UnityEngine;

public class Seth : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 1.5f; 
    public int maxHealth = 100; 
    private int currentHealth;

    [Header("AI Settings")]
    public Transform player; 
    public float detectionRange = 15f; 
    public float obstacleAvoidanceDistance = 1.5f;
    public float obstacleAvoidanceAngle = 45f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int projectileDamage = 15; 
    public float moveDuration = 3f; 
    public float fireDelay = 1f; 
    private bool isFiring = false;

    private Vector3 targetDirection;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackPattern());
    }

    void Update()
    {
        if (player == null || isFiring) return;

        targetDirection = (player.position - transform.position).normalized;
        targetDirection.y = 0f;

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

        Vector3 spawnPos = transform.position + Vector3.up * 1f; 
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
        Destroy(gameObject);
    }
}