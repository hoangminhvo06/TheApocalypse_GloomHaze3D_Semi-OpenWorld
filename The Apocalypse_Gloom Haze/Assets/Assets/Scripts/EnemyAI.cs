// using UnityEngine;
// using UnityEngine.AI;
// using Fusion;

// public class EnemyAI : NetworkBehaviour
// {
//     [Header("Target")]
//     private Transform player;

//     [Header("Detection")]
//     public float detectionRange = 15f;
//     public float fieldOfView = 110f;
//     public float losePlayerDistance = 20f;

//     [Header("Combat")]
//     public float attackRange = 2f;
//     public float attackCooldown = 2f;
//     private float lastAttackTime;

//     [Header("Movement")]
//     public float walkSpeed = 2f;
//     public float runSpeed = 5f;
//     public float patrolWaitTime = 3f;
//     public float patrolRadius = 15f;

//     [Header("Patrol Zone")]
//     public Vector3 patrolZoneCenter;
//     public float patrolZoneRadius = 20f;
//     public bool usePatrolZone = true;

//     [Header("Audio & Effects")]
//     public float soundDetectionRange = 20f;

//     private NavMeshAgent agent;
//     private Animator animator;

//     private enum State { Patrol, Investigate, Chase, Attack, LostPlayer, ReturnToZone }
//     private State currentState = State.Patrol;

//     private Vector3 patrolPoint;
//     private float patrolTimer;
//     private Vector3 lastKnownPlayerPos;
//     private Vector3 spawnPosition;

//     private bool canSeePlayer;
//     private float playerLastSeenTime;
//     private float lostPlayerDuration = 5f;

//     public Transform[] patrolPoints;
//     private int currentPoint = 0;

//     public override void Spawned()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponentInChildren<Animator>();

//         spawnPosition = transform.position;

//         if (patrolZoneCenter == Vector3.zero)
//         {
//             patrolZoneCenter = spawnPosition;
//         }

//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             player = playerObj.transform;
//         }

//         agent.speed = walkSpeed;
//         SetNewPatrolPoint();
//     }

//     public override void FixedUpdateNetwork()
//     {
//         if (!Object.HasStateAuthority) return;

//         if (player == null) return;

//         if (usePatrolZone)
//         {
//             CheckPatrolZoneBoundary();
//         }

//         CheckPlayerDetection();

//         switch (currentState)
//         {
//             case State.Patrol:
//                 PatrolBehavior();
//                 break;

//             case State.Investigate:
//                 InvestigateBehavior();
//                 break;

//             case State.Chase:
//                 ChaseBehavior();
//                 break;

//             case State.Attack:
//                 AttackBehavior();
//                 break;

//             case State.LostPlayer:
//                 LostPlayerBehavior();
//                 break;

//             case State.ReturnToZone:
//                 ReturnToZoneBehavior();
//                 break;
//         }

//         UpdateAnimations();
//     }

//     void CheckPatrolZoneBoundary()
//     {
//         float distanceFromCenter = Vector3.Distance(transform.position, patrolZoneCenter);

//         if (distanceFromCenter > patrolZoneRadius)
//         {
//             if (currentState != State.Chase && currentState != State.Attack)
//             {
//                 currentState = State.ReturnToZone;
//             }
//             else if (currentState == State.Chase)
//             {
//                 float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//                 if (Vector3.Distance(player.position, patrolZoneCenter) > patrolZoneRadius + 5f)
//                 {
//                     currentState = State.ReturnToZone;
//                 }
//             }
//         }
//     }

//     void CheckPlayerDetection()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//         Vector3 directionToPlayer = (player.position - transform.position).normalized;
//         float angle = Vector3.Angle(transform.forward, directionToPlayer);

//         canSeePlayer = false;

//         bool playerInZone = true;

//         if (usePatrolZone)
//         {
//             float playerDistanceFromCenter = Vector3.Distance(player.position, patrolZoneCenter);
//             playerInZone = playerDistanceFromCenter <= (patrolZoneRadius + detectionRange);
//         }

//         if (playerInZone && distanceToPlayer <= detectionRange)
//         {
//             if (angle < fieldOfView / 2)
//             {
//                 RaycastHit hit;

//                 if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
//                 {
//                     if (hit.collider.CompareTag("Player"))
//                     {
//                         canSeePlayer = true;
//                         playerLastSeenTime = Time.time;
//                         lastKnownPlayerPos = player.position;
//                     }
//                 }
//             }
//         }

