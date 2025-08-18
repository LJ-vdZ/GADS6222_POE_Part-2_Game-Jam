using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnLocation;

    void Awake()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        }
    }
}