using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    public Transform cameraTransform;

    [Header("Keys")]
    public KeyCode toggleKey = KeyCode.F;
    public KeyCode modeKey = KeyCode.G;

    [Header("Base Light")]
    public float baseIntensity = 300f;

    [Header("Cos (Near)")]
    public float cosAngle = 60f;
    public float cosRange = 18f;

    [Header("Pha (Far)")]
    public float phaAngle = 28f;
    public float phaRange = 40f;
    public float phaIntensityMultiplier = 1.4f;

    private bool isPha = false;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        baseIntensity = flashlight.intensity;

        flashlight.enabled = false;
        ApplyLightMode();
    }

    void Update()
    {
        // Bật / tắt đèn
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }

        // Chuyển Cos / Pha
        if (Input.GetKeyDown(modeKey))
        {
            isPha = !isPha;
            ApplyLightMode();
        }

        // Luôn theo hướng camera
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            cameraTransform.rotation,
            Time.deltaTime * 8f
        );
    }

    void ApplyLightMode()
    {
        if (isPha)
        {
            flashlight.spotAngle = phaAngle;
            flashlight.range = phaRange;
            flashlight.intensity = baseIntensity * phaIntensityMultiplier;
        }
        else
        {
            flashlight.spotAngle = cosAngle;
            flashlight.range = cosRange;
            flashlight.intensity = baseIntensity;
        }
    }
}
