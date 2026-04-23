using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float smoothSpeed = 50f;
    public float rotationSpeed = 5f;

    public void SnapToTarget()
    {
        if (target == null) return;
        transform.position = target.position + target.rotation * offset;
        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = desiredRot;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
    }
}