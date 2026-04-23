using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void OnMouseDown()
    {
        Debug.Log("CLICKED ON PICKUP: " + gameObject.name);

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance == NULL");
            return;
        }

        if (item == null)
        {
            Debug.LogError("Item == NULL");
            return;
        }

        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }
}
