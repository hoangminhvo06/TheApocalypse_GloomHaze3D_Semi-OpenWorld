using UnityEngine;
using TMPro;
using System;

public class RealTimeClock : MonoBehaviour // khai báo 1 class tên RealTimeClock
{
    public TMP_Text clockText; // 1 biến public cho phép kéo UI Text vào
    private float timer = 0f; // Tạo 1 biến kiểu float tên Timer với giá trị ban đầu bằng 0

    void Update() // Hàm update mỗi frame ( Unity tự gọi liên tục )
    {
        timer += Time.deltaTime; // Thời gian(giây) giữa 2 frame
                                 // Mỗi frame sẽ cộng thêm vào timer.
                                 // Như vậy timer tăng liên tục theo thời gian thực, bất kể FPS bao nhiêu

        if (timer >= 1f) // vẫn đếm từng giây, câu lệnh nếu timer >= 1 giây --> 1 giây đã trôi qua
        {
            DateTime now = DateTime.Now; // Lấy thời gian thực của System PC or Mobile phone of players.
            // Chỉ hiển thị giờ:phút
            clockText.text = now.ToString("HH:mm"); // Biến now được chuyển thành chuỗi text.
                                                    // "HH:mm" là định dạng:
                                                    // HH = giờ (24h, có số 0 phía trước nếu <10).
                                                    // mm = phút (cũng có số 0 phía trước nếu <10).
            timer = 0f; // Reset timer về 0 và bắt đầu đếm lại giây tiếp theo.
        }
    }
}
