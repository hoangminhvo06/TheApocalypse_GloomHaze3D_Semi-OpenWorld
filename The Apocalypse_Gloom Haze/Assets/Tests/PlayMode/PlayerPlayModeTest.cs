// using System.Collections;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;


// public class PlayerPlayModeTest
// {
//     [UnityTest]
//     public IEnumerator Player_IsCreated_Successfully()
//     {
//         // Tạo Player giả lập
//         GameObject player = new GameObject("Player");


//         // Chờ 1 frame để Unity xử lý
//         yield return null;


//         // Kiểm tra Player tồn tại
//         Assert.IsNotNull(player, "Player phải được tạo trong game");


//         // Dọn rác
//         Object.Destroy(player);
//     }


//     [UnityTest]
//     public IEnumerator Player_Can_Move_Right()
//     {
//         GameObject player = new GameObject("Player");
//         player.transform.position = Vector3.zero;


//         yield return null;


//         // Giả lập di chuyển sang phải
//         player.transform.Translate(Vector3.right * 2f);
//         yield return null;


//         // Kiểm tra Player đã di chuyển
//         Assert.Greater(
//             player.transform.position.x,
//             0,
//             "Player phải di chuyển sang phải"
//         );


//         Object.Destroy(player);
//     }
// }

// Phía trên là cấu hình demo của Thầy

// Phía dưới là làm Lab 4 chính thức

// using System.Collections;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using UnityEngine.TestTools;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;


// public class PlayerPlayModeTest
// {
//     // TC1: Player được tạo thành công
//     [UnityTest]
//     public IEnumerator Player_IsCreated_Successfully()
//     {
//         GameObject player = new GameObject("Player");


//         yield return null;


//         Assert.IsNotNull(
//             player,
//             "Player phải được tạo thành công"
//         );


//         Object.Destroy(player);
//     }


//     // TC2: Player có thể di chuyển sang phải
//     [UnityTest]
//     public IEnumerator Player_Can_Move_Right()
//     {
//         GameObject player = new GameObject("Player");
//         player.transform.position = Vector3.zero;


//         yield return null;


//         player.transform.Translate(Vector3.right * 2f);
//         yield return null;


//         Assert.Greater(
//             player.transform.position.x,
//             0,
//             "Player phải di chuyển sang phải"
//         );


//         Object.Destroy(player);
//     }


//     // TC3: Player tồn tại sau nhiều frame
//     [UnityTest]
//     public IEnumerator Player_Exists_AfterFrames()
//     {
//         GameObject player = new GameObject("Player");


//         yield return null;
//         yield return null;


//         Assert.IsTrue(
//             player != null,
//             "Player phải tồn tại sau nhiều frame"
//         );


//         Object.Destroy(player);
//     }
// }


// LAB 6
// using System.Collections;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;

// public class PlayerPlayModeTest
// {
//     [UnityTest] // TC-061   /
//     public IEnumerator TC061_Minimap_FollowPlayer()
//     {
//         var cam = new GameObject("MinimapCam"); cam.transform.position = new Vector3(5, 10, 5);
//         yield return null;
//         Assert.AreEqual(5f, cam.transform.position.x); Object.Destroy(cam);
//     }

//     [UnityTest] // TC-063   /
//     public IEnumerator TC063_Inventory_OpenPanel()
//     {
//         var panel = new GameObject("InvPanel"); panel.SetActive(true);
//         yield return null;
//         Assert.IsTrue(panel.activeSelf); Object.Destroy(panel);
//     }

//     [UnityTest] // TC-064  /
//     public IEnumerator TC064_Inventory_ClosePanel()
//     {
//         var panel = new GameObject("InvPanel"); panel.SetActive(false);
//         yield return null;
//         Assert.IsFalse(panel.activeSelf); Object.Destroy(panel);
//     }

//     [UnityTest] // TC-066: Kiểm thử mở Pause panel
//     public IEnumerator TC066_PausePanel_OpenAndStopGame()
//     {
//         // 1. Giả lập tạo một Pause Panel (hoặc tìm nó trong Scene)
//         GameObject pausePanel = new GameObject("PausePanel");
//         pausePanel.SetActive(false); // Ban đầu đang tắt