//         if (canSeePlayer && currentState != State.ReturnToZone)
//         {
//             if (distanceToPlayer <= attackRange)
//             {
//                 currentState = State.Attack;
//             }
//             else
//             {
//                 currentState = State.Chase;
//             }
//         }
//         else if (currentState == State.Chase)
//         {
//             if (Time.time - playerLastSeenTime > 1f)
//             {
//                 currentState = State.LostPlayer;
//             }
//         }
//     }

//     void PatrolBehavior()
//     {
//         if (patrolPoints == null || patrolPoints.Length == 0)
//             return;

//         agent.speed = walkSpeed;

//         if (!agent.pathPending)
//         {
//             agent.SetDestination(patrolPoints[currentPoint].position);
//         }

//         if (!agent.pathPending && agent.remainingDistance < 0.5f)
//         {
//             currentPoint++;

//             if (currentPoint >= patrolPoints.Length)
//             {
//                 currentPoint = 0;
//             }
//         }
//     }

//     void InvestigateBehavior()
//     {
//         agent.speed = walkSpeed;
//         agent.isStopped = false;
//         agent.SetDestination(lastKnownPlayerPos);

//         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//         {
//             patrolTimer += Runner.DeltaTime;

//             if (patrolTimer >= patrolWaitTime)
//             {
//                 currentState = State.Patrol;
//                 SetNewPatrolPoint();
//                 patrolTimer = 0f;
//             }
//         }
//     }

//     void ChaseBehavior()
//     {
//         agent.speed = runSpeed;
//         agent.isStopped = false;
//         agent.SetDestination(player.position);

//         if (canSeePlayer)
//         {
//             lastKnownPlayerPos = player.position;
//         }
//     }

//     void AttackBehavior()
//     {
//         agent.isStopped = true;

//         Vector3 direction = (player.position - transform.position).normalized;
//         direction.y = 0;

//         if (direction != Vector3.zero)
//         {
//             Quaternion lookRotation = Quaternion.LookRotation(direction);
//             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Runner.DeltaTime * 10f);
//         }

//         if (Runner.SimulationTime - lastAttackTime >= attackCooldown)
//         {
//             PerformAttack();
//             lastAttackTime = Runner.SimulationTime;
//         }

//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         if (distanceToPlayer > attackRange + 1f)
//         {
//             currentState = State.Chase;
//         }
//     }

//     void LostPlayerBehavior()
//     {
//         agent.speed = runSpeed;
//         agent.isStopped = false;
//         agent.SetDestination(lastKnownPlayerPos);

//         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//         {
//             patrolTimer += Runner.DeltaTime;

//             if (patrolTimer >= lostPlayerDuration)
//             {
//                 currentState = State.Patrol;
//                 SetNewPatrolPoint();
//                 patrolTimer = 0f;
//             }
//         }
//     }

//     void ReturnToZoneBehavior()
//     {
//         agent.speed = walkSpeed;
//         agent.isStopped = false;

//         agent.SetDestination(patrolZoneCenter);

//         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
//         {
//             currentState = State.Patrol;
//             SetNewPatrolPoint();
//         }
//     }

//     void PerformAttack()
//     {
//         animator.SetTrigger("Attack");

//         if (player != null)
//         {
//             PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

//             if (playerHealth != null)
//             {
//                 playerHealth.TakeDamage(10);
//             }
//         }
//     }

//     void SetNewPatrolPoint()
//     {
//         Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;

//         if (usePatrolZone)
//         {
//             randomDirection += patrolZoneCenter;
//         }
//         else
//         {
//             randomDirection += transform.position;
//         }

//         randomDirection.y = transform.position.y;

//         NavMeshHit hit;

//         if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
//         {
//             patrolPoint = hit.position;
//         }
//     }

//     void UpdateAnimations()
//     {
//         float speed = agent.velocity.magnitude;
//         bool isMoving = speed > 0.1f;

//         bool isChasing = (currentState == State.Chase || currentState == State.LostPlayer);

//         animator.SetFloat("Speed", speed);
//         animator.SetBool("isPatrolling", isMoving && !isChasing);
//         animator.SetBool("isChasing", isChasing);
//     }
// }




// //===========================================//
//     // Code dùng chạy local để dev //

    
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    private Transform player;

    [Header("Detection")]
    public float detectionRange = 15f;
    public float fieldOfView = 110f;
    public float losePlayerDistance = 20f;

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float patrolWaitTime = 3f;
    public float patrolRadius = 15f;

    [Header("Patrol Zone")]
    public Vector3 patrolZoneCenter;
    public float patrolZoneRadius = 20f;
    public bool usePatrolZone = true;

    [Header("Audio & Effects")]
    public float soundDetectionRange = 20f;

    private NavMeshAgent agent;
    private Animator animator;

    private enum State { Patrol, Investigate, Chase, Attack, LostPlayer, ReturnToZone }
    private State currentState = State.Patrol;

    private Vector3 patrolPoint;
    private float patrolTimer;
    private Vector3 lastKnownPlayerPos;
    private Vector3 spawnPosition;

    private bool canSeePlayer;
    private float playerLastSeenTime;
    private float lostPlayerDuration = 5f;

    public Transform[] patrolPoints;
    private int currentPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        spawnPosition = transform.position;

        if (patrolZoneCenter == Vector3.zero)
            patrolZoneCenter = spawnPosition;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        agent.speed = walkSpeed;
        SetNewPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        if (usePatrolZone) CheckPatrolZoneBoundary();
        CheckPlayerDetection();

        switch (currentState)
        {
            case State.Patrol:        PatrolBehavior();       break;
            case State.Investigate:   InvestigateBehavior();  break;
            case State.Chase:         ChaseBehavior();        break;
            case State.Attack:        AttackBehavior();       break;
            case State.LostPlayer:    LostPlayerBehavior();   break;
            case State.ReturnToZone:  ReturnToZoneBehavior(); break;
        }

        UpdateAnimations();
    }

    void CheckPatrolZoneBoundary()
    {
        float distanceFromCenter = Vector3.Distance(transform.position, patrolZoneCenter);

        if (distanceFromCenter > patrolZoneRadius)
        {
            if (currentState != State.Chase && currentState != State.Attack)
                currentState = State.ReturnToZone;
            else if (currentState == State.Chase)
            {
                if (Vector3.Distance(player.position, patrolZoneCenter) > patrolZoneRadius + 5f)
                    currentState = State.ReturnToZone;
            }
        }
    }

    void CheckPlayerDetection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        canSeePlayer = false;

        bool playerInZone = true;
        if (usePatrolZone)
        {
            float playerDistanceFromCenter = Vector3.Distance(player.position, patrolZoneCenter);
            playerInZone = playerDistanceFromCenter <= (patrolZoneRadius + detectionRange);
        }

        if (playerInZone && distanceToPlayer <= detectionRange)
        {
            if (angle < fieldOfView / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        canSeePlayer = true;
                        playerLastSeenTime = Time.time;
                        lastKnownPlayerPos = player.position;
                    }
                }
            }
        }

        if (canSeePlayer && currentState != State.ReturnToZone)
        {
            currentState = distanceToPlayer <= attackRange ? State.Attack : State.Chase;
        }
        else if (currentState == State.Chase)
        {
            if (Time.time - playerLastSeenTime > 1f)
                currentState = State.LostPlayer;
        }
    }

    void PatrolBehavior()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.speed = walkSpeed;

        if (!agent.pathPending)
            agent.SetDestination(patrolPoints[currentPoint].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
                currentPoint = 0;
        }
    }

    void InvestigateBehavior()
    {
        agent.speed = walkSpeed;
        agent.isStopped = false;
        agent.SetDestination(lastKnownPlayerPos);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                currentState = State.Patrol;
                SetNewPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    void ChaseBehavior()
    {
        agent.speed = runSpeed;
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (canSeePlayer)
            lastKnownPlayerPos = player.position;
    }

    void AttackBehavior()
    {
        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange + 1f)
            currentState = State.Chase;
    }

    void LostPlayerBehavior()
    {
        agent.speed = runSpeed;
        agent.isStopped = false;
        agent.SetDestination(lastKnownPlayerPos);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= lostPlayerDuration)
            {
                currentState = State.Patrol;
                SetNewPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    void ReturnToZoneBehavior()
    {
        agent.speed = walkSpeed;
        agent.isStopped = false;
        agent.SetDestination(patrolZoneCenter);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
        {
            currentState = State.Patrol;
            SetNewPatrolPoint();
        }
    }

    void PerformAttack()
    {
        animator.SetTrigger("Attack");

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(10);
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;

        randomDirection += usePatrolZone ? patrolZoneCenter : transform.position;
        randomDirection.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            patrolPoint = hit.position;
    }

    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;
        bool isMoving = speed > 0.1f;
        bool isChasing = (currentState == State.Chase || currentState == State.LostPlayer);

        animator.SetFloat("Speed", speed);
        animator.SetBool("isPatrolling", isMoving && !isChasing);
        animator.SetBool("isChasing", isChasing);
    }
}