using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    private EnemyAITrigger enemyAI;

    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAITrigger>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetChasing(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetChasing(false);
        }
    }
}
