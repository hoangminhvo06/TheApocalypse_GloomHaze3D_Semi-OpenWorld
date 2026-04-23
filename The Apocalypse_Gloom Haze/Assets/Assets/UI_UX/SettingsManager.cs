using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle lowToggle;
    public Toggle mediumToggle;
    public Toggle highToggle;

    // Biến này để xác định mục đầu tiên (PC) là index 0
    // Vậy các mục sau sẽ bắt đầu từ index 1
    private int offset = 1; 

    void Start()
    {
        // Lấy index hiện tại từ hệ thống
        int currentLevel = QualitySettings.GetQualityLevel();

        // Tự động tick đúng Toggle dựa trên index (đã cộng offset)
        if (currentLevel == 1) lowToggle.isOn = true;
        else if (currentLevel == 2) mediumToggle.isOn = true;
        else if (currentLevel == 3) highToggle.isOn = true;

        // Đăng ký sự kiện
        lowToggle.onValueChanged.AddListener(delegate { if(lowToggle.isOn) SetQuality(1); });
        mediumToggle.onValueChanged.AddListener(delegate { if(mediumToggle.isOn) SetQuality(2); });
        highToggle.onValueChanged.AddListener(delegate { if(highToggle.isOn) SetQuality(3); });
    }

    public void SetQuality(int index)
    {
        // Đổi mức đồ họa
        QualitySettings.SetQualityLevel(index, true);
        
        // In ra Console để cậu kiểm tra cho chắc
        Debug.Log("Đã đổi sang: " + QualitySettings.names[index] + " (Index: " + index + ")");
    }
}