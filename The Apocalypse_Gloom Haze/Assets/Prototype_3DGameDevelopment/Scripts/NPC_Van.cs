using UnityEngine;

public class NPCVan : MonoBehaviour
{
    public float speed = 8f;             // tốc độ bình thường
    public float fleeSpeed = 12f;        // tốc độ khi bỏ chạy
    public float wanderRadius = 15f;     // bán kính đi random
    public float detectionRange = 20f;   // tầm phát hiện cảnh sát
    public Transform player;             // xe cảnh sát (Player)

    private Rigidbody rb;
    private Vector3 targetPos;
    private float timer;

    private enum State { Wander, Flee }
    private State currentState = State.Wander;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PickRandomTarget();

        // Nếu chưa gán player, tự tìm theo tag
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            currentState = (dist <= detectionRange) ? State.Flee : State.Wander;
        }

        if (currentState == State.Wander)
        {
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                PickRandomTarget();
                timer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;

        if (currentState == State.Wander)
        {
            moveDir = (targetPos - transform.position).normalized;
            Move(moveDir, speed);
        }
        else if (currentState == State.Flee && player != null)
        {
            moveDir = (transform.position - player.position).normalized; // chạy ngược
            Move(moveDir, fleeSpeed);
        }
    }

    void Move(Vector3 dir, float spd)
    {
        rb.MovePosition(transform.position + dir * spd * Time.fixedDeltaTime);
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.1f);
    }

    void PickRandomTarget()
    {
        Vector2 rnd = Random.insideUnitCircle * wanderRadius;
        targetPos = new Vector3(transform.position.x + rnd.x, transform.position.y, transform.position.z + rnd.y);
    }
}
