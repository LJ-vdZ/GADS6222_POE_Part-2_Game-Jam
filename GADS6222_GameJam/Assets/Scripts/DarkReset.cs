using UnityEngine;
using System.Collections;

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

        if (other.CompareTag("Player") && (!hasTriggered || allowMultipleTriggers))
        {
            Debug.Log("Player detected in trigger on " + gameObject.name);
            player = other.gameObject;
            hasTriggered = true;

            // Verify player has a CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (!controller)
            {
                Debug.LogWarning("Player has no CharacterController! Cannot teleport.");
                return;
            }

            if (teleportTarget != null)
            {
                // Disable CharacterController to allow transform change
                controller.enabled = false;
                player.transform.position = teleportTarget.position;
                player.transform.rotation = teleportTarget.rotation;
                controller.enabled = true;
                Debug.Log("Player teleported to " + teleportTarget.position);

                // Notify Playermovement to skip Y-position lock for one frame
                Playermovement movementScript = player.GetComponent<Playermovement>();
                if (movementScript != null)
                {
                    StartCoroutine(movementScript.SkipYLockForFrame());
                }
                else
                {
                    Debug.LogWarning("Playermovement script not found on player!");
                }
            }
            else
            {
                Debug.LogError("Teleport target not assigned in Inspector for " + gameObject.name);
            }
        }
    }
}