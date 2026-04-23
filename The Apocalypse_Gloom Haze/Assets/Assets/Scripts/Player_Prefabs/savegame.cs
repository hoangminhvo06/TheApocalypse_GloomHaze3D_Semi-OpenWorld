using System;
using UnityEngine;

public class savegame : MonoBehaviour
{
    public GameObject player;

    // Mã hóa float thành Base64
    private string EncodeFloat(float value) // Encode: Mã hóa
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    // Giải mã Base64 thành float
    private float DecodeFloat(string base64Value, float defaultValue = 0.0f) // Decode: Giải mã code
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(base64Value);
            return BitConverter.ToSingle(bytes, 0);
        }
        catch (Exception)
        {
            return defaultValue; // Trả về giá trị mặc định nếu không thể giải mã
        }
    }

    void Start()
    {
        // Đọc tọa độ từ PlayerPrefabs và giải mã Base64
        float x = DecodeFloat(PlayerPrefs.GetString("x", EncodeFloat(0.0f)), 0.0f);
        float y = DecodeFloat(PlayerPrefs.GetString("y", EncodeFloat(0.0f)), 0.0f);
        float z = DecodeFloat(PlayerPrefs.GetString("z", EncodeFloat(0.0f)), 0.0f);

        // Đặt vị trí cho player
        Vector3 vector = new Vector3(x, y, z);
        player.transform.position = vector;
        Debug.Log("Đã load:" + vector);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 vector = player.transform.position;
            PlayerPrefs.SetString("x", EncodeFloat(vector.x));
            PlayerPrefs.SetString("y", EncodeFloat(vector.y));
            PlayerPrefs.SetString("z", EncodeFloat(vector.z));


            PlayerPrefs.Save();
            Debug.Log("Đã lưu: " + vector);

        }
    }

}
