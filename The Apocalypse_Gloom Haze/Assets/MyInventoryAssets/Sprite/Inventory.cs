using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();
    public ItemSlotUI[] slots;

    void Awake()
    {
        instance = this;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        UpdateUI();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
