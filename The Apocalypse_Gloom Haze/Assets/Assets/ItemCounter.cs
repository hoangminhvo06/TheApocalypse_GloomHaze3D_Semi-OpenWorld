using UnityEngine;
using TMPro;
using Fusion;

public class ItemCounter : NetworkBehaviour
{
    [Networked] public int itemCount { get; set; }

    public override void Render()
    {
        if (!HasInputAuthority) return;

        TextMeshProUGUI text = GameObject.Find("ItemCountText")
                               ?.GetComponent<TextMeshProUGUI>();
        if (text != null)
            text.text = $"Items: {itemCount}";
    }

    public void AddItem()
    {
        itemCount++;
    }
}