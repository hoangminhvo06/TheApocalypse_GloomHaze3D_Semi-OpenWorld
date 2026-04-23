using UnityEngine;

public class SpatialSound : MonoBehaviour
{
    public AudioSource spatialSource;

    void Start()
    {
        spatialSource.spatialBlend = 1.0f; // Âm thanh 3D hoàn toàn/ 3D sound totally
        spatialSource.Play();
    } 
}
