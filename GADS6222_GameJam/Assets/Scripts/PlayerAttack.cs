using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int damage = 25;
    public LayerMask enemyLayer;
    public float attackDuration = 0.2f;

    private Playermovement playerMovement;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    void Start()
    {
        playerMovement = GetComponent<Playermovement>();
    }

    void Update()
    {
        // Trigger attack
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
            attackTimer = attackDuration;
            isAttacking = true;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
                isAttacking = false;
        }
    }

    void Attack()
    {/*
       // Vector3 attackDirection = playerMovement.lastMoveDirection;
       // Vector3 attackPosition = transform.position + attackDirection;

      //  Collider[] hits = Physics.OverlapSphere(attackPosition, attackRange, enemyLayer);

        foreach (var hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }*/
    }

    void OnDrawGizmos()
    {
        if (!isAttacking) return;

        var pm = playerMovement != null ? playerMovement : GetComponent<Playermovement>();
        if (pm == null) return;

      //  Vector3 attackDirection = pm.lastMoveDirection;
        //Vector3 attackPosition = transform.position + attackDirection;

        //Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}