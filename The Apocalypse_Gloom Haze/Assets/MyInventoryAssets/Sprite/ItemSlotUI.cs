using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon; // Image con trong slot
    private Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.image;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
