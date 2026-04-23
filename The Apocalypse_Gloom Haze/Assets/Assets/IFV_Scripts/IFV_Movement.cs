using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IFV_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 25f;      // Tăng từ 20
    public float rotateSpeed = 120f;   // Tăng từ 100
    public float acceleration = 8000f; // LỰC ĐẨY - quan trọng!

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 2000f;
        rb.linearDamping = 0.1f;  // GIẢM từ 0.5 → ít ma sát hơn
        rb.angularDamping = 1f;   // GIẢM từ 2
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        enabled = false;
    }

    void OnEnable()
    {
        // Reset khi vào xe
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        // ═══════════════════════════════════
        // DI CHUYỂN - TĂNG LỰC ĐẨY
        // ═══════════════════════════════════
        if (Mathf.Abs(move) > 0.1f)
        {
            Vector3 force = transform.forward * move * acceleration; // Tăng lực
            rb.AddForce(force, ForceMode.Force);
            
            Debug.Log($"[MOVE] Input: {move:F2} | Force: {force.magnitude:F0}N | Velocity: {rb.linearVelocity.magnitude:F2}m/s");
        }

        // ═══════════════════════════════════
        // XOAY
        // ═══════════════════════════════════
        if (Mathf.Abs(turn) > 0.1f)
        {
            float rotation = turn * rotateSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, rotation, 0);
        }

        // ═══════════════════════════════════
        // GIỚI HẠN TỐC ĐỘ
        // ═══════════════════════════════════
        if (rb.linearVelocity.magnitude > moveSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        // ═══════════════════════════════════
        // CHỐNG ĐI NGANG (drift)
        // ═══════════════════════════════════
        Vector3 vel = rb.linearVelocity;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float forwardVel = Vector3.Dot(vel, forward);
        float rightVel = Vector3.Dot(vel, right);

        // Loại bỏ vận tốc ngang
        if (Mathf.Abs(rightVel) > 1f)
        {
            vel -= right * rightVel * 0.8f;
            rb.linearVelocity = vel;
        }
    }

    void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}