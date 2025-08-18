using UnityEngine;
using UnityEngine.Rendering;

public class PlayerTeleportAndGlow : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // Assign target transform in Inspector
    [SerializeField] private float glowIntensity = 2.0f; // Intensity of player's glow
    [SerializeField] private Color glowColor = Color.white; // Color of player's glow
    [SerializeField] private bool allowMultipleTriggers = false; // Allow repeated triggers?

    private GameObject player;
    private Light playerLight;
    private bool hasTriggered = false;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private Light[] sceneLights;
    private bool[] originalLightStates;
    private Color originalAmbientLight;
    private float originalAmbientIntensity;
    private SphericalHarmonicsL2 originalAmbientProbe;

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
                player.AddComponent<Rigidbody>().isKinematic = true;
            }

            // Store original player position and rotation
            originalPlayerPosition = player.transform.position;
            originalPlayerRotation = player.transform.rotation;

            // Store original lighting settings
            StoreLightingSettings();

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

            // Disable all existing lights in the scene
            DisableAllLights();

            // Make player the only light source
            AddGlowToPlayer();
        }
    }

    void StoreLightingSettings()
    {
        // Store all scene lights and their states
        sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        originalLightStates = new bool[sceneLights.Length];
        for (int i = 0; i < sceneLights.Length; i++)
        {
            originalLightStates[i] = sceneLights[i].enabled;
        }

        // Store ambient light settings
        originalAmbientLight = RenderSettings.ambientLight;
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalAmbientProbe = RenderSettings.ambientProbe;

        Debug.Log("Stored lighting settings: " + sceneLights.Length + " lights, ambient color=" + originalAmbientLight);
    }

    void DisableAllLights()
    {
        // Disable all lights in the scene
        foreach (Light light in sceneLights)
        {
            light.enabled = false;
            Debug.Log("Disabled light: " + light.gameObject.name);
        }

        // Disable ambient light
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = 0f;

        // Disable environment lighting
        RenderSettings.ambientProbe = new UnityEngine.Rendering.SphericalHarmonicsL2();
        Debug.Log("Environment lighting disabled.");
    }

    void AddGlowToPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player is null! Cannot add glow.");
            return;
        }

        // Add a point light to the player if it doesn't already have one
        playerLight = player.GetComponent<Light>();
        if (playerLight == null)
        {
            playerLight = player.AddComponent<Light>();
            Debug.Log("Added Light component to player.");
        }

        // Configure the light to make the player glow
        playerLight.type = LightType.Point;
        playerLight.color = glowColor;
        playerLight.intensity = glowIntensity;
        playerLight.range = 10f; // Adjust range as needed
        playerLight.shadows = LightShadows.Soft;
        Debug.Log("Player light configured: Color=" + glowColor + ", Intensity=" + glowIntensity);
    }

    // Public methods to allow restoration and disabling
    public void RestoreLighting()
    {
        // Restore scene lights
        for (int i = 0; i < sceneLights.Length; i++)
        {
            sceneLights[i].enabled = originalLightStates[i];
            Debug.Log("Restored light: " + sceneLights[i].gameObject.name + " to " + originalLightStates[i]);
        }

        // Restore ambient light
        RenderSettings.ambientLight = originalAmbientLight;
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.ambientProbe = originalAmbientProbe;
        Debug.Log("Restored ambient light: Color=" + originalAmbientLight + ", Intensity=" + originalAmbientIntensity);

        // Remove player light
        if (playerLight != null)
        {
            Destroy(playerLight);
            Debug.Log("Removed player light.");
        }
    }

    public Vector3 GetOriginalPlayerPosition()
    {
        return originalPlayerPosition;
    }

    public Quaternion GetOriginalPlayerRotation()
    {
        return originalPlayerRotation;
    }

    public void DisableTrigger()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
            Debug.Log("Disabled trigger on " + gameObject.name);
        }
    }
}