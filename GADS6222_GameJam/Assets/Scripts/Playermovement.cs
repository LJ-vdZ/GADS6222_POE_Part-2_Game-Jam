using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance { get; private set; }
    public Camera playerCameraForReference;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Aiming")]
    public Transform weaponTransform; // Player’s weapon/arm

    [Header("Melee Attack")]
    public float attackRange = 2f;
    [Range(0, 180)] public float attackAngle = 60f;
    public float attackCooldown = 0.5f;
    public int attackDamage = 20;

    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioSource swingSound;

    private float attackTimer;
    private CharacterController controller;

    private float groundY;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        controller = GetComponent<CharacterController>();

        groundY = transform.position.y; 
    }

    void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleAttack();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
    }

    void HandleAiming()
    {
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
        // Play swing sound
        if (swingSound != null)
            swingSound.Play();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider col in hitColliders)
        {
            Vector3 dirToTarget = col.transform.position - transform.position;
            dirToTarget.y = 0f;

            if (Vector3.Angle(weaponTransform.forward, dirToTarget) <= attackAngle / 2f)
            {
                Mummy gm = col.GetComponent<Mummy>();
                if (gm != null)
                {
                    gm.TakeDamage(attackDamage, transform.position); 
                    PlayHitEffect(col.transform.position);
                }

                Mummy sm = col.GetComponent<Mummy>();
                if (sm != null)
                {
                    sm.TakeDamage(attackDamage, transform.position); 
                    PlayHitEffect(col.transform.position);
                }
            }
        }

        StartCoroutine(SwingAnimation());
    }

    System.Collections.IEnumerator SwingAnimation()
    {
        float swingDuration = 0.2f;
        Quaternion startRot = weaponTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 60f, 0f);
        float timer = 0f;

        while (timer < swingDuration)
        {
            weaponTransform.localRotation = Quaternion.Slerp(startRot, endRot, timer / swingDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        weaponTransform.localRotation = startRot; 
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