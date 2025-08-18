using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform roomCenter;
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    public int startingEnemies = 5;

    // Objects that should be destroyed when all enemies are dead
    public GameObject objectToDestroy1;
    public GameObject objectToDestroy2;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            SpawnWave();
        }
    }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        // If no more enemies are left, destroy the two assigned objects
        if (activeEnemies.Count == 0)
        {
            if (objectToDestroy1 != null) Destroy(objectToDestroy1);
            if (objectToDestroy2 != null) Destroy(objectToDestroy2);
        }
    }

    void SpawnWave()
    {
<<<<<<< Updated upstream:GADS6222_GameJam/Assets/Spawner.cs
        isSpawning = true;
        yield return new WaitForSeconds(waveDelay);
        SpawnNextWave();
        isSpawning = false;
    }

    void SpawnNextWave()
    {
        currentWave++;
        int enemiesToSpawn = startingEnemies + (enemiesPerWaveIncrement * (currentWave - 1));

        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find player
        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
            return;
        }

        for (int i = 0; i < enemiesToSpawn; i++)
=======
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < startingEnemies; i++)
>>>>>>> Stashed changes:GADS6222_GameJam/Assets/Scripts/Spawner.cs
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            Vector3 spawnPos = roomCenter ? roomCenter.position + randomOffset : transform.position + randomOffset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);

            // Assign player to EnemyAITrigger
            EnemyAITrigger ai = enemy.GetComponent<EnemyAITrigger>();
            if (ai != null)
            {
                ai.player = player.transform;
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy.name} is missing EnemyAITrigger component!");
            }

            // Assign spawner to EnemyHealth
            EnemyHealth notifier = enemy.GetComponent<EnemyHealth>();
            if (notifier != null)
            {
                notifier.spawner = this;
            }
        }
    }
}