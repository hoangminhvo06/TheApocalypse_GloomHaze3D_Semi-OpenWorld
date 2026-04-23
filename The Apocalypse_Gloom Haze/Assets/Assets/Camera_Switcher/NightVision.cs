using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVision : MonoBehaviour
{
    public Volume nightVisionVolume;
    public Volume thermalVisionVolume;

    private bool nightOn = false;
    private bool thermalOn = false;
    private bool isRedMode = false;

    private float targetNight = 0f;
    private float currentNight = 0f;

    private float targetThermal = 0f;
    private float currentThermal = 0f;

    private ColorAdjustments nightColor;

    public float fadeSpeed = 3f;
    public float flickerIntensity = 0.05f;
    public float flickerSpeed = 15f;

    void Start()
    {
        nightVisionVolume.weight = 0f;
        thermalVisionVolume.weight = 0f;

        if (nightVisionVolume.profile.TryGet(out nightColor))
        {
            SetGreenMode();
        }
    }

    void Update()
    {
        // NIGHT VISION
        if (Input.GetKeyDown(KeyCode.N))
        {
            nightOn = !nightOn;
            thermalOn = false;

            targetNight = nightOn ? 1f : 0f;
            targetThermal = 0f;
        }

        // CHANGE COLOR
        if (Input.GetKeyDown(KeyCode.C) && nightOn)
        {
            isRedMode = !isRedMode;

            if (isRedMode)
                SetRedMode();
            else
                SetGreenMode();
        }

        // THERMAL VISION
        if (Input.GetKeyDown(KeyCode.T))
        {
            thermalOn = !thermalOn;
            nightOn = false;

            targetThermal = thermalOn ? 1f : 0f;
            targetNight = 0f;
        }

        // Fade mượt
        currentNight = Mathf.Lerp(currentNight, targetNight, Time.deltaTime * fadeSpeed);
        currentThermal = Mathf.Lerp(currentThermal, targetThermal, Time.deltaTime * fadeSpeed);

        nightVisionVolume.weight = currentNight;
        thermalVisionVolume.weight = currentThermal;

        // Flicker cho night
        if (nightOn && nightColor != null)
        {
            float flicker = 1f + Mathf.Sin(Time.time * flickerSpeed) * flickerIntensity;
            nightColor.postExposure.value = 2f * flicker;
        }
    }

    void SetGreenMode()
    {
        nightColor.colorFilter.value = new Color(0.6f, 1f, 0.6f);
    }

    void SetRedMode()
    {
        nightColor.colorFilter.value = new Color(0.9f, 0.15f, 0.15f);
    }
}