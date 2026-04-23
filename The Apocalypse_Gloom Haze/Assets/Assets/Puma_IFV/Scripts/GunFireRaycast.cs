using UnityEngine;

public class GunFireRaycast : MonoBehaviour
{
    [Header("Fire Setup")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRange = 1000f;

    [Header("Debug")]
    [SerializeField] private bool drawDebugRay = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire input received");
            Fire();
        }
    }


    void Fire()
{
    Debug.Log("FirePoint world pos: " + firePoint.position);
    Debug.Log("FirePoint forward: " + firePoint.forward);

    Vector3 origin = firePoint.position;
    Vector3 direction = firePoint.forward;

    Debug.DrawLine(origin, origin + direction * fireRange, Color.red, 2f);

    if (Physics.Raycast(origin, direction, out RaycastHit hit, fireRange))
    {
        Debug.Log("Hit: " + hit.collider.name);
    }
}

}
