using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform roomCenter;
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    public int startingEnemies = 5;
    public int enemiesPerWaveIncrement = 2;
    public float waveDelay = 3f;
    public GameObject objectToDestroy1; // First object to destroy
    public GameObject objectToDestroy2; // Second object to destroy

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWave = 0;
    private bool isTriggered = false;
    private bool isSpawning = false;
    private bool isSpawnerActive = true; // Controls whether spawner can spawn new waves

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player") && isSpawnerActive)
        {
            isTriggered = true;
            StartCoroutine(SpawnNextWaveWithDelay());
        }
    }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0 && !isSpawning && isSpawnerActive)
        {
            // Destroy the assigned objects if all enemies are dead
            if (objectToDestroy1 != null)
            {
                Destroy(objectToDestroy1);
            }
            if (objectToDestroy2 != null)
            {
                Destroy(objectToDestroy2); // Fixed typo (was objectTonero)
            }
            isSpawnerActive = false; // Disable spawner after destroying objects
        }
    }

    IEnumerator SpawnNextWaveWithDelay()
    {
        if (!isSpawnerActive)
        {
            yield break; // Exit coroutine if spawner is disabled
        }
        isSpawning = true;
        yield return new WaitForSeconds(waveDelay);
        SpawnNextWave();
        isSpawning = false;
    }

    void SpawnNextWave()
    {
        if (!isSpawnerActive)
        {
            return; // Do not spawn if spawner is disabled
        }

        currentWave++;
        int enemiesToSpawn = startingEnemies + (enemiesPerWaveIncrement * (currentWave - 1));

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
            return;
        }

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            Vector3 spawnPos = roomCenter ? roomCenter.position + randomOffset : transform.position + randomOffset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);

            Mummy ai = enemy.GetComponent<Mummy>();
            if (ai != null)
            {
                ai.player = player.transform;
                ai.spawner = this; // Assign spawner reference
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy.name} is missing Mummy component!");
            }
        }
    }
}