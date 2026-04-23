using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioSource audioSource; // gán từ Inspector
    public AudioClip clickSound;    // gán file âm thanh từ Assets

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
