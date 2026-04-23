using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public Toggle enableRemoveButton;
    void Awake()
    {
        if (Instance != null && Instance != this) // Sửa || thành &&
        {
            Destroy(gameObject); // Sửa Destroy(Instance) thành Destroy(gameObject)
            return;
        }
        Instance = this;
    }

    public void Add(Item item)
    {
        items.Add(item);
        DisplayInventory();
    }

    // Là vùng content để hiển thị ds các item
    public Transform itemHolder;
    // Màu item để làm khuôn
    public GameObject itemPrefabl;

    public void DisplayInventory()
    {
        // Dọn kho đồ sạch sẽ
        foreach(Transform item in itemHolder)
        Destroy(item.gameObject);

        foreach(Item item in items) // duyệt
        {
            // Khởi tạo 1 itemPrefab trong itemHolder
            GameObject obj = Instantiate(itemPrefabl, itemHolder);
            // Truy xuất itemName
            TextMeshProUGUI itemName = obj.transform
                                          .Find("ItemName")
                                          .GetComponent<TextMeshProUGUI>();

            // Truy xuất ItemImage
            Image itemImage = obj.transform
                     .Find("ItemImage")
                     .GetComponent<Image>();

            itemName.text = item.itemName;

            itemImage.sprite = item.image;
            obj.GetComponent<ItemUIController>().SetItem(item);

            OnEnableRemoveButton();
            
        }
    }

    public void OnEnableRemoveButton()
    {
        if(enableRemoveButton.isOn)
        {
            foreach (Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                                    .gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                                    .gameObject.SetActive(false);
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
