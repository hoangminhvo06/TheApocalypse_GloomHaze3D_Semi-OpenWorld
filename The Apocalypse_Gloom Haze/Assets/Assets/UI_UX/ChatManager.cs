using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject chatPanel;
    public TMP_InputField inputField;


    public GameObject messagePrefab; // Tạo 1 cái Text TMP làm mẫu rồi kéo vào đây
    public Transform messageParent;  // Kéo cái Message_List vào đây

    void Start()
    {
        if (chatPanel != null) chatPanel.SetActive(false);
        // Xóa trắng bảng chat khi bắt đầu game

    }

    public void ToggleChat()
    {
        bool isActive = !chatPanel.activeSelf;
        chatPanel.SetActive(isActive);

        if (isActive)
        {
            inputField.ActivateInputField();
        }
    }

    void Update()
    {
        // 1. Nhấn Enter hoặc Space để gửi khi đang nhập liệu
        if (inputField.isFocused && !string.IsNullOrEmpty(inputField.text))
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                SendMessage();
            }
        }

        // 2. Nhấn Esc để đóng chat nhanh
        if (Input.GetKeyDown(KeyCode.Escape) && chatPanel.activeSelf)
        {
            ToggleChat();
        }
    }

    public void SendMessage()
    {
        // 1. Kiểm tra đầu vào
        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("<color=yellow>ChatSystem:</color> Người chơi nhấn gửi nhưng ô nhập trống.");
            return;
        }

        // 2. Kiểm tra các ô tham chiếu trong Inspector
        if (messagePrefab == null || messageParent == null)
        {
            Debug.LogError("<color=red>ChatSystem LỖI:</color> Cậu quên chưa kéo Message Prefab hoặc Message Parent vào Chat_System rồi!");
            return;
        }

        try
        {
            // 3. Bắt đầu quá trình tạo tin nhắn
            Debug.Log($"<color=cyan>ChatSystem:</color> Đang khởi tạo tin nhắn: {inputField.text}");

            GameObject newMsg = Instantiate(messagePrefab, messageParent);

            // Kiểm tra xem Object có được tạo ra thành công không
            if (newMsg == null)
            {
                Debug.LogError("ChatSystem: Không thể Instantiate được Prefab!");
                return;
            }

            TextMeshProUGUI t = newMsg.GetComponent<TextMeshProUGUI>();

            if (t != null)
            {
                t.text = "<color=#00FF00>You:</color> " + inputField.text;
                Debug.Log("<color=green>ChatSystem: Gửi tin nhắn thành công!</color>");
            }
            else
            {
                Debug.LogError("ChatSystem LỖI: Cái Prefab Chat_Display của cậu thiếu component TextMeshProUGUI rồi!");
            }

            // 4. Dọn dẹp ô nhập
            inputField.text = "";
            inputField.ActivateInputField();

            // 5. Giới hạn số lượng
            if (messageParent.childCount > 20)
            {
                Destroy(messageParent.GetChild(0).gameObject);
                Debug.Log("ChatSystem: Đã xóa tin nhắn cũ nhất để tối ưu bộ nhớ.");
            }
        }
        catch (System.Exception e)
        {
            // Nếu có bất kỳ lỗi "ngầm" nào, nó sẽ hiện lên đây
            Debug.LogError("ChatSystem CRASH: " + e.Message);
        }
    }
}