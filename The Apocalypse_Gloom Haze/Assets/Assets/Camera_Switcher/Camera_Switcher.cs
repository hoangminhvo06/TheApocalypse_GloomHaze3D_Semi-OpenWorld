using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;

    private bool isThirdPerson = false;

    void Start()
    {
        ActivateFirstPerson();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isThirdPerson)
                ActivateFirstPerson();
            else
                ActivateThirdPerson();
        }
    }

    void ActivateFirstPerson()
    {
        firstPersonCamera.Priority = 20;
        thirdPersonCamera.Priority = 10;
        isThirdPerson = false;
    }

    void ActivateThirdPerson()
    {
        firstPersonCamera.Priority = 10;
        thirdPersonCamera.Priority = 20;
        isThirdPerson = true;
    }
}