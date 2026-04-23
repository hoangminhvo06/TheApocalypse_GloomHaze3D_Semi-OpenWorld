// using UnityEngine;
// using UnityEngine.InputSystem;
// using Fusion.Photon;
// using Fusion;

// [RequireComponent(typeof(CharacterController))]
// public class PlayerController : NetworkBehaviour
// {
//     private InputAction moveAction;
//     private InputAction sprintAction;
//     private InputAction attackAction;
//     private InputAction strafeAction;

//     [Header("Movement Settings")]
//     [Tooltip("Tốc độ đi bộ (WASD)")]
//     public float walkSpeed = 5f;
//     [Tooltip("Tốc độ chạy (WASD + Shift)")]
//     public float sprintSpeed = 8f;
//     [Tooltip("Độ mượt input (thấp = responsive hơn)")]
//     public float movementSmooth = 5f;
//     [Tooltip("Tốc độ tăng tốc")]
//     public float acceleration = 30f;
//     [Tooltip("Tốc độ giảm tốc")]
//     public float deceleration = 15f;

//     [Header("Rotation Settings - AAA Style")]
//     [Tooltip("Tốc độ xoay khi đi bộ (độ/giây)")]
//     public float walkRotationSpeed = 540f;
//     [Tooltip("Tốc độ xoay khi chạy (độ/giây)")]
//     public float sprintRotationSpeed = 720f;
//     [Tooltip("Độ mượt của rotation (0.05-0.3, thấp = nhanh hơn)")]
//     [Range(0.05f, 0.3f)]
//     public float rotationSmoothTime = 0.12f;
//     [Tooltip("Góc tối thiểu để bắt đầu xoay nhanh (độ)")]
//     [Range(5f, 45f)]
//     public float quickTurnThreshold = 120f;
//     [Tooltip("Tốc độ xoay nhanh khi quay 180° (độ/giây)")]
//     public float quickTurnSpeed = 1080f;

//     [Header("Strafe Settings")]
//     public bool enableStrafe = true;
//     public KeyCode strafeKey = KeyCode.Tab;

//     [Header("Controls")]
//     [Tooltip("Giữ phím này + WASD để chạy nhanh")]
//     public KeyCode sprintKey = KeyCode.LeftShift;

//     [Header("Gravity Settings")]
//     public float gravity = -20f;
//     public float groundedGravity = -2f;

//     [Header("Combat Settings")]
//     public float attackRange = 2f;
//     public int attackDamage = 25;
//     public float attackCooldown = 0.5f;

//     [Header("References")]
//     public Transform cameraTransform;
//     public Animator animator;

//     // Private variables
//     private CharacterController controller;
//     private Vector3 input;
//     private Vector3 inputSmooth;
//     private Vector3 moveDirection;
//     private Vector3 velocity;
//     private float currentSpeed = 0f;
//     private float targetSpeed = 0f;

//     // Smooth rotation variables
//     private float currentAngularVelocity;
//     private float targetRotationAngle;
//     private float currentRotationAngle;

//     private bool isStrafing = false;
//     private bool isSprinting = false;
//     private bool isAttacking = false;
//     private float lastAttackTime = 0f;
//     private Quaternion attackRotation;

//     // Animator parameter hashes
//     private int speedHash;
//     private int attackHash;

//     // Combat optimization
//     private Collider[] hitBuffer = new Collider[10];

//     // Lưu vị trí spawn để warp thủ công sau
//     private Vector3 spawnPosition;

//     public override void Spawned()
//     {
//         controller = GetComponent<CharacterController>();

//         if (animator == null)
//             animator = GetComponent<Animator>();

//         speedHash = Animator.StringToHash("Speed");
//         attackHash = Animator.StringToHash("Attack");

//         currentRotationAngle = transform.eulerAngles.y;
//         targetRotationAngle = currentRotationAngle;

//         if (walkSpeed <= 0) walkSpeed = 5f;
//         if (sprintSpeed <= walkSpeed) sprintSpeed = walkSpeed * 2f;

//         if (!HasInputAuthority)
//         {
//             controller.enabled = false;
//             return;
//         }

//         // Lưu vị trí spawn TRƯỚC KHI CharacterController can thiệp
//         spawnPosition = transform.position;
//         Debug.Log($"Saved spawn position: {spawnPosition}");

//         moveAction = new InputAction("Move", InputActionType.Value);
//         moveAction.AddCompositeBinding("2DVector")
//             .With("Up", "<Keyboard>/w")
//             .With("Down", "<Keyboard>/s")
//             .With("Left", "<Keyboard>/a")
//             .With("Right", "<Keyboard>/d");

//         sprintAction = new InputAction("Sprint", binding: "<Keyboard>/leftShift");
//         attackAction = new InputAction("Attack", binding: "<Keyboard>/space");
//         strafeAction = new InputAction("Strafe", binding: "<Keyboard>/tab");

//         moveAction.Enable();
//         sprintAction.Enable();
//         attackAction.Enable();
//         strafeAction.Enable();

//         StartCoroutine(SetupAfterSpawn());
//     }

//     private System.Collections.IEnumerator SetupAfterSpawn()
//     {
//         // Chờ 3 frame để Fusion sync xong
//         yield return null;
//         yield return null;
//         yield return null;

//         // Warp thủ công về đúng vị trí spawn đã lưu
//         controller.enabled = false;
//         transform.position = spawnPosition;
//         controller.enabled = true;

//         Debug.Log($"Warped to: {transform.position}");

//         // Setup camera sau khi warp xong
//         Camera mainCam = Camera.main;
//         if (mainCam != null)
//         {
//             CameraFollow camFollow = mainCam.GetComponent<CameraFollow>();
//             if (camFollow == null)
//                 camFollow = mainCam.gameObject.AddComponent<CameraFollow>();

//             camFollow.target = transform;
//             camFollow.SnapToTarget();
//             cameraTransform = mainCam.transform;

//             Debug.Log($"Camera snap to: {transform.position}");
//         }
//     }

//     void Update()
//     {
//         if (!HasInputAuthority) return;
//         if (controller == null) return;

//         HandleInput();
//         HandleStrafe();
//         HandleSprint();
//         HandleAttack();
//         UpdateMoveDirection();
//         HandleMovement();
//         HandleRotation_AAA();
//         HandleAnimation();
//     }

//     void HandleInput()
//     {
//         Vector2 moveInput = moveAction.ReadValue<Vector2>();
//         input.x = moveInput.x;
//         input.z = moveInput.y;
//         input.y = 0;
//         inputSmooth = Vector3.Lerp(inputSmooth, input, movementSmooth * Time.deltaTime);
//     }

//     void HandleStrafe()
//     {
//         if (enableStrafe && strafeAction.WasPressedThisFrame())
//             isStrafing = !isStrafing;
//     }

//     void HandleSprint()
//     {
//         bool isHoldingSprint = sprintAction.IsPressed();
//         bool hasInput = input.magnitude > 0.1f;
//         bool canSprint = hasInput && controller.isGrounded && !isStrafing && !isAttacking;
//         isSprinting = isHoldingSprint && canSprint;
//     }

//     void HandleAttack()
//     {
//         if (Time.time - lastAttackTime < attackCooldown)
//             return;

//         if (attackAction.WasPressedThisFrame())
//         {
//             if (animator != null)
//                 animator.SetTrigger(attackHash);

//             isAttacking = true;
//             attackRotation = transform.rotation;
//             lastAttackTime = Time.time;

//             Vector3 sphereCenter = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
//             int hitCount = Physics.OverlapSphereNonAlloc(sphereCenter, attackRange, hitBuffer);

//             for (int i = 0; i < hitCount; i++)
//             {
//                 if (hitBuffer[i] != null && hitBuffer[i].CompareTag("Enemy"))
//                 {
//                     EnemyHealth enemyHealth = hitBuffer[i].GetComponent<EnemyHealth>();
//                     if (enemyHealth != null)
//                         enemyHealth.TakeDamage(attackDamage);
//                 }
//             }
//         }
//     }

//     void UpdateMoveDirection()
//     {
//         if (cameraTransform == null) return;

//         if (isAttacking && inputSmooth.magnitude < 0.1f)
//         {
//             moveDirection = Vector3.zero;
//             return;
//         }

//         if (inputSmooth.magnitude <= 0.01f)
//         {
//             moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
//             return;
//         }

//         Vector3 camForward = cameraTransform.forward;
//         Vector3 camRight = cameraTransform.right;
//         camForward.y = 0;
//         camRight.y = 0;
//         camForward.Normalize();
//         camRight.Normalize();

//         Vector3 targetDirection = camRight * inputSmooth.x + camForward * inputSmooth.z;
//         moveDirection = Vector3.Lerp(moveDirection, targetDirection, 15f * Time.deltaTime);
//     }

//     void HandleMovement()
//     {
//         float inputMagnitude = inputSmooth.magnitude;

//         if (isAttacking && inputMagnitude < 0.1f)
//             targetSpeed = 0f;
//         else if (isSprinting)
//             targetSpeed = sprintSpeed;
//         else if (inputMagnitude > 0.01f)
//             targetSpeed = walkSpeed * Mathf.Clamp01(inputMagnitude);
//         else
//             targetSpeed = 0f;

//         float speedDelta = targetSpeed > currentSpeed ? acceleration : deceleration;
//         currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedDelta * Time.deltaTime);

//         if (controller.isGrounded)
//             velocity.y = groundedGravity;
//         else
//             velocity.y += gravity * Time.deltaTime;

//         Vector3 move = moveDirection.normalized * currentSpeed + Vector3.up * velocity.y;
//         controller.Move(move * Time.deltaTime);
//     }

//     void HandleRotation_AAA()
//     {
//         if (isAttacking && inputSmooth.magnitude < 0.1f)
//         {
//             transform.rotation = attackRotation;
//             return;
//         }

//         if (inputSmooth.magnitude < 0.1f) return;

//         Vector3 targetDirection = moveDirection.normalized;
//         if (targetDirection.sqrMagnitude < 0.01f) return;

//         targetRotationAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

//         float angleDifference = Mathf.DeltaAngle(currentRotationAngle, targetRotationAngle);
//         float absAngleDiff = Mathf.Abs(angleDifference);

//         float adaptiveRotationSpeed;
//         if (absAngleDiff > quickTurnThreshold)
//             adaptiveRotationSpeed = quickTurnSpeed;
//         else if (isSprinting)
//             adaptiveRotationSpeed = sprintRotationSpeed;
//         else
//             adaptiveRotationSpeed = walkRotationSpeed;

//         float smoothTime = rotationSmoothTime;
//         if (absAngleDiff > quickTurnThreshold)
//             smoothTime *= 0.6f;

//         currentRotationAngle = Mathf.SmoothDampAngle(
//             currentRotationAngle,
//             targetRotationAngle,
//             ref currentAngularVelocity,
//             smoothTime,
//             adaptiveRotationSpeed
//         );

//         transform.rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);

// #if UNITY_EDITOR
//         if (Input.GetKey(KeyCode.LeftControl))
//             Debug.Log($"Angle Diff: {absAngleDiff:F1}° | Speed: {adaptiveRotationSpeed:F0}°/s | Angular Velocity: {currentAngularVelocity:F1}");
// #endif
//     }

//     void HandleAnimation()
//     {
//         if (animator == null) return;
//         float normalizedSpeed = Mathf.Clamp01(currentSpeed / sprintSpeed);
//         animator.SetFloat(speedHash, normalizedSpeed, 0.15f, Time.deltaTime);
//     }

//     public void UnlockRotation()
//     {
//         isAttacking = false;
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (!Application.isPlaying) return;

//         Gizmos.color = isAttacking ? Color.red : Color.yellow;
//         Vector3 sphereCenter = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
//         Gizmos.DrawWireSphere(sphereCenter, attackRange);

//         if (moveDirection.magnitude > 0.1f)
//         {
//             Gizmos.color = Color.cyan;
//             Gizmos.DrawRay(transform.position + Vector3.up, moveDirection.normalized * 2f);
//         }

//         Vector3 targetDir = Quaternion.Euler(0, targetRotationAngle, 0) * Vector3.forward;
//         Gizmos.color = Color.green;
//         Gizmos.DrawRay(transform.position + Vector3.up * 1.3f, targetDir * 2.5f);

//         Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
//         Vector3 forward = transform.forward;
//         Vector3 leftBound = Quaternion.AngleAxis(-quickTurnThreshold, Vector3.up) * forward;
//         Vector3 rightBound = Quaternion.AngleAxis(quickTurnThreshold, Vector3.up) * forward;
//         Gizmos.DrawRay(transform.position + Vector3.up, leftBound * 3f);
//         Gizmos.DrawRay(transform.position + Vector3.up, rightBound * 3f);
//     }

//     void OnValidate()
//     {
//         if (sprintSpeed <= walkSpeed)
//             sprintSpeed = walkSpeed * 1.6f;

//         walkRotationSpeed = Mathf.Clamp(walkRotationSpeed, 180f, 900f);
//         sprintRotationSpeed = Mathf.Clamp(sprintRotationSpeed, 360f, 1440f);
//         quickTurnSpeed = Mathf.Clamp(quickTurnSpeed, 720f, 1800f);
//         quickTurnThreshold = Mathf.Clamp(quickTurnThreshold, 90f, 150f);
//     }

//     void OnDestroy()
//     {
//         moveAction?.Disable();
//         sprintAction?.Disable();
//         attackAction?.Disable();
//         strafeAction?.Disable();
//     }
// }


