using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Spawner spawner;
    public EnemyHealthBar healthBar;

    private void Awake()
    {
         
    }

    void Start()
    {
        healthBar = GetComponent<EnemyHealthBar>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        spawner?.RegisterEnemyDeath(gameObject);
        Destroy(gameObject);
    }
}
