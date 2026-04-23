using UnityEngine;

public class MagicAttackController : MonoBehaviour
{
    public Transform p;
    private Animator anim;
    private float range = 10f;

    public GameObject fireballPrefab;
    public float fireballSpeed = 20f;
    public float fireballLifetime = 5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawRay(p.position, p.forward * 100, Color.red);

        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("MagicAttack");
            Invoke("ShootFireball", 2.0f);

            RaycastHit hit;
            if (Physics.Raycast(p.position, p.forward + Vector3.up, out hit, range))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }

    void ShootFireball()
    {
        Ray ray = new Ray(p.position, p.forward);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        Vector3 direction = (targetPoint - p.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, p.position, Quaternion.identity);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb == null)
            rb = fireball.AddComponent<Rigidbody>();

        rb.linearVelocity = direction * fireballSpeed;

        Destroy(fireball, fireballLifetime);
    }
}
