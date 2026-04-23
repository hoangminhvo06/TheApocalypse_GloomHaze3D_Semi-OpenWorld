using UnityEngine;
using Fusion;

public class NetworkItem : NetworkBehaviour
{
    // Networked bool — khi thay đổi tự sync cho tất cả client
    [Networked] private NetworkBool isCollected { get; set; }

    public override void Spawned()
    {
        isCollected = false;
         Debug.Log($"Item spawned | HasStateAuthority={Object.HasStateAuthority} | IsMasterClient={Runner.IsSharedModeMasterClient}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Runner.IsSharedModeMasterClient) return;
        if (!other.CompareTag("Player")) return;
        if (isCollected) return;

        isCollected = true;

        ItemCounter counter = other.GetComponent<ItemCounter>();
        if (counter != null)
            counter.AddItem();

        Runner.Despawn(Object);
    }
}