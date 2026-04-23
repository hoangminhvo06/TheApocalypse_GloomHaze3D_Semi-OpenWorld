using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PolicePatrollerController : MonoBehaviour
{
    public float speed = 20f;        // tốc độ tiến/lùi
    public float turnSpeed = 50f;    // tốc độ quay

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // tránh lật
    }

    void Update()
    {
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Di chuyển tiến/lùi
        transform.Translate(0, 0, move);

        // Quay trái phải
        transform.Rotate(0, turn, 0);
    }
}
