using UnityEngine;
using UnityEngine.Rendering;

public class NightVisionBattery : MonoBehaviour
{
    [Header("Night Vision")]
    public Volume nightVisionVolume;

    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float drainRate = 1f;          // % mỗi giây khi bật
    public float rechargeRate = 0.5f;     // % mỗi giây khi đang sạc
    public float rechargeDelay = 180f;    // 180 = 3 phút realtime

    private float currentBattery;
    private float rechargeTimer = 0f;
    private bool isOn = false;
    private bool isRecharging = false;

    void Start()
    {
        currentBattery = maxBattery;
        nightVisionVolume.weight = 0f;
    }

    void Update()
    {
        HandleInput();
        HandleBattery();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isRecharging) return;
            if (currentBattery <= 0f) return;

            isOn = !isOn;
            nightVisionVolume.weight = isOn ? 1f : 0f;
        }
    }

    void HandleBattery()
    {
        if (isOn)
        {
            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                isOn = false;
                nightVisionVolume.weight = 0f;
                isRecharging = true;
                rechargeTimer = rechargeDelay;
            }
        }

        if (isRecharging)
        {
            rechargeTimer -= Time.deltaTime;

            if (rechargeTimer <= 0f)
            {
                currentBattery += rechargeRate * Time.deltaTime;

                if (currentBattery >= maxBattery)
                {
                    currentBattery = maxBattery;
                    isRecharging = false;
                }
            }
        }

        if (currentBattery <= 20f && isOn)
        {
            nightVisionVolume.weight = Mathf.Lerp(0.8f, 1f, Mathf.PingPong(Time.time * 5f, 1f));
        }
    }

    public float GetBatteryPercent()
    {
        return currentBattery;
    }
}