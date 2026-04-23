using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Accounts : MonoBehaviour
{
    private string baseUrl = "http://localhost:5033/api/Account";
    public TMP_InputField emailInput;
    public void GetAllAccounts()
    {
        StartCoroutine(GetAll());
    }

    public void GetAccountByEmail()
    {
        StartCoroutine(GetByEmail());
    }

    // Phương thức GetAll()
    IEnumerator GetAll()
    {
        var url = "http://localhost:5033/api/get-all-accounts";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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
            ResponseAccountList response = JsonUtility
                        .FromJson<ResponseAccountList> (request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.notification);

            if (response.issuccess) 
            {
                foreach (var acc in response.data)
                    {
                        Debug.Log("Username: " + acc.userName + "; Email:" + acc.email);
                    }
            }
            else
            {
                Debug.Log("Lỗi lấy thông tin ds tài khoản");
            }
        }
    }
    }

    // Phương thức GetByEmail
    IEnumerator GetByEmail()
    {
        var email = emailInput.text;
        var url = $"http://localhost:5033/api/get-accounts-by-email/{email}";

        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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

            Response response = JsonUtility
                        .FromJson<Response>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.notification); // => Lấy tất cả accounts thành công!

            if (response.issuccess) 
            {
                Debug.Log("Email:" +response.data.email);
                Debug.Log("Username:" + response.data.userName);
            }
            else
            {
                Debug.Log("Lỗi lấy thông tin ds tài khoản");
            }
        }
    }
    }
}
