using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;  // assign your player prefab in inspector

    void Start()
    {
        if (playerPrefab != null)
        {
            // Instantiate player at (0,0,0) with default rotation
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Player prefab not assigned in PlayerSpawner!");
        }
    }
}