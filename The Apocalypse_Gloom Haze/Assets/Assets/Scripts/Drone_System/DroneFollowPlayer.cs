using UnityEngine;

public class DroneCompanionStable : MonoBehaviour
{
    public float moveSmoothTime = 0.2f;
    public float rotateSpeed = 10f;

    Transform player;
    Vector3 velocity;

    Vector3 localOffset; // 🔑 offset local-space

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogError("Không tìm thấy Player (tag Player)");
            enabled = false;
            return;
        }

        player = p.transform;

        // ✅ OFFSET THEO LOCAL SPACE
        localOffset = player.InverseTransformPoint(transform.position);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 🎯 Anchor XOAY THEO PLAYER
        Vector3 anchor = player.TransformPoint(localOffset);

        // 🚀 Move mượt, KHÔNG văng
        transform.position = Vector3.SmoothDamp(
            transform.position,
            anchor,
            ref velocity,
            moveSmoothTime
        );

        // 🧭 Rotation: khóa X Z, chỉ xoay Y
        Vector3 flatForward = player.forward;
        flatForward.y = 0f;

        if (flatForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatForward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}
