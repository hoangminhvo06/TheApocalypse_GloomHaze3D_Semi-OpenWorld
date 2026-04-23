using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        // Xoay icon theo hướng Player
        float yRotation = player.eulerAngles.y;
        transform.localEulerAngles = new Vector3(0, 0, -yRotation);
    }
}
