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

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWave = 0;
    private bool isTriggered = false;
    private bool isSpawning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(SpawnNextWaveWithDelay());
        }
    }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0 && !isSpawning)
        {
            StartCoroutine(SpawnNextWaveWithDelay());
        }
    }

    IEnumerator SpawnNextWaveWithDelay()
    {
        isSpawning = true;
        yield return new WaitForSeconds(waveDelay);
        SpawnNextWave();
        isSpawning = false;
    }

    void SpawnNextWave()
    {
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

            EnemyAITrigger ai = enemy.GetComponent<EnemyAITrigger>();
            if (ai != null)
            {
                ai.player = player.transform;
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy.name} is missing EnemyAITrigger component!");
            }

            EnemyHealth notifier = enemy.GetComponent<EnemyHealth>();
            if (notifier != null)
            {
                notifier.spawner = this;
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy.name} is missing EnemyHealth component!");
            }
        }
    }
}