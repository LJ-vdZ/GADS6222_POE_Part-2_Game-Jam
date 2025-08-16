using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed = 6f;

    private CharacterController controller;
    [HideInInspector] public Vector3 lastMoveDirection = Vector3.forward;   // default facing

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // record last non-zero movement direction
        if (move.sqrMagnitude > 0.001f)
            lastMoveDirection = move.normalized;

        controller.Move(move * speed * Time.deltaTime);
    }
}