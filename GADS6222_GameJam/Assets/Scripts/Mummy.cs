using UnityEngine;

public class Mummy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public int maxHealth = 50;
    public int collisionDamage = 10;
    private int currentHealth;

    [Header("AI Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float roamRadius = 5f;
    public float roamInterval = 3f;
    public float stopDuration = 2f;
    public float obstacleAvoidanceDistance = 1.5f;
    public float obstacleAvoidanceAngle = 45f;

    [Header("Spawner Reference")]
    public Spawner spawner; // Reference to the Spawner

    private Vector3 targetDirection;
    private Vector3 roamTarget;
    private float roamTimer;
    private bool isStopped;
    private float stopTimer;

    void Start()
    {
        currentHealth = maxHealth;
        SetNewRoamTarget();
        roamTimer = roamInterval;
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned in Mummy script on " + gameObject.name);
        }
        if (spawner == null)
        {
            Debug.LogWarning("Spawner not assigned in Mummy script on " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null) return;

        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                isStopped = false;
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            targetDirection = (player.position - transform.position).normalized;
            targetDirection.y = 0f;
        }
        else
        {
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0f || Vector3.Distance(transform.position, roamTarget) < 0.5f)
            {
                SetNewRoamTarget();
                roamTimer = roamInterval;
            }
            targetDirection = (roamTarget - transform.position).normalized;
            targetDirection.y = 0f;
        }

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
            if (hit.collider.gameObject.CompareTag("Player") == false)
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
            spawner.RegisterEnemyDeath(gameObject); // Notify spawner
        }
        Destroy(gameObject);
    }

    void SetNewRoamTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * roamRadius;
        roamTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(collisionDamage, transform.position);
            }

            isStopped = true;
            stopTimer = stopDuration;
        }
    }
}