using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int damage = 25;
    public LayerMask enemyLayer;
    public float attackDuration = 0.2f;  // how long the hitbox and gizmo stay active

    private Playermovement playerMovement;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    void Start()
    {
        playerMovement = GetComponent<Playermovement>();
    }

    void Update()
    {
        // trigger attack
        if (Input.GetMouseButtonDown(0))
        {
            attackTimer = attackDuration;
            isAttacking = true;
        }

        // update attack timer
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            // perform continuous hit detection while timer is active
            Vector3 attackDirection = playerMovement.lastMoveDirection;
            Vector3 attackPosition = transform.position + attackDirection;

            Collider[] hits = Physics.OverlapSphere(attackPosition, attackRange, enemyLayer);

            foreach (var hit in hits)
            {
                EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }

            // stop attacking once timer runs out
            if (attackTimer <= 0f)
                isAttacking = false;
        }
    }

    // Gizmo for visual debugging (only while isAttacking == true)
    void OnDrawGizmos()
    {
        if (!isAttacking) return;

        // fallback if playerMovement hasn't been cached yet (e.g. prefab in editor)
        var pm = playerMovement != null ? playerMovement : GetComponent<Playermovement>();
        if (pm == null) return;

        Vector3 attackDirection = pm.lastMoveDirection;
        Vector3 attackPosition = transform.position + attackDirection;

        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