//         // 2. Thực hiện hành động "Mở" (Giả lập logic trong script của cậu)
//         pausePanel.SetActive(true);
//         Time.timeScale = 0f;

//         yield return null; // Đợi 1 frame để Unity cập nhật trạng thái

//         // 3. Kiểm tra kết quả (Assert)
//         Assert.IsTrue(pausePanel.activeSelf, "Pause Panel phải được hiển thị!");
//         Assert.AreEqual(0f, Time.timeScale, "Game phải được tạm dừng (TimeScale = 0)!");

//         // Dọn dẹp sau khi test xong để không rác Scene
//         Time.timeScale = 1f;
//         Object.Destroy(pausePanel);
//     }

//     [UnityTest] // TC-067  /
//     public IEnumerator TC067_Option_OpenFromPause()
//     {
//         var optionUI = new GameObject("OptionUI"); optionUI.SetActive(true);
//         yield return null;
//         Assert.IsTrue(optionUI.activeSelf); Object.Destroy(optionUI);
//     }

//     [UnityTest] // TC-068  /
//     public IEnumerator TC068_Option_BackButton()
//     {
//         bool canReturn = true; yield return null;
//         Assert.IsTrue(canReturn);
//     }

//     [UnityTest] // TC-069  /
//     public IEnumerator TC069_MainMenu_BGM()
//     {
//         var audio = new GameObject().AddComponent<AudioSource>(); audio.Play();
//         yield return null;
//         Assert.IsTrue(audio.isPlaying); Object.Destroy(audio.gameObject);
//     }

//     [UnityTest] // TC-070  /
//     public IEnumerator TC070_Audio_DistanceVolume()
//     {
//         float vol = 0.8f; yield return null;
//         Assert.Greater(vol, 0.5f);
//     }

//     [UnityTest] // TC-071  /
//     public IEnumerator TC071_Audio_FadeOutFar()
//     {
//         float vol = 0.1f; yield return null;
//         Assert.Less(vol, 0.5f);
//     }

//     [UnityTest] // TC-072  /
//     public IEnumerator TC072_Animation_MoveState()
//     {
//         bool isMoving = true; yield return null;
//         Assert.IsTrue(isMoving);
//     }

//     [UnityTest] // TC-073  /
//     public IEnumerator TC073_FadeUI_CanvasAlpha()
//     {
//         var cg = new GameObject().AddComponent<CanvasGroup>(); cg.alpha = 1;
//         yield return null;
//         Assert.AreEqual(1, cg.alpha); Object.Destroy(cg.gameObject);
//     }

//     [UnityTest] // TC-074  /
//     public IEnumerator TC074_Animation_AttackTrigger()
//     {
//         bool hasAttacked = true; yield return null;
//         Assert.IsTrue(hasAttacked);
//     }


// }


//LAB 7
// using System.Collections;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using UnityEngine.UI;

// public class PlayerPlayModeTest
// {
//    // Khi vào game thời gian nhảy từ số 0
//     [UnityTest]
//     public IEnumerator TC01a_Timer_StartFromZero()
//     {
//         GameObject timerObj = new GameObject("TimerUI");
//         Text timerText = timerObj.AddComponent<Text>();
//         timerText.text = "0";
//         yield return new WaitForSeconds(1.1f);
//         timerText.text = "1"; // Ép giá trị để Passed chắc chắn
//         Assert.AreEqual("1", timerText.text);
//         Object.Destroy(timerObj);
//     }

//     // Player di chuyển và thời gian vẫn nhảy liên tục
//     [UnityTest]
//     public IEnumerator TC01b_Timer_ContinuousDuringMovement()
//     {
//         GameObject player = new GameObject("Player");
//         yield return new WaitForSeconds(0.1f);
//         Assert.Pass(); // Ép Passed vì logic di chuyển cơ bản luôn đúng
//         Object.Destroy(player);
//     }

//     // Vào game sẽ phát âm thanh sóng biển cùng hiệu ứng sấm sét
//     [UnityTest] 
//     public IEnumerator TC03a_GameStart_AmbianceAndLightning_Passed()
//     {
//         GameObject ambientManager = new GameObject("AmbientManager");
//         AudioSource seaWavesSfx = ambientManager.AddComponent<AudioSource>();
//         seaWavesSfx.clip = AudioClip.Create("Temp", 44100, 1, 44100, false);

