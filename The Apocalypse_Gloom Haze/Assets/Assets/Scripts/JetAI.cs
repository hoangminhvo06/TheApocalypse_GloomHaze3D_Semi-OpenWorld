using UnityEngine;

public class JetAI : MonoBehaviour
{
    public Transform[] waypoints; // các điểm máy bay sẽ bay qua
    public float speed = 100f;
    public float turnSpeed = 2f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;

        // xoay máy bay dần về hướng target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // di chuyển máy bay
        transform.position += transform.forward * speed * Time.deltaTime;

        // check tới waypoint
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}
