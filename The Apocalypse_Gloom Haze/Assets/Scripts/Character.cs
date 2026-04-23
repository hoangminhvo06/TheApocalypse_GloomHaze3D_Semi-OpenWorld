using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Character : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField emailInput;
    

    public void GetCharacterById()
    {
        StartCoroutine(GetCharacById());
    }


    public void GetCharacterByEmail()
    {
        StartCoroutine(GetCharacByEmail());
    }

 
    public void GetAllCharacters()
    {
        StartCoroutine(GetAll());
    }


    // Phương thức GetCharacterById
    IEnumerator GetCharacById()
    {
        var id = idInput.text;
        var url = $"http://localhost:5033/api/get-character-by-id/{id}";
        
        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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
                ResponseCharacter response = JsonUtility
                            .FromJson<ResponseCharacter>(request.downloadHandler.text);

                Debug.Log(request.downloadHandler.text);
                Debug.Log(response.notification);

                if (response.issuccess) 
                {
                    Debug.Log("Character ID: " + response.data.id);
                    Debug.Log("Character Name: " + response.data.characterName);
                    Debug.Log("Email: " + response.data.email);
                    Debug.Log("Level: " + response.data.level);
                    Debug.Log("Experience: " + response.data.experience);
                }
                else
                {
                    Debug.Log("Lỗi lấy thông tin character");
                }
            }
        }
    }

    // Phương thức GetCharacterByEmail
    IEnumerator GetCharacByEmail()
    {
        var email = emailInput.text;
        var url = $"http://localhost:5033/api/get-character-by-email/{email}";

        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
        
            yield return request.SendWebRequest();
            Debug.Log(">>>>>>>>>>>>>>>>" + request.result);

            // Kiểm tra dữ liệu trả về
            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                ResponseCharacter response = JsonUtility
                            .FromJson<ResponseCharacter>(request.downloadHandler.text);

                Debug.Log(request.downloadHandler.text);
                Debug.Log(response.notification);

                if (response.issuccess) 
                {
                    Debug.Log("Character ID: " + response.data.id);
                    Debug.Log("Character Name: " + response.data.characterName);
                    Debug.Log("Email: " + response.data.email);
                    Debug.Log("Level: " + response.data.level);
                    Debug.Log("Experience: " + response.data.experience);
                }
                else
                {
                    Debug.Log("Lỗi lấy thông tin character");
                }
            }
        }
    }

    // Phương thức GetAllCharacters - ĐÃ FIX
    IEnumerator GetAll()
    {
        var url = "http://localhost:5033/api/get-all-characters";
        Debug.Log("Calling URL: " + url);
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
        
            yield return request.SendWebRequest();

            // Kiểm tra dữ liệu trả về
            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Lỗi kết nối: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Raw JSON: " + jsonResponse);
                
                string wrappedJson = "{\"issuccess\":true,\"notification\":\"Success\",\"data\":" + jsonResponse + "}";
                
                ResponseCharacterList response = JsonUtility.FromJson<ResponseCharacterList>(wrappedJson);

                Debug.Log("Notification: " + response.notification);
                Debug.Log("IsSuccess: " + response.issuccess);

                if (response.issuccess && response.data != null && response.data.Count > 0) 
                {
                    Debug.Log("=== DANH SÁCH CHARACTERS (Tổng: " + response.data.Count + ") ===");
                    foreach (var character in response.data)
                    {
                        Debug.Log("Character ID: " + character.id + 
                                  "; Name: " + character.characterName + 
                                  "; Level: " + character.level +
                                  "; Experience: " + character.experience);
                    }
                }
                else
                {
                    Debug.LogWarning("Không có characters trong data");
                }
            }
        }
    }
}