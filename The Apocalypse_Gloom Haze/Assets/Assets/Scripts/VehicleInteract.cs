using UnityEngine;

public class VehicleInteract : MonoBehaviour
{
    public GameObject player;         // nhân vật
    public GameObject vehicle;        // xe
    public Camera playerCam;          // camera player
    public Camera vehicleCam;         // camera xe
    public GameObject enterUI;        // UI "Press E to Enter"

    private bool canEnter = false;
    private bool inVehicle = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canEnter = true;
            enterUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canEnter = false;
            enterUI.SetActive(false);
        }
    }

    private void Update()
    {
        if(canEnter && !inVehicle && Input.GetKeyDown(KeyCode.E))
        {
            EnterVehicle();
        }

        if(inVehicle && Input.GetKeyDown(KeyCode.F))
        {
            ExitVehicle();
        }
    }

    void EnterVehicle()
    {
        inVehicle = true;

        // Tắt player, bật xe
        player.SetActive(false);
        vehicle.GetComponent<PolicePatrollerController>().enabled = true;

        // Chuyển camera
        playerCam.enabled = false;
        vehicleCam.enabled = true;

        enterUI.SetActive(false);
    }

    void ExitVehicle()
    {
        inVehicle = false;

        // Đưa player ra bên cạnh xe
        player.transform.position = vehicle.transform.position + vehicle.transform.right * 2f;
        player.SetActive(true);

        // Tắt xe
        vehicle.GetComponent<PolicePatrollerController>().enabled = false;

        // Chuyển camera
        playerCam.enabled = true;
        vehicleCam.enabled = false;
    }
}
