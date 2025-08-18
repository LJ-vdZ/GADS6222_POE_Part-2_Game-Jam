using UnityEngine;

public class DarkReset : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // Assign target transform in Inspector
    [SerializeField] private bool allowMultipleTriggers = false; // Allow repeated triggers?

    private GameObject player;
    private bool hasTriggered = false; // Prevent multiple triggers unless allowed

    void Start()
    {
        // Verify collider is set as trigger
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("No Collider found on " + gameObject.name + "! Please add a Collider component.");
            return;
        }
        if (!collider.isTrigger)
        {
            Debug.LogWarning("Collider on " + gameObject.name + " is not set as Trigger! Enabling Is Trigger.");
            collider.isTrigger = true;
        }

        // Verify teleport target is assigned
        if (teleportTarget == null)
        {
            Debug.LogError("Teleport target not assigned in Inspector for " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player") && (!hasTriggered || allowMultipleTriggers))
        {
            Debug.Log("Player detected in trigger on " + gameObject.name);
            player = other.gameObject;
            hasTriggered = true;

            // Verify player has a Rigidbody
            if (!player.GetComponent<Rigidbody>())
            {
                Debug.LogWarning("Player has no Rigidbody! Adding one for trigger detection.");
                player.AddComponent<Rigidbody>().isKinematic = true; // Kinematic to avoid physics issues
            }

            // Teleport player to target transform
            if (teleportTarget != null)
            {
                player.transform.position = teleportTarget.position;
                player.transform.rotation = teleportTarget.rotation;
                Debug.Log("Player teleported to " + teleportTarget.position);
            }
            else
            {
                Debug.LogError("Teleport target not assigned in Inspector for " + gameObject.name);
            }
        }
    }
}