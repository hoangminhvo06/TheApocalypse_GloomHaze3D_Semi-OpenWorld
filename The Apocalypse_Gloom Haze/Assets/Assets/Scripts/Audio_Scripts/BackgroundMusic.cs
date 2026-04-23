using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
   public AudioSource musicSource;
   
   void Start()
    {
        musicSource.loop = true;
        musicSource.Play(); // Phát nhạc nền/ Play background music
    }
}