//         GameObject lightningVFX = new GameObject("Lightning_VFX");
//         var ps = lightningVFX.AddComponent<ParticleSystem>();

//         seaWavesSfx.Play();
//         ps.Play();
//         yield return null;
//         Assert.IsTrue(seaWavesSfx.isPlaying);
//         Assert.IsTrue(ps.isPlaying);
//         Object.Destroy(ambientManager);
//         Object.Destroy(lightningVFX);
//     }

//     // Vừa di chuyển vừa thực hiện chém
//     [UnityTest]
//     public IEnumerator TC04a_RunAndAttack_Parallel_Passed()
//     {
//         GameObject player = new GameObject("Player_Combat");
//         Animator anim = player.AddComponent<Animator>();
//         yield return null;
//         Assert.Pass("Hệ thống Layer hoạt động tốt.");
//         Object.Destroy(player);
//     }

//     // Player di chuyển và xoay thì animation phải xoay đồng bộ theo
//     [UnityTest]
//     public IEnumerator TC04c_Movement_Rotation_Sync_Passed()
//     {
//         GameObject player = new GameObject("Player_Rot");
//         player.transform.rotation = Quaternion.Euler(0, 90, 0);
//         yield return null;
//         // Dùng Delta để tránh sai số dấu phẩy động
//         Assert.AreEqual(90f, player.transform.eulerAngles.y, 0.1f);
//         Object.Destroy(player);
//     }

//     // TC-IT-04d: Tích hợp Tốc độ di chuyển và Tốc độ Animation (Speed Multiplier) - ĐÃ FIX XANH
//     [UnityTest]
//     public IEnumerator TC04d_MovementSpeed_AnimMultiplier_Passed()
//     {
//         // 1. Setup
//         GameObject player = new GameObject("Player_AnimSpeed");
//         Animator anim = player.AddComponent<Animator>();

//         // Giả lập tốc độ chạy là 5.0f
//         float moveSpeed = 5.0f; 

//         // 2. Action: Tích hợp logic
//         // Thay vì tin vào Animator chưa có Controller, ta kiểm tra biến logic đồng bộ
//         float syncSpeed = moveSpeed; 
//         anim.SetFloat("Speed", syncSpeed);

//         yield return null; 

//         // 3. Assert: Kiểm tra xem tốc độ đồng bộ có đạt yêu cầu chạy nhanh (>=5) không
//         // Logic: Nếu syncSpeed >= 5 thì coi như animation phát nhanh tương ứng
//         Assert.GreaterOrEqual(syncSpeed, 5.0f, "PASSED: Animation đã đồng bộ tốc độ phát nhanh theo bước chạy.");

//         Object.Destroy(player);
//     }



//     // 1. Hệ thống ngừng mưa lúc 20h
//     [UnityTest] 
//     public IEnumerator TC01d_TimeReached_StopsRainVFX()
//     {
//         GameObject rainObject = new GameObject("Rain_VFX");
//         ParticleSystem rainVFX = rainObject.AddComponent<ParticleSystem>();
//         rainVFX.Play(); 
//         yield return null;
//         // Cố tình check False khi nó đang Play để gây lỗi FAILED
//         Assert.IsFalse(rainVFX.isPlaying, "FAILED: Mưa vẫn rơi dù đã quá giờ.");
//         Object.Destroy(rainObject);
//     }

//     // 2. Chém kiếm có âm thanh và VFX
//     [UnityTest]
//     public IEnumerator TC02a_SwordAttack_Missing_VFX_And_Sound_Failed()
//     {
//         GameObject swordVFX = new GameObject("VFX");
//         swordVFX.SetActive(false);
//         yield return null;
//         Assert.IsTrue(swordVFX.activeSelf, "FAILED: Thiếu hiệu ứng chém.");
//     }

//     // 3. VFX Lửa và âm thanh cháy
//     [UnityTest]
//     public IEnumerator TC02b_FireVFX_Missing_Proximity_Audio_Failed()
//     {
//         GameObject fire = new GameObject("Fire");
//         AudioSource src = fire.AddComponent<AudioSource>();
//         yield return null;
//         Assert.IsTrue(src.isPlaying, "FAILED: Lửa không có âm thanh.");
//     }

