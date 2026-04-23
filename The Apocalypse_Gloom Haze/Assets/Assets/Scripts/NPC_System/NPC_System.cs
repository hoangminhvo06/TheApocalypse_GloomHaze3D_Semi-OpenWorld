using Unity.VisualScripting;
using UnityEngine;
using DialogueEditor;

public class NPC_System : MonoBehaviour
{
    bool player_detection = false;
    public NPCConversation con;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }

    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Vào vùng hội thoại");
            ConversationManager.Instance.StartConversation(con);
        }
    }
}
