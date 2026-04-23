using UnityEngine;

public class Underwater : MonoBehaviour
{
    [Header("Water Settings")]
    public float waterHeight = -12f;
    public Color underwaterColor = new Color(0.15f, 0.3f, 0.35f);
    public Color deepUnderwaterColor = new Color(0.03f, 0.08f, 0.12f);
    public Color normalFogColor = new Color(0.5f, 0.5f, 0.5f);

    [Header("Fog Settings")]
    public float underwaterFogDensity = 0.02f;
    public float maxDepth = 20f;
    public float transitionSpeed = 4f;

    [Header("Lighting")]
    public Light sunLight;
    public float underwaterLightIntensity = 0.25f;
    public float ambientUnderwaterIntensity = 0.35f;

    [Header("Camera Effects")]
    public float cameraSwayStrength = 0.03f;
    public float cameraSwaySpeed = 0.6f;

    private bool isUnderwater;

    private Color currentFogColor;
    private float currentFogDensity;

    private float originalSunIntensity;
    private float originalAmbientIntensity;
    private Vector3 originalCamLocalPos;

    void Start()
    {
        currentFogColor = RenderSettings.fogColor;
        currentFogDensity = RenderSettings.fogDensity;

        originalAmbientIntensity = RenderSettings.ambientIntensity;

        if (sunLight != null)
            originalSunIntensity = sunLight.intensity;

        if (Camera.main != null)
            originalCamLocalPos = Camera.main.transform.localPosition;
    }

    void Update()
    {
        if (Camera.main == null) return;

        bool shouldBeUnderwater = Camera.main.transform.position.y < waterHeight;
        isUnderwater = shouldBeUnderwater;

        RenderSettings.fog = isUnderwater;

        // ===== DEPTH =====
        float depth = Mathf.Clamp01(
            (waterHeight - Camera.main.transform.position.y) / maxDepth
        );

        // ===== FOG =====
        if (isUnderwater)
        {
            Color targetFog = Color.Lerp(underwaterColor, deepUnderwaterColor, depth);
            float targetDensity = Mathf.Lerp(
                underwaterFogDensity,
                underwaterFogDensity * 2.8f,
                depth
            );

            currentFogColor = Color.Lerp(currentFogColor, targetFog, Time.deltaTime * transitionSpeed);
            currentFogDensity = Mathf.Lerp(currentFogDensity, targetDensity, Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentFogColor = Color.Lerp(currentFogColor, normalFogColor, Time.deltaTime * transitionSpeed);
            currentFogDensity = Mathf.Lerp(currentFogDensity, 0f, Time.deltaTime * transitionSpeed);
        }

        RenderSettings.fogColor = currentFogColor;
        RenderSettings.fogDensity = currentFogDensity;

        // ===== SUN LIGHT =====
        if (sunLight != null)
        {
            float targetSun = isUnderwater
                ? Mathf.Lerp(underwaterLightIntensity, 0.05f, depth)
                : originalSunIntensity;

            sunLight.intensity = Mathf.Lerp(
                sunLight.intensity,
                targetSun,
                Time.deltaTime * transitionSpeed
            );
        }

        // ===== AMBIENT (CHE BẦU TRỜI) =====
        float targetAmbient = isUnderwater
            ? Mathf.Lerp(ambientUnderwaterIntensity, 0.15f, depth)
            : originalAmbientIntensity;

        RenderSettings.ambientIntensity = Mathf.Lerp(
            RenderSettings.ambientIntensity,
            targetAmbient,
            Time.deltaTime * transitionSpeed
        );

        // ===== CAMERA SWAY =====
        if (isUnderwater)
        {
            float sway = Mathf.Sin(Time.time * cameraSwaySpeed) * cameraSwayStrength;
            Camera.main.transform.localPosition =
                originalCamLocalPos + new Vector3(0f, sway, 0f);
        }
        else
        {
            Camera.main.transform.localPosition = Vector3.Lerp(
                Camera.main.transform.localPosition,
                originalCamLocalPos,
                Time.deltaTime * transitionSpeed
            );
        }
    }
}
