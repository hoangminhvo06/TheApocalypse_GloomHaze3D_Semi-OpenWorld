using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource.loop = true;
        audioSource.Play(); // Phát âm thanh môi trường liên tục/ Play environment sound constantly
    }
}