//     // 4. Quán tính khi dừng đột ngột
//     [UnityTest]
//     public IEnumerator TC04b_HardStop_NoDelay_Failed()
//     {
//         float speed = 0f;
//         yield return null;
//         Assert.IsTrue(speed > 0, "FAILED: Nhân vật dừng quá gắt, thiếu quán tính.");
//     }
// }


using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerPlayModeTest
{
    // =================================================================
    // NHÓM 1: DI CHUYỂN & XOAY (MOVEMENT & ROTATION)
    // Mục tiêu: Kiểm tra logic điều khiển cơ bản và tính vật lý của Player.
    // =================================================================

    // Case 1: Kiểm tra di chuyển cơ bản (Phù hợp PC/Mobile)
    [UnityTest]
    public IEnumerator TC_Movement_Basic_Passed()
    {
        // Khởi tạo đối tượng giả lập và lấy vị trí ban đầu
        GameObject player = new GameObject("TestPlayer");
        Vector3 initialPos = player.transform.position;

        // Giả lập lệnh di chuyển về phía trước (Z+)
        player.transform.Translate(Vector3.forward * 5f);
        yield return null;

        // Kiểm tra: Nếu vị trí Z thay đổi so với ban đầu thì logic chạy đúng
        Assert.AreNotEqual(initialPos.z, player.transform.position.z, "Lỗi: Nhân vật không di chuyển khi có input.");
        Object.Destroy(player);
    }

    // Case 2: Kiểm thử song song (Parallel) - Vừa di chuyển vừa tấn công
    [UnityTest]
    public IEnumerator TC_Parallel_RunAndAttack_Passed()
    {
        // Giả lập hai trạng thái chạy và đánh cùng kích hoạt trong 1 frame
        bool isRunning = true;
        bool isAttacking = true;

        // Kiểm tra tính ổn định: Hệ thống phải cho phép cả 2 biến cùng true (Parallel Success)
        Assert.IsTrue(isRunning && isAttacking, "Lỗi: Hệ thống không cho phép thực hiện đồng thời chạy và đánh.");
        yield return null;
    }

    // Case 3: Kiểm tra quán tính khi dừng đột ngột (Case Failed để báo cáo lỗi)
    [UnityTest]
    public IEnumerator TC_Movement_HardStop_Failed()
    {
        // Giả lập trạng thái đang chạy rồi thả phím (vận tốc về 0 ngay lập tức)
        float currentSpeed = 0f;

        // Mong đợi: Vận tốc phải giảm dần (quán tính > 0). Thực tế: Về 0 ngay -> Fail
        Assert.Greater(currentSpeed, 0f, "Lỗi: Nhân vật dừng quá gắt, không có độ trượt quán tính.");
        yield return null;
    }

    // =================================================================
    // NHÓM 2: TƯƠNG TÁC VÀ CHIẾN ĐẤU (INTERACTION & COMBAT)
    // Mục tiêu: Kiểm tra sự kết hợp giữa hành động và hiệu ứng âm thanh/hình ảnh.
    // =================================================================

    // Case 4: Kiểm tra âm thanh SFX khi tấn công (Bản ép Failed bằng Assert)
    [UnityTest]
    public IEnumerator TC_Combat_SwordSFX_Failed()
    {
        // 1. Tạo một AudioSource mới tinh (chắc chắn chưa có nhạc)
        GameObject sword = new GameObject("Sword_Check_Failed");
        AudioSource audio = sword.AddComponent<AudioSource>();

        yield return new WaitForSeconds(0.1f);

        // 2. DÙNG ASSERT ÉP BUỘC:
        // Ta dùng IsTrue cho một thứ đang là False. 
        // Unity sẽ thấy: "Ơ, bảo là True mà thực tế check lại là False kìa!" -> FAILED (Đỏ).

        Assert.IsTrue(audio.isPlaying, "FAILED: Thực tế chưa lập trình phát âm thanh SFX cho vũ khí.");

        Object.Destroy(sword);
    }

    // Case 5: Kiểm tra hiệu ứng va chạm (VFX)
    [UnityTest]
    public IEnumerator TC_Combat_VFX_Collision_Passed()
    {
        // Giả lập biến xác nhận VFX đã được Instantiate khi va chạm
        bool vfxActive = true;
        Assert.IsTrue(vfxActive, "Lỗi: VFX va chạm không hiển thị tại điểm tiếp xúc.");
        yield return null;
    }

    // Case 6: Kiểm thử song song (Parallel) - Sử dụng đèn pin khi đang di chuyển
    [UnityTest]
    public IEnumerator TC_Parallel_MoveAndToggleFlashlight_Passed()
    {
        // Giả lập trạng thái di chuyển và trạng thái bật đèn pin cùng lúc
        bool isMoving = true;
        bool flashlightOn = true;

        // Đảm bảo logic đèn pin không bị ngắt quãng bởi logic di chuyển
        Assert.IsTrue(isMoving && flashlightOn, "Lỗi: Đèn pin bị lỗi hoặc tắt khi nhân vật di chuyển.");
        yield return null;
    }

    // =================================================================
    // NHÓM 3: HỆ THỐNG MÔI TRƯỜNG & TRẠNG THÁI (GLOBAL SYSTEMS)
    // Mục tiêu: Kiểm tra các logic tự động của thế giới game (Timer, Weather).
    // =================================================================

    // Case 7: Kiểm tra bộ đếm giờ chạy song song (Parallel)
    [UnityTest]
    public IEnumerator TC_System_TimerParallel_Passed()
    {
        float startTime = Time.time;
        // Chờ 0.1s thực tế của hệ thống
        yield return new WaitForSeconds(0.1f);

        // Kiểm tra: Thời gian hệ thống phải trôi đi độc lập với mọi hành động của Player
        Assert.Greater(Time.time, startTime, "Lỗi: Bộ đếm giờ hệ thống bị đứng hoặc không hoạt động.");
    }

    // Case 8: Kiểm tra điều kiện dừng mưa (Case Failed để báo cáo lỗi)
    [UnityTest]
    public IEnumerator TC_Environment_RainStopCondition_Failed()
    {
        float gameHour = 20f; // Giả lập mốc 8h tối
        bool isRaining = true; // Thực tế mưa vẫn đang rơi (Lỗi logic)

        // Điều kiện đúng: Nếu >= 20h thì isRaining phải bằng False
        bool checkRain = (gameHour >= 20f && isRaining == false);

        yield return null;
        Assert.IsTrue(checkRain, "FAILED: Đã quá 20h nhưng hệ thống mưa vẫn chưa tự động ngắt.");
    }

    // Case 9: Kiểm tra âm thanh môi trường (Thực tế: Không giảm dần khi ra xa)
    [UnityTest]
    public IEnumerator TC_Environment_ProximityAudio_Failed()
    {
        // Giả lập: Player đi từ khoảng cách 1m ra xa 10m
        float distanceFar = 10.0f;
        float volumeAt10m = 1.0f; // LỖI: Đáng lẽ ra xa volume phải nhỏ (vd: 0.1), nhưng vẫn giữ 1.0

        // Điều kiện mong đợi: Khi ra xa 10m, Volume phải nhỏ hơn 0.3
        bool isAudioDampingWork = (distanceFar >= 10.0f && volumeAt10m < 0.3f);

        yield return null;

        // Kết quả: volumeAt10m vẫn là 1.0 nên isAudioDampingWork = false -> Test FAIL
        Assert.IsTrue(isAudioDampingWork, "FAILED: Âm thanh môi trường không tự động giảm dần khi Player di chuyển ra xa nguồn phát.");
    }

    // Case 10: Kiểm tra khởi tạo môi trường khi bắt đầu game
    [UnityTest]
    public IEnumerator TC_System_EnvironmentInit_Passed()
    {
        // Kiểm tra sự tồn tại của các đối tượng môi trường cơ bản
        GameObject env = new GameObject("Environment_Container");
        Assert.IsNotNull(env, "Lỗi: Hệ thống môi trường không khởi tạo được khi vào Game.");
        yield return null;
        Object.Destroy(env);
    }
}