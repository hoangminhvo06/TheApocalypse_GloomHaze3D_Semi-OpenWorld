using UnityEngine;
using Unity.Cinemachine;

public class IFV_EnterTrigger : MonoBehaviour
{
    [Header("References")]
    public Transform driverSeat;
    private GameObject player;

    [Header("Cameras (Unity 6 / Cinemachine 3)")]
    public CinemachineCamera playerCam;
    public CinemachineCamera ifvCam;

    [Header("Vehicle Control")]
    public IFV_Movement vehicleController;
    [Header("Settings")]
    public KeyCode enterKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.F; // ← Phím xuống xe

    [Header("UI")]
    public GameObject enterHintUI;
    public GameObject exitHintUI; // ← UI "Press F to Exit"

    private bool playerInRange = false;
    private bool isInVehicle = false;
    
    // References lưu lại để restore khi xuống xe
    private CharacterController playerController;
    private PlayerController playerScript;
    private Rigidbody playerRb;

    void Start()
    {
        // Camera mặc định: nhân vật
        if (playerCam != null) playerCam.Priority = 20;
        if (ifvCam != null) ifvCam.Priority = 10;

        // Đảm bảo vehicle controller tắt lúc đầu
        if (vehicleController != null)
            vehicleController.enabled = false;

        if (exitHintUI != null)
            exitHintUI.SetActive(false);
    }

    void Update()
    {
        // Nếu NGOÀI xe và trong vùng trigger
        if (!isInVehicle && playerInRange && Input.GetKeyDown(enterKey))
        {
            EnterVehicle();
        }

        // Nếu TRONG xe
        if (isInVehicle && Input.GetKeyDown(exitKey))
        {
            ExitVehicle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerInRange = true;

            if (enterHintUI != null)
                enterHintUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            // Chỉ xóa reference nếu KHÔNG trong xe
            if (!isInVehicle)
                player = null;

            if (enterHintUI != null)
                enterHintUI.SetActive(false);
        }
    }

    void EnterVehicle()
    {
        if (player == null) return;

        isInVehicle = true;

        // ─────────────────────────────────────
        // 1. TẮT ĐIỀU KHIỂN NHÂN VẬT
        // ─────────────────────────────────────
        playerController = player.GetComponent<CharacterController>();
        playerScript = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody>();

        if (playerController) playerController.enabled = false;
        if (playerScript) playerScript.enabled = false;

        if (playerRb)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        // ─────────────────────────────────────
        // 2. ĐẶT NHÂN VẬT VÀO GHẾ LÁI
        // ─────────────────────────────────────
        player.transform.SetParent(driverSeat); // ← Parent vào ghế để theo xe
        player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // Ẩn model nhân vật (optional - nếu muốn)
        var renderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer) renderer.enabled = false;

        // ─────────────────────────────────────
        // 3. CHUYỂN CAMERA
        // ─────────────────────────────────────
        if (playerCam != null) playerCam.Priority = 5;
        if (ifvCam != null) ifvCam.Priority = 20;

        // ─────────────────────────────────────
        // 4. BẬT ĐIỀU KHIỂN XE ← ĐÂY LÀ CHÌA KHÓA!
        // ─────────────────────────────────────
        if (vehicleController != null)
            vehicleController.enabled = true;

        // ─────────────────────────────────────
        // 5. CẬP NHẬT UI
        // ─────────────────────────────────────
        if (enterHintUI != null) enterHintUI.SetActive(false);
        if (exitHintUI != null) exitHintUI.SetActive(true);

        Debug.Log("✅ Entered IFV - Press F to exit");
    }

    void ExitVehicle()
    {
        if (player == null) return;

        isInVehicle = false;

        // ─────────────────────────────────────
        // 1. TẮT ĐIỀU KHIỂN XE
        // ─────────────────────────────────────
        if (vehicleController != null)
            vehicleController.enabled = false;

        // ─────────────────────────────────────
        // 2. LẤY NHÂN VẬT RA KHỎI XE
        // ─────────────────────────────────────
        player.transform.SetParent(null); // ← Bỏ parent

        // Đặt nhân vật ra bên cạnh xe (offset 2m sang phải)
        Vector3 exitPosition = transform.position + transform.right * 3f;
        player.transform.position = exitPosition;
        player.transform.rotation = transform.rotation;

        // Hiện lại model nhân vật
        var renderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer) renderer.enabled = true;

        // ─────────────────────────────────────
        // 3. BẬT LẠI ĐIỀU KHIỂN NHÂN VẬT
        // ─────────────────────────────────────
        if (playerController) playerController.enabled = true;
        if (playerScript) playerScript.enabled = true;

        if (playerRb)
        {
            playerRb.isKinematic = false;
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // ─────────────────────────────────────
        // 4. CHUYỂN CAMERA VỀ NHÂN VẬT
        // ─────────────────────────────────────
        if (playerCam != null) playerCam.Priority = 20;
        if (ifvCam != null) ifvCam.Priority = 10;

        // ─────────────────────────────────────
        // 5. CẬP NHẬT UI
        // ─────────────────────────────────────
        if (exitHintUI != null) exitHintUI.SetActive(false);

        Debug.Log("✅ Exited IFV");
    }
}