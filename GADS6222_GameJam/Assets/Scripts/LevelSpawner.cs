using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject levelPrefab;

    void Awake()
    {
        if (levelPrefab != null)
        {
            Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
