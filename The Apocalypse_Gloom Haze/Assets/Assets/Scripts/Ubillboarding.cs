using UnityEngine;

public class Ubillboarding : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;//lấy cam chính
    }
    void Update()
    {
        //vị trí theo cam chính
        transform.forward = cam.transform.forward;
    }
}
