using UnityEngine;

public class DroneGun : MonoBehaviour
{
    public float fireRate = 0.1f;
    public float range = 30f;
    public int damage = 5;

    public Transform muzzle;
    public LineRenderer laser;

    public Color idleColor = Color.green;
    public Color fireColor = Color.red;

    float fireTimer;

    Transform target;

    void Update()
    {
        FindTarget();
        Aim();
        Fire();
        UpdateLaser();
    }

    void FindTarget()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        target = enemy != null ? enemy.transform : null;
    }

    void Aim()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f; // giữ súng ngang

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Fire()
    {
        if (target == null) return;

        fireTimer += Time.deltaTime;
        if (fireTimer < fireRate) return;
        fireTimer = 0f;

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Sau này gắn EnemyHealth
                Debug.Log("Drone hit enemy");
            }
        }
    }

    void UpdateLaser()
    {
        if (laser == null || muzzle == null) return;

        laser.positionCount = 2;
        laser.SetPosition(0, muzzle.position);

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            laser.SetPosition(1, hit.point);
            laser.startColor = fireColor;
            laser.endColor = fireColor;
        }
        else
        {
            laser.SetPosition(1, muzzle.position + muzzle.forward * range);
            laser.startColor = idleColor;
            laser.endColor = idleColor;
        }
    }
}
