using UnityEngine;

public class EnemyAITrigger : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float roamRadius = 5f;
    public float waitTime = 2f;

    private Vector3 roamPoint;
    private float waitTimer;
    private bool isChasing = false;

    void Start()
    {
        SetNewRoamPoint();
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.LookAt(player);
        transform.position += direction * speed * Time.deltaTime;
    }

    void Roam()
    {
        Vector3 lookDir = (roamPoint - transform.position).normalized;
        lookDir.y = 0; 
        if (lookDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }
        Vector3 direction = (roamPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, roamPoint) < 1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                SetNewRoamPoint();
                waitTimer = 0f;
            }
        }
    }

    void SetNewRoamPoint()
    {
        Vector3 newPoint;
        do
        {
            Vector2 randomOffset = Random.insideUnitCircle * roamRadius;
            newPoint = new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y
            );
        }
        while (Vector3.Distance(transform.position, newPoint) < 2f) ; 

        roamPoint = newPoint;
    }

    public void SetChasing(bool chasing)
    {
        isChasing = chasing;
    }
}