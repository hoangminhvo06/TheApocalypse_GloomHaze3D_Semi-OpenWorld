using UnityEngine;

public class UISound : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip buttonClickClip;

    public void PlayButtonClickSound()
    {
        uiAudioSource.PlayOneShot(buttonClickClip); // Âm thanh khi nhấn nút/ Sound when clicking button
    }
}
