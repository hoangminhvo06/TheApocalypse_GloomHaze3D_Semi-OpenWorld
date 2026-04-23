using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // gán InventoryPanel
    private bool isOpen = false;

    void Update()
    {
        // Nhấn I để mở/đóng inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }

    // Gán nút Close
    public void CloseInventory()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
    }
}
