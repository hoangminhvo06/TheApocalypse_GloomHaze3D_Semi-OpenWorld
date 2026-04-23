using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    public Item item;
    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void Remove()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(this.gameObject);
    }

    public void UseItem()
    {
        switch(item.itemType)
        {
            case ItemType.Consumables:
                Debug.Log("Sử dụng vật phẩm tên:" + item.name);
                break;
            case ItemType.Weapons:
                Debug.Log("Sử dụng vũ khí tên:" + item.name);
                break;
            default:
                break;
        }
        Remove();
    }
}
