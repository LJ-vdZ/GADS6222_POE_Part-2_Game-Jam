using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;  

    void Awake()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, new Vector3(0, 0.39f, 0), Quaternion.identity);
        }
    }
}