using UnityEngine;

public class ActionSound : MonoBehaviour
{
   public AudioSource audioSource;
   public AudioClip swordSwingClip;

   void SwingSword()
    {
        audioSource.PlayOneShot(swordSwingClip); // Phát âm thanh đánh kiếm/ Play sword sound
    }
}
