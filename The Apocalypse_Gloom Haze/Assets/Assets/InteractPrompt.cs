using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    public GameObject promptUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (promptUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            promptUI.SetActive(false);
            // Sau này đặt code tương tác ở đây
        }
    }
}
