using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Login : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public void GoToRegister()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Register");
    }

    public void OnLoginClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;

        var account = new Account
        {
            email = email,
            password = password,
        };
        // Chuyển đối tượng thành dạng chuỗi
        var json = JsonUtility.ToJson(account);
        StartCoroutine(Post(json)); //  <<< sửa chỗ này
    }

    IEnumerator Post(string json)
    {
        var url = "http://localhost:5033/api/login";

        var request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Kiểm tra dữ liệu trả về
        if (request.result == UnityWebRequest.Result.ConnectionError || 
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Chuyển từ json sang object
            var response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.notification);

            if (response.issuccess)
            {
                // Debug.Log(response.data.email); 
                // Debug.Log(response.data.userName);
                // Debug.Log(response.data.createAt);
                Debug.Log("Đăng nhập thành công!");
                // Chuyển trang Login
                UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
            }
            else
            {
                Debug.Log("Đăng nhập thất bại");
            }
        }
    }
}
