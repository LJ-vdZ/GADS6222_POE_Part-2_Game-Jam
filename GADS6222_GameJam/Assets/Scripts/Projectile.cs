using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private int damage;
    private float lifetime = 5f; // Destroy after 5 seconds if no collision

    public void Initialize(Vector3 dir, float spd, int dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
        Destroy(gameObject, lifetime); // Auto-destroy after lifetime
    }

    void Update()
    {
        // Move in straight line
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position);
            }
            Destroy(gameObject); // Destroy on hitting player
        }
        else if (!other.CompareTag("ShooterEnemy") && !other.CompareTag("Mummy"))
        {
            // Destroy on hitting anything except other enemies
            Destroy(gameObject);
        }
    }
}