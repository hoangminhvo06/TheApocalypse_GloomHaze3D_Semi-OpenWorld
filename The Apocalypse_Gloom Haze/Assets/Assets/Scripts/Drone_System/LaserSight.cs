using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public Transform muzzle;
    public float distance = 30f;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = false;
    }

    void LateUpdate()
    {
        Vector3 start = muzzle.localPosition;
        Vector3 end = start + Vector3.forward * distance;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
