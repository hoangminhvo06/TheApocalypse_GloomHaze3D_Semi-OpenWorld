using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    // Khai báo biến lưu giá trị 3 trường thông tin đk tài khoản
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;

     public void GotoLogin()
    {
        // Chuyển scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }
    public void OnRegisterClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;
        var name = nameInput.text;

        var account = new Account
        {
            email = email,
            password = password,
            userName = name
        };
        // chuyển đối tượng thành dạng chuỗi
        var json = JsonUtility.ToJson(account);
        // Debug.Log(json);
        StartCoroutine(Post(json));
    }

    IEnumerator Post(string json)
    {
        var url = "http://localhost:5033/api/register";

        var request = new UnityWebRequest(url, "POST");
        
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();

        // Kiểm tra dl trả về
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
                
                // Chuyển trang Login 
                UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
            }
            else
            {
                Debug.Log("Lỗi đăng ký tài khoản");
            }
        }
    }
}