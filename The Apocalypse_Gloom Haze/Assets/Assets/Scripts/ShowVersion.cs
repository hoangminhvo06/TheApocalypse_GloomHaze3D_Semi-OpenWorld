using UnityEngine;
using TMPro; // Nếu dùng TextMeshPro

public class ShowVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private bool isBeta = true; // bật tắt chữ Beta

    void Start()
    {
        // Lấy version từ Project Settings -> Player
        versionText.text = "v." + Application.version;
        if (isBeta)
        {
            versionText.text += "Beta";
        }
    }
}
