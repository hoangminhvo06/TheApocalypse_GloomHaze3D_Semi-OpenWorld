using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 10;
    public float hitCooldown = 0.05f;
    private float lastHitTime;

    void OnTriggerStay(Collider other)
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        if (other.CompareTag("Enemy"))
        {
            var health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastHitTime = Time.time;
            }
        }
    }
}
