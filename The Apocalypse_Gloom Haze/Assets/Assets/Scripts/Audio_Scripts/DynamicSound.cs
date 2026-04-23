using UnityEngine;

public class DynamicSound : MonoBehaviour
{
   public AudioSource engineSource;

    void Update()
    {
        float speed = Input.GetAxis("Vertical"); // Lấy tốc độ từ input/ Get speed from input
        engineSource.pitch = 1f + speed; // Điều chỉnh tốc độ âm/ Optimize sound speed
    }
}