// //===========================================//
//     // Code dùng chạy local để dev //


using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction strafeAction;

    [Header("Movement Settings")]
    [Tooltip("Tốc độ đi bộ (WASD)")]
    public float walkSpeed = 5f;
    [Tooltip("Tốc độ chạy (WASD + Shift)")]
    public float sprintSpeed = 8f;
    [Tooltip("Độ mượt input (thấp = responsive hơn)")]
    public float movementSmooth = 5f;
    [Tooltip("Tốc độ tăng tốc")]
    public float acceleration = 30f;
    [Tooltip("Tốc độ giảm tốc")]
    public float deceleration = 15f;

    [Header("Rotation Settings - AAA Style")]
    [Tooltip("Tốc độ xoay khi đi bộ (độ/giây)")]
    public float walkRotationSpeed = 540f;
    [Tooltip("Tốc độ xoay khi chạy (độ/giây)")]
    public float sprintRotationSpeed = 720f;
    [Tooltip("Độ mượt của rotation (0.05-0.3, thấp = nhanh hơn)")]
    [Range(0.05f, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    [Tooltip("Góc tối thiểu để bắt đầu xoay nhanh (độ)")]
    [Range(5f, 45f)]
    public float quickTurnThreshold = 120f;
    [Tooltip("Tốc độ xoay nhanh khi quay 180° (độ/giây)")]
    public float quickTurnSpeed = 1080f;

    [Header("Strafe Settings")]
    public bool enableStrafe = true;
    public KeyCode strafeKey = KeyCode.Tab;

    [Header("Controls")]
    [Tooltip("Giữ phím này + WASD để chạy nhanh")]
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Gravity Settings")]
    public float gravity = -20f;
    public float groundedGravity = -2f;

    [Header("Combat Settings")]
    public float attackRange = 2f;
    public int attackDamage = 25;
    public float attackCooldown = 0.5f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private Vector3 input;
    private Vector3 inputSmooth;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    private float currentAngularVelocity;
    private float targetRotationAngle;
    private float currentRotationAngle;

    private bool isStrafing = false;
    private bool isSprinting = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Quaternion attackRotation;

    private int speedHash;
    private int attackHash;

    private Collider[] hitBuffer = new Collider[10];

    void Start()
    {
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        sprintAction = new InputAction("Sprint", binding: "<Keyboard>/leftShift");
        attackAction = new InputAction("Attack", binding: "<Keyboard>/space");
        strafeAction = new InputAction("Strafe", binding: "<Keyboard>/tab");

        moveAction.Enable();
        sprintAction.Enable();
        attackAction.Enable();
        strafeAction.Enable();

        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;

        speedHash = Animator.StringToHash("Speed");
        attackHash = Animator.StringToHash("Attack");

        currentRotationAngle = transform.eulerAngles.y;
        targetRotationAngle = currentRotationAngle;

        if (walkSpeed <= 0) walkSpeed = 5f;
        if (sprintSpeed <= walkSpeed) sprintSpeed = walkSpeed * 2f;
    }

    void Update()
    {
        HandleInput();
        HandleStrafe();
        HandleSprint();
        HandleAttack();
        UpdateMoveDirection();
        HandleMovement();
        HandleRotation_AAA();
        HandleAnimation();
    }

    void HandleInput()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        input.x = moveInput.x;
        input.z = moveInput.y;
        input.y = 0;
        inputSmooth = Vector3.Lerp(inputSmooth, input, movementSmooth * Time.deltaTime);
    }

    void HandleStrafe()
    {
        if (enableStrafe && strafeAction.WasPressedThisFrame())
            isStrafing = !isStrafing;
    }

    void HandleSprint()
    {
        bool isHoldingSprint = sprintAction.IsPressed();
        bool hasInput = input.magnitude > 0.1f;
        bool canSprint = hasInput && controller.isGrounded && !isStrafing && !isAttacking;
        isSprinting = isHoldingSprint && canSprint;
    }

    void HandleAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        if (attackAction.WasPressedThisFrame())
        {
            if (animator != null)
                animator.SetTrigger(attackHash);

            isAttacking = true;
            attackRotation = transform.rotation;
            lastAttackTime = Time.time;

            Vector3 sphereCenter = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
            int hitCount = Physics.OverlapSphereNonAlloc(sphereCenter, attackRange, hitBuffer);

            for (int i = 0; i < hitCount; i++)
            {
                if (hitBuffer[i] != null && hitBuffer[i].CompareTag("Enemy"))
                {
                    EnemyHealth enemyHealth = hitBuffer[i].GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                        enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    void UpdateMoveDirection()
    {
        if (cameraTransform == null) return;

        if (isAttacking && inputSmooth.magnitude < 0.1f)
        {
            moveDirection = Vector3.zero;
            return;
        }

        if (inputSmooth.magnitude <= 0.01f)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDirection = camRight * inputSmooth.x + camForward * inputSmooth.z;
        moveDirection = Vector3.Lerp(moveDirection, targetDirection, 15f * Time.deltaTime);
    }

    void HandleMovement()
    {
        float inputMagnitude = inputSmooth.magnitude;

        if (isAttacking && inputMagnitude < 0.1f)
            targetSpeed = 0f;
        else if (isSprinting)
            targetSpeed = sprintSpeed;
        else if (inputMagnitude > 0.01f)
            targetSpeed = walkSpeed * Mathf.Clamp01(inputMagnitude);
        else
            targetSpeed = 0f;

        float speedDelta = targetSpeed > currentSpeed ? acceleration : deceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedDelta * Time.deltaTime);

        if (controller.isGrounded)
            velocity.y = groundedGravity;
        else
            velocity.y += gravity * Time.deltaTime;

        Vector3 move = moveDirection.normalized * currentSpeed + Vector3.up * velocity.y;
        controller.Move(move * Time.deltaTime);
    }

    void HandleRotation_AAA()
    {
        if (isAttacking && inputSmooth.magnitude < 0.1f)
        {
            transform.rotation = attackRotation;
            return;
        }

        if (inputSmooth.magnitude < 0.1f) return;

        Vector3 targetDirection = moveDirection.normalized;
        if (targetDirection.sqrMagnitude < 0.01f) return;

        targetRotationAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        float angleDifference = Mathf.DeltaAngle(currentRotationAngle, targetRotationAngle);
        float absAngleDiff = Mathf.Abs(angleDifference);

        float adaptiveRotationSpeed;
        if (absAngleDiff > quickTurnThreshold)
            adaptiveRotationSpeed = quickTurnSpeed;
        else if (isSprinting)
            adaptiveRotationSpeed = sprintRotationSpeed;
        else
            adaptiveRotationSpeed = walkRotationSpeed;

        float smoothTime = rotationSmoothTime;
        if (absAngleDiff > quickTurnThreshold)
            smoothTime *= 0.6f;

        currentRotationAngle = Mathf.SmoothDampAngle(
            currentRotationAngle,
            targetRotationAngle,
            ref currentAngularVelocity,
            smoothTime,
            adaptiveRotationSpeed
        );

        transform.rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl))
            Debug.Log($"Angle Diff: {absAngleDiff:F1}° | Speed: {adaptiveRotationSpeed:F0}°/s | Angular Velocity: {currentAngularVelocity:F1}");
#endif
    }

    void HandleAnimation()
    {
        if (animator == null) return;
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / sprintSpeed);
        animator.SetFloat(speedHash, normalizedSpeed, 0.15f, Time.deltaTime);
    }

    public void UnlockRotation()
    {
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = isAttacking ? Color.red : Color.yellow;
        Vector3 sphereCenter = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(sphereCenter, attackRange);

        if (moveDirection.magnitude > 0.1f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position + Vector3.up, moveDirection.normalized * 2f);
        }

        Vector3 targetDir = Quaternion.Euler(0, targetRotationAngle, 0) * Vector3.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.3f, targetDir * 2.5f);

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Vector3 forward = transform.forward;
        Vector3 leftBound = Quaternion.AngleAxis(-quickTurnThreshold, Vector3.up) * forward;
        Vector3 rightBound = Quaternion.AngleAxis(quickTurnThreshold, Vector3.up) * forward;
        Gizmos.DrawRay(transform.position + Vector3.up, leftBound * 3f);
        Gizmos.DrawRay(transform.position + Vector3.up, rightBound * 3f);
    }

    void OnValidate()
    {
        if (sprintSpeed <= walkSpeed)
            sprintSpeed = walkSpeed * 1.6f;

        walkRotationSpeed = Mathf.Clamp(walkRotationSpeed, 180f, 900f);
        sprintRotationSpeed = Mathf.Clamp(sprintRotationSpeed, 360f, 1440f);
        quickTurnSpeed = Mathf.Clamp(quickTurnSpeed, 720f, 1800f);
        quickTurnThreshold = Mathf.Clamp(quickTurnThreshold, 90f, 150f);
    }

    void OnDestroy()
    {
        moveAction?.Disable();
        sprintAction?.Disable();
        attackAction?.Disable();
        strafeAction?.Disable();
    }
}