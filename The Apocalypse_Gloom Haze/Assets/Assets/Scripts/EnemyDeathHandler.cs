// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyDeathHandler : MonoBehaviour
// {
//     public GameObject dropItem;
//     private Animator anim;
//     private NavMeshAgent agent;

//     void Start()
//     {
//         anim = GetComponentInChildren<Animator>();
//         agent = GetComponent<NavMeshAgent>();

//         HealthSystem health = GetComponent<HealthSystem>();
//         //health.onDeath.AddListener(OnDeath);
//     }

//     void OnDeath()
//     {
//         if (agent) agent.isStopped = true;

//         if (anim) anim.SetTrigger("Die");

//         if (dropItem != null)
//             Instantiate(dropItem, transform.position, Quaternion.identity);

//         Destroy(gameObject, 3f); // 3 giây sau xóa enemy
//     }
// }
