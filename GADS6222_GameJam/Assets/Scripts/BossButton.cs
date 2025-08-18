using UnityEngine;

public class BossButton : MonoBehaviour
{
    public GameObject boss;
    public Transform spawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        // Spawn the boss if the boss GameObject is assigned
        if (boss != null)
        {
            Instantiate(boss, spawnLocation.position, Quaternion.identity);
        }

        // Disable the button's Collider to prevent further triggers
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Destroy(this);
    }
}