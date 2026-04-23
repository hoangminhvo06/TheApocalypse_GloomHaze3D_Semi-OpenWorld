using UnityEngine;

public class ImpactSound : MonoBehaviour
{
   public AudioSource impactSource;
   public AudioClip impactClip;

    void OnCollisionEnter2D(Collision2D collision)
    {
        impactSource.PlayOneShot(impactClip); // Phát âm thanh va chạm/ Play impact sound
    }
}
