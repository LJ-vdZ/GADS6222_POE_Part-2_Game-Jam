using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance { get; private set; }
    public Camera playerCameraForReference;
    CharacterController controller;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Aiming")]
    public Transform weaponTransform; // Player’s weapon/arm

    [Header("Melee Attack")]
    public float attackRange = 2f;
    [Range(0, 180)] public float attackAngle = 60f; // cone angle
    public float attackCooldown = 0.5f;
    public int attackDamage = 20;

    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioSource swingSound;

    private float attackTimer;
    private bool skipYLock = false; // Flag to skip Y-position locking

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // destroy duplicate
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // optional: persist across scenes
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleAttack();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        // Keep player at the same Y position unless skipYLock is true
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        if (!skipYLock)
        {
            Vector3 pos = transform.position;
            pos.y = 0.21f;
            transform.position = pos;
        }
    }

    // Coroutine to skip Y-position lock for one frame
    public IEnumerator SkipYLockForFrame()
    {
        skipYLock = true;
        yield return null; // Wait for one frame
        skipYLock = false;
    }

    void HandleAiming()
    {
        // Mouse aiming
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 dir = hitInfo.point - transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.01f)
                weaponTransform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    void HandleAttack()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SwingWeapon();
            attackTimer = attackCooldown;
        }
    }

    void SwingWeapon()
    {
        if (swingSound != null)
            swingSound.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            return;

        Vector3 dirToMouse = hitInfo.point - transform.position;
        dirToMouse.y = 0f;
        dirToMouse.Normalize();

        Vector3 attackCenter = transform.position + dirToMouse * (attackRange * 0.5f);

        PlayHitEffect(attackCenter);

        bool hitSomething = false;

        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, attackRange * 0.5f);

        foreach (Collider col in hitColliders)
        {
            // Optional: cone check so the attack is still directional
            Vector3 dirToTarget = col.transform.position - transform.position;
            dirToTarget.y = 0f;

            if (Vector3.Angle(dirToMouse, dirToTarget) <= attackAngle / 2f)
            {
                Mummy mummy = col.GetComponent<Mummy>();
                if (mummy != null)
                {
                    mummy.TakeDamage(attackDamage, transform.position);
                    hitSomething = true;
                }

                Seth shooter = col.GetComponent<Seth>();
                if (shooter != null)
                {
                    shooter.TakeDamage(attackDamage, transform.position);
                    hitSomething = true;
                }
            }
        }

        StartCoroutine(SwingAnimation());
    }

    System.Collections.IEnumerator SwingAnimation()
    {
        float swingDuration = 0.2f;
        Quaternion startRot = weaponTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 60f, 0f); // swing 60 degrees
        float timer = 0f;

        while (timer < swingDuration)
        {
            weaponTransform.localRotation = Quaternion.Slerp(startRot, endRot, timer / swingDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        weaponTransform.localRotation = startRot; // reset
    }

    void PlayHitEffect(Vector3 position)
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}