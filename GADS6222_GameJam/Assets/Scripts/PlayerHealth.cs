using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
        }
    }
}