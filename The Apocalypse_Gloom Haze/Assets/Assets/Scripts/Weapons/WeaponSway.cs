using UnityEngine;


public class WeaponSway : MonoBehaviour
{
    public float intensity = 1f;
    public float smoothness = 10f;

    void Update()
    {
        // Lấy dữ liệu di chuyển chuột
        float mouseX = Input.GetAxis("Mouse X") * intensity;
        float mouseY = Input.GetAxis("Mouse Y") * intensity;

        // Tính toán góc xoay mục tiêu (xoay nhẹ theo hướng di chuyển)
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;

        // Xoay súng mượt mà về hướng đó
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothness);
    }
}