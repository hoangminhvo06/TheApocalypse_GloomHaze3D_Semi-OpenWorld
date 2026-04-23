using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MosasaurusAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform tailTip;
    public Transform[] fins;
    public Transform jaw;
    
    [Header("River Patrol Settings")]
    public Transform[] riverWaypoints; // Gắn waypoints dọc theo sông
    public float riverPatrolSpeed = 12f;
    public float riverWidth = 30f; // Độ rộng sông để random
    public bool autoFindRiver = true;
    
    [Header("Depth Behavior")]
    public float waterSurfaceY = -12f;
    public float preferredDepthMin = -18f; // Thích bơi ở độ sâu này
    public float preferredDepthMax = -25f;
    public float maxDiveDepth = -35f; // Sâu nhất có thể lặn
    public float depthChangeSpeed = 2f;
    
    [Header("Movement Settings")]
    public float swimSpeed = 12f;
    public float chaseSpeed = 15f;
    public float rotationSpeed = 2.5f;
    public float aggressiveRotationSpeed = 5f;
    
    [Header("Detection - IMPORTANT")]
    public float detectionRangeHorizontal = 50f; // Phát hiện theo chiều ngang
    public float detectionRangeVertical = 30f; // Phát hiện theo chiều dọc (lên xuống)
    public float attackRange = 12f;
    public float losePlayerTime = 8f;
    public LayerMask playerLayer;
    
    [Header("Jaw Animation")]
    public float jawRestAngle = 5f; // Hơi hé miệng khi bơi thường
    public float jawHuntingAngle = 25f; // Há miệng khi săn mồi
    public float jawAttackAngle = 50f; // Há toang khi tấn công
    public float jawAnimSpeed = 4f;
    
    [Header("Advanced Hunting Behavior")]
    public float huntingCircleRadius = 12f;
    public float ambushDepthOffset = -8f; // Lặn sâu hơn để phục kích
    public float stalkerDistance = 15f; // Theo dõi từ xa
    public int bitesToKill = 10;
    public float biteCooldown = 1.8f;
    public float lungeForce = 20f;
    
    [Header("Realistic Behavior")]
    public float idleChance = 0.15f; // 15% dừng lại quan sát
    public float surfaceBreathInterval = 30f; // Nổi lên thở sau 25s
    public float aggressionLevel = 0.5f; // 0=thận trọng, 1=hung dữ
    
    // Private variables
    private Rigidbody rb;
    private Vector3 targetPosition;
    private int currentWaypointIndex = 0;
    
    // Advanced State Machine
    private enum State { 
        RiverPatrol,    // Tuần tra dọc sông
        Dive,           // Lặn sâu
        SurfaceBreathe, // Nổi lên thở
        Stalking,       // Rình rập từ xa
        Hunting,        // Săn mồi tích cực
        Circling,       // Bay vòng quanh mồi
        Ambush,         // Tấn công từ dưới sâu
        Attack,         // Cắn
        Dragging        // Kéo mồi xuống
    }
    private State currentState = State.RiverPatrol;
    
    // Tracking
    private Vector3 lastKnownPlayerPos;
    private float timeSincePlayerSeen = 999f;
    private bool playerDetected = false;
    private float targetDepth;
    
    // Animation
    private Quaternion tailStartRot;
    private Quaternion[] finStartRots;
    private Quaternion jawStartRot;
    private float currentJawAngle = 0f;
    private float targetJawAngle = 0f;
    
    // Behavior timers
    private float stateTimer = 0f;
    private float behaviorTimer = 0f;
    private float breathTimer = 0f;
    private int currentBites = 0;
    private bool canBite = true;
    
    // Movement
    private Vector3 currentVelocity;
    private Vector3 smoothVelocity;
    private float currentSpeed;
    
    void Start()
    {
        InitializeComponents();
        InitializeAnimations();
        
        if (autoFindRiver && (riverWaypoints == null || riverWaypoints.Length == 0))
        {
            AutoGenerateRiverPath();
        }
        
        targetDepth = Random.Range(preferredDepthMin, preferredDepthMax);
        SetNextRiverTarget();
        
        Debug.Log("🌊 MOSASAURUS: Ancient predator awakens in the river...");
    }
    
    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.useGravity = false;
        rb.linearDamping = 1.5f;
        rb.angularDamping = 3f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("✅ Player locked on target!");
            }
        }
    }
    
    void InitializeAnimations()
    {
        if (tailTip != null) tailStartRot = tailTip.localRotation;
        if (jaw != null) jawStartRot = jaw.localRotation;
        
        if (fins != null && fins.Length > 0)
        {
            finStartRots = new Quaternion[fins.Length];
            for (int i = 0; i < fins.Length; i++)
            {
                if (fins[i] != null)
                    finStartRots[i] = fins[i].localRotation;
            }
        }
        
        currentJawAngle = jawRestAngle;
        targetJawAngle = jawRestAngle;
    }
    
    void AutoGenerateRiverPath()
    {
        // Tạo path dọc sông tự động từ vị trí hiện tại
        List<Transform> waypoints = new List<Transform>();
        
        for (int i = -3; i <= 3; i++)
        {
            GameObject wp = new GameObject($"RiverWaypoint_{i}");
            wp.transform.position = transform.position + new Vector3(i * 50f, 0, Random.Range(-10f, 10f));
            wp.transform.parent = transform.parent;
            waypoints.Add(wp.transform);
        }
        
        riverWaypoints = waypoints.ToArray();
        Debug.Log($"🗺️ Auto-generated {riverWaypoints.Length} river waypoints");
    }
    
    void Update()
    {
        if (player == null) return;
        
        stateTimer += Time.deltaTime;
        behaviorTimer += Time.deltaTime;
        breathTimer += Time.deltaTime;
        
        DetectPlayer();
        UpdateAI();
        AnimateCreature();
    }
    
    void FixedUpdate()
    {
        UpdateMovement();
        ClampToRiver();
    }
    
    void DetectPlayer()
    {
        if (player == null) return;
        
        Vector3 toPlayer = player.position - transform.position;
        float horizontalDist = new Vector2(toPlayer.x, toPlayer.z).magnitude;
        float verticalDist = Mathf.Abs(toPlayer.y);
        
        bool isPlayerUnderwater = player.position.y < waterSurfaceY;
        
        // Phát hiện 3D - cả ngang và dọc
        bool inHorizontalRange = horizontalDist < detectionRangeHorizontal;
        bool inVerticalRange = verticalDist < detectionRangeVertical;
        
        if (isPlayerUnderwater && inHorizontalRange && inVerticalRange)
        {
            playerDetected = true;
            lastKnownPlayerPos = player.position;
            timeSincePlayerSeen = 0f;
            
            // Debug line để thấy detection
            Debug.DrawLine(transform.position, player.position, Color.red);
        }
        else
        {
            timeSincePlayerSeen += Time.deltaTime;
            if (timeSincePlayerSeen > losePlayerTime)
            {
                playerDetected = false;
            }
        }
    }
    
    void UpdateAI()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Complex state machine
        switch (currentState)
        {
            case State.RiverPatrol:
                targetJawAngle = jawRestAngle;
                
                if (playerDetected && distToPlayer < detectionRangeHorizontal)
                {
                    float rand = Random.value;
                    if (rand < 0.3f)
                        ChangeState(State.Ambush);
                    else if (rand < 0.6f)
                        ChangeState(State.Stalking);
                    else
                        ChangeState(State.Hunting);
                }
                else if (breathTimer > surfaceBreathInterval)
                {
                    ChangeState(State.SurfaceBreathe);
                }
                else if (behaviorTimer > 15f && Random.value < 0.3f)
                {
                    ChangeState(State.Dive);
                }
                break;
                
            case State.Dive:
                targetJawAngle = jawRestAngle;
                if (transform.position.y <= targetDepth + 2f || stateTimer > 8f)
                    ChangeState(State.RiverPatrol);
                break;
                
            case State.SurfaceBreathe:
                targetJawAngle = jawRestAngle;
                if (transform.position.y >= waterSurfaceY + 2f || stateTimer > 5f)
                {
                    breathTimer = 0f;
                    ChangeState(State.RiverPatrol);
                }
                break;
                
            case State.Stalking:
                targetJawAngle = jawHuntingAngle;
                
                if (!playerDetected || timeSincePlayerSeen > losePlayerTime)
                    ChangeState(State.RiverPatrol);
                else if (distToPlayer < stalkerDistance * 0.5f)
                    ChangeState(State.Hunting);
                else if (stateTimer > 10f)
                    ChangeState(State.Ambush);
                break;
                
            case State.Hunting:
                targetJawAngle = jawHuntingAngle;
                
                if (!playerDetected || timeSincePlayerSeen > losePlayerTime)
                    ChangeState(State.RiverPatrol);
                else if (distToPlayer < attackRange)
                    ChangeState(State.Attack);
                else if (stateTimer > 8f && Random.value < 0.4f)
                    ChangeState(State.Circling);
                break;
                
            case State.Circling:
                targetJawAngle = jawHuntingAngle;
                
                if (distToPlayer < attackRange)
                    ChangeState(State.Attack);
                else if (stateTimer > 5f)
                    ChangeState(State.Hunting);
                break;
                
            case State.Ambush:
                targetJawAngle = jawHuntingAngle;
                
                if (distToPlayer < attackRange * 1.5f || stateTimer > 4f)
                    ChangeState(State.Attack);
                break;
                
            case State.Attack:
                targetJawAngle = jawAttackAngle;
                
                if (distToPlayer > attackRange * 3f)
                    ChangeState(State.Hunting);
                break;
                
            case State.Dragging:
                targetJawAngle = jawAttackAngle;
                
                if (stateTimer > 3f)
                {
                    if (currentBites >= bitesToKill)
                        KillPlayer();
                    ChangeState(State.Hunting);
                }
                break;
        }
    }
    
    void ChangeState(State newState)
    {
        if (currentState == newState) return;
        
        Debug.Log($"🦖 State: {currentState} → {newState}");
        currentState = newState;
        stateTimer = 0f;
        
        switch (newState)
        {
            case State.Dive:
                targetDepth = Random.Range(maxDiveDepth + 5f, preferredDepthMax);
                behaviorTimer = 0f;
                break;
            case State.SurfaceBreathe:
                targetDepth = waterSurfaceY + 2f;
                break;
            case State.RiverPatrol:
                targetDepth = Random.Range(preferredDepthMin, preferredDepthMax);
                SetNextRiverTarget();
                break;
            case State.Ambush:
                targetDepth = lastKnownPlayerPos.y + ambushDepthOffset;
                break;
        }
    }
    
    void UpdateMovement()
    {
        switch (currentState)
        {
            case State.RiverPatrol:
                RiverPatrolMovement();
                break;
            case State.Dive:
            case State.SurfaceBreathe:
                VerticalMovement();
                break;
            case State.Stalking:
                StalkingMovement();
                break;
            case State.Hunting:
                HuntingMovement();
                break;
            case State.Circling:
                CirclingMovement();
                break;
            case State.Ambush:
                AmbushMovement();
                break;
            case State.Attack:
                AttackMovement();
                break;
            case State.Dragging:
                DraggingMovement();
                break;
        }
        
        ApplyDepthControl();
        ApplyWaterPhysics();
    }
    
    void RiverPatrolMovement()
    {
        if (riverWaypoints == null || riverWaypoints.Length == 0) return;
        
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0; // Only horizontal
        float distToTarget = direction.magnitude;
        direction.Normalize();
        
        // Add natural swimming variation
        direction += new Vector3(
            Mathf.Sin(Time.time * 0.3f) * 0.4f,
            0,
            Mathf.Cos(Time.time * 0.4f) * 0.4f
        );
        direction.Normalize();
        
        currentSpeed = riverPatrolSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 2f);
        
        Vector3 newVel = currentVelocity;
        newVel.y = rb.linearVelocity.y; // Keep vertical velocity
        rb.linearVelocity = newVel;
        
        RotateTowards(direction, 1f);
        
        if (distToTarget < 10f)
        {
            SetNextRiverTarget();
        }
    }
    
    void VerticalMovement()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;
        targetPos.y = targetDepth;
        
        Vector3 direction = (targetPos - currentPos).normalized;
        
        currentSpeed = swimSpeed * 1.3f;
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 2f);
        rb.linearVelocity = currentVelocity;
        
        RotateTowards(direction, 1.5f);
    }
    
    void StalkingMovement()
    {
        Vector3 toPlayer = lastKnownPlayerPos - transform.position;
        float dist = toPlayer.magnitude;
        
        // Keep distance
        Vector3 targetPos = lastKnownPlayerPos - toPlayer.normalized * stalkerDistance;
        Vector3 direction = (targetPos - transform.position).normalized;
        
        currentSpeed = swimSpeed * 0.7f;
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 2f);
        rb.linearVelocity = currentVelocity;
        
        RotateTowards(toPlayer.normalized, 1.5f);
        
        Debug.DrawLine(transform.position, lastKnownPlayerPos, Color.yellow);
    }
    
    void HuntingMovement()
    {
        Vector3 direction = (lastKnownPlayerPos - transform.position).normalized;
        
        currentSpeed = chaseSpeed * (0.8f + aggressionLevel * 0.4f);
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 3f);
        rb.linearVelocity = currentVelocity;
        
        RotateTowards(direction, aggressiveRotationSpeed);
        
        Debug.DrawLine(transform.position, lastKnownPlayerPos, Color.red);
    }
    
    void CirclingMovement()
    {
        Vector3 toPlayer = lastKnownPlayerPos - transform.position;
        Vector3 circleDir = Quaternion.Euler(0, stateTimer * 80f, 0) * Vector3.right;
        Vector3 targetPos = lastKnownPlayerPos + circleDir * huntingCircleRadius;
        
        Vector3 direction = (targetPos - transform.position).normalized;
        
        currentSpeed = chaseSpeed * 0.8f;
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 2f);
        rb.linearVelocity = currentVelocity;
        
        RotateTowards(toPlayer.normalized, 2f);
    }
    
    void AmbushMovement()
    {
        Vector3 attackPos = lastKnownPlayerPos;
        attackPos.y += ambushDepthOffset; // Below player
        
        Vector3 direction = (attackPos - transform.position).normalized;
        
        currentSpeed = chaseSpeed * 1.3f;
        currentVelocity = Vector3.Lerp(currentVelocity, direction * currentSpeed, Time.fixedDeltaTime * 4f);
        rb.linearVelocity = currentVelocity;
        
        RotateTowards(direction, aggressiveRotationSpeed);
    }
    
    void AttackMovement()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        RotateTowards(toPlayer, aggressiveRotationSpeed * 1.5f);
        
        if (canBite)
        {
            StartCoroutine(BiteAttack());
        }
    }
    
    void DraggingMovement()
    {
        // Pull player down
        Vector3 deepWater = transform.position;
        deepWater.y = maxDiveDepth;
        
        Vector3 direction = (deepWater - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }
    
    void ApplyDepthControl()
    {
        float currentY = transform.position.y;
        float depthDiff = targetDepth - currentY;
        
        // Smooth depth adjustment
        float verticalForce = depthDiff * depthChangeSpeed;
        rb.AddForce(Vector3.up * verticalForce, ForceMode.Acceleration);
    }
    
    void ApplyWaterPhysics()
    {
        // Clamp to valid swimming area
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, maxDiveDepth, waterSurfaceY + 3f);
        transform.position = pos;
        
        // Natural bobbing
        float bob = Mathf.Sin(Time.time * 0.4f) * 0.15f;
        rb.AddForce(Vector3.up * bob, ForceMode.VelocityChange);
    }
    
    void ClampToRiver()
    {
        // Keep within river bounds
        if (riverWaypoints != null && riverWaypoints.Length > 0)
        {
            Transform nearest = riverWaypoints[currentWaypointIndex];
            float distFromCenter = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(nearest.position.x, 0, nearest.position.z)
            );
            
            if (distFromCenter > riverWidth)
            {
                Vector3 toCenter = (nearest.position - transform.position).normalized;
                toCenter.y = 0;
                rb.AddForce(toCenter * 5f, ForceMode.Acceleration);
            }
        }
    }
    
    void RotateTowards(Vector3 direction, float speedMult = 1f)
    {
        if (direction.magnitude < 0.1f) return;
        
        Quaternion targetRot = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * speedMult * Time.fixedDeltaTime));
    }
    
    IEnumerator BiteAttack()
    {
        canBite = false;
        Debug.Log("🦷 CHOMP!");
        
        // Full jaw open
        targetJawAngle = jawAttackAngle;
        yield return new WaitForSeconds(0.2f);
        
        // Lunge
        rb.AddForce(transform.forward * lungeForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.3f);
        
        // Check hit
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < attackRange * 2f)
        {
            currentBites++;
            Debug.Log($"💀 Bite {currentBites}/{bitesToKill}");
            
            if (currentBites >= bitesToKill / 2)
            {
                ChangeState(State.Dragging);
            }
            
            if (currentBites >= bitesToKill)
            {
                KillPlayer();
                ChangeState(State.RiverPatrol);
            }
        }
        
        yield return new WaitForSeconds(biteCooldown);
        canBite = true;
    }
    
    void KillPlayer()
    {
        Debug.Log("☠️ PLAYER DEVOURED!");
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }
        currentBites = 0;
        playerDetected = false;
    }
    
    void AnimateCreature()
    {
        float time = Time.time;
        float speedFactor = rb.linearVelocity.magnitude / chaseSpeed;
        
        // Jaw animation
        currentJawAngle = Mathf.Lerp(currentJawAngle, targetJawAngle, Time.deltaTime * jawAnimSpeed);
        if (jaw != null)
        {
            jaw.localRotation = jawStartRot * Quaternion.Euler(currentJawAngle, 0, 0);
        }
        
        // Tail
        if (tailTip != null)
        {
            float tailSpeed = 3f * (1f + speedFactor);
            float tailAngle = Mathf.Sin(time * tailSpeed) * 15f * (0.5f + speedFactor * 0.5f);
            tailTip.localRotation = tailStartRot * Quaternion.Euler(0, tailAngle, 0);
        }
        
        // Fins
        if (fins != null && finStartRots != null)
        {
            for (int i = 0; i < fins.Length; i++)
            {
                if (fins[i] != null)
                {
                    float finSpeed = 2f * (1f + speedFactor * 0.5f);
                    float finAngle = Mathf.Sin(time * finSpeed + i * 0.5f) * 10f;
                    fins[i].localRotation = finStartRots[i] * Quaternion.Euler(finAngle, 0, 0);
                }
            }
        }
    }
    
    void SetNextRiverTarget()
    {
        if (riverWaypoints == null || riverWaypoints.Length == 0)
        {
            targetPosition = transform.position + Random.insideUnitSphere * 30f;
            targetPosition.y = targetDepth;
            return;
        }
        
        currentWaypointIndex = (currentWaypointIndex + 1) % riverWaypoints.Length;
        
        Vector3 waypointPos = riverWaypoints[currentWaypointIndex].position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-riverWidth * 0.3f, riverWidth * 0.3f),
            0,
            Random.Range(-riverWidth * 0.3f, riverWidth * 0.3f)
        );
        
        targetPosition = waypointPos + randomOffset;
        targetPosition.y = targetDepth;
        
        Debug.Log($"🎯 Next river target: waypoint {currentWaypointIndex}");
    }
    
    void OnDrawGizmosSelected()
    {
        // Detection sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRangeHorizontal);
        
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Target
        Gizmos.color = Color.cyan;
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawWireSphere(targetPosition, 2f);
        }
        
        // River waypoints
        if (riverWaypoints != null && riverWaypoints.Length > 1)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < riverWaypoints.Length - 1; i++)
            {
                if (riverWaypoints[i] != null && riverWaypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(riverWaypoints[i].position, riverWaypoints[i + 1].position);
                }
            }
        }
        
        // Depth range
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (waterSurfaceY - transform.position.y));
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (transform.position.y - maxDiveDepth));
    }
}