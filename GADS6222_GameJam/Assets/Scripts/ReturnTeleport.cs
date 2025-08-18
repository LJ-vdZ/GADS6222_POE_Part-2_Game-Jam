using UnityEngine;
using System.Collections;

public class ReturnTeleport : MonoBehaviour
{
    [SerializeField] private PlayerTeleportAndGlow firstTrigger; // Assign the first trigger's GameObject in Inspector
    [SerializeField] private GameObject objectToDestroy1; // First object to destroy
    [SerializeField] private GameObject objectToDestroy2; // Second object to destroy

    private GameObject player;
    private bool hasTriggered = false; // Prevent multiple triggers

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

        // Verify first trigger is assigned
        if (firstTrigger == null)
        {
            Debug.LogError("First trigger not assigned in Inspector for " + gameObject.name);
        }

        // Verify objects to destroy are assigned
        if (objectToDestroy1 == null)
        {
            Debug.LogWarning("Object to Destroy 1 not assigned in Inspector for " + gameObject.name);
        }
        if (objectToDestroy2 == null)
        {
            Debug.LogWarning("Object to Destroy 2 not assigned in Inspector for " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Return trigger entered by: " + other.gameObject.name);

        // Check if the object entering the trigger is the player and hasn't triggered yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player detected in return trigger on " + gameObject.name);
            player = other.gameObject;
            hasTriggered = true; // Prevent multiple triggers

            // Verify player has a CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (!controller)
            {
                Debug.LogWarning("Player has no CharacterController! Cannot teleport.");
                return;
            }

            // Teleport player back to original position
            if (firstTrigger != null)
            {
                // Disable CharacterController to allow transform change
                controller.enabled = false;
                player.transform.position = firstTrigger.GetOriginalPlayerPosition();
                player.transform.rotation = firstTrigger.GetOriginalPlayerRotation();
                controller.enabled = true;
                Debug.Log("Player returned to original position: " + player.transform.position);

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

                // Restore original lighting
                firstTrigger.RestoreLighting();

                // Disable the first trigger to prevent looping
                firstTrigger.DisableTrigger();
            }
            else
            {
                Debug.LogError("First trigger not assigned! Cannot return player or restore lighting.");
            }

            // Destroy the assigned objects
            if (objectToDestroy1 != null)
            {
                Destroy(objectToDestroy1);
                Debug.Log("Destroyed object: " + objectToDestroy1.name);
            }
            else
            {
                Debug.LogWarning("Object to Destroy 1 is null, cannot destroy.");
            }

            if (objectToDestroy2 != null)
            {
                Destroy(objectToDestroy2);
                Debug.Log("Destroyed object: " + objectToDestroy2.name);
            }
            else
            {
                Debug.LogWarning("Object to Destroy 2 is null, cannot destroy.");
            }
        }
    }
}