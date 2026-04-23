// using NUnit.Framework;
// using UnityEngine;


// public class PlayerEditModeTest
// {
//     // Test case 1: Tốc độ di chuyển phải > 0
//     [Test]
//     public void Player_MoveSpeed_GreaterThanZero()
//     {
//         float moveSpeed = -5f;


//         Assert.Greater(
//             moveSpeed,
//             0,
//             "Tốc độ di chuyển phải lớn hơn 0"
//         );
//     }


//     // Test case 2: Tốc độ di chuyển không được âm
//     [Test]
//     public void Player_MoveSpeed_NotNegative()
//     {
//         float moveSpeed = 0f;


//         Assert.GreaterOrEqual(
//             moveSpeed,
//             0,
//             "Tốc độ di chuyển không được là số âm"
//         );
//     }
// }



// Phía trên là cấu hình demo của Thầy

// Phía dưới là làm Lab 4 chính thức

// using NUnit.Framework;
// using UnityEngine;


// public class PlayerEditModeTest
// {

    // Lab 4
    // ========================
    // LOGIC CƠ BẢN
    // ========================


    // TC1: Điểm khởi đầu phải >= 0
    // [Test]
    // public void Score_InitialValue_NotNegative()
    // {
    //     int score = 0;


    //     Assert.GreaterOrEqual(
    //         score,
    //         0,
    //         "Score ban đầu không được âm"
    //     );
    // }


    // // TC2: Cộng điểm hợp lệ
    // [Test]
    // public void Score_AddPoint_Correct()
    // {
    //     int score = 10;
    //     score += 5;


    //     Assert.AreEqual(
    //         15,
    //         score,
    //         "Score phải được cộng đúng"
    //     );
    // }


    // // TC3: Không cho phép cộng điểm âm
    // [Test]
    // public void Score_AddNegative_NotAllowed()
    // {
    //     int score = 10;
    //     int addPoint = -5;


    //     Assert.GreaterOrEqual(
    //         addPoint,
    //         0,
    //         "Không cho phép cộng điểm âm"
    //     );
    // }


    // // ========================
    // // LOGIC NÂNG CAO
    // // ========================


    // // TC4: Tốc độ di chuyển > 0
    // [Test]
    // public void Player_MoveSpeed_GreaterThanZero()
    // {
    //     float moveSpeed = 5f;


    //     Assert.Greater(
    //         moveSpeed,
    //         0,
    //         "Tốc độ di chuyển phải lớn hơn 0"
    //     );
    // }


    // // TC5: Máu nhân vật không âm
    // [Test]
    // public void Player_Health_NotNegative()
    // {
    //     int health = 100;


    //     Assert.GreaterOrEqual(
    //         health,
    //         0,
    //         "Máu nhân vật không được âm"
    //     );
    // }


    // // TC6: Sát thương không được âm
    // [Test]
    // public void Damage_NotNegative()
    // {
    //     int damage = 10;


    //     Assert.GreaterOrEqual(
    //         damage,
    //         0,
    //         "Sát thương không được là số âm"
    //     );
    // }
//}









// LAB 6

// using NUnit.Framework;
// using UnityEngine;

// public class PlayerEditModeTest
// {
//     [Test] // TC-060  /
//     public void TC060_HealthBar_Logic() 
//     {
//         int health = 100; health -= 20;
//         Assert.AreEqual(80, health, "Máu phải giảm đúng sau khi trừ");
//     }

//     [Test] // TC-062  /
//     public void TC062_TimeSystem_Logic() 
//     {
//         float currentHour = 20f;
//         Assert.IsTrue(currentHour >= 18f, "Logic ban đêm phải kích hoạt sau 18h");
//     }

//     [Test] // TC-065  /
//     public void TC065_Inventory_ItemSelection() 
//     {
//         string itemName = "Pistol"; int dmg = 15;
//         Assert.IsTrue(!string.IsNullOrEmpty(itemName) && dmg > 0, "Dữ liệu Item phải hợp lệ");
//     }

//     [Test] // TC-075  /
//     public void TC075_Particle_RainConfig() 
//     {
//         float emissionRate = 50f;
//         Assert.Greater(emissionRate, 0, "Tốc độ rơi mưa phải lớn hơn 0");
//     }

//     [Test] // TC-076  /
//     public void TC076_GameplayEffect_NVState() 
//     {
//         bool isNVActive = true;
//         Assert.IsTrue(isNVActive, "Trạng thái Night Vision phải được bật");
//     }

//     [Test] // TC-077  /
//     public void TC077_NVEffect_ColorSwap() 
//     {
//         Color nvColor = Color.red;
//         Assert.AreEqual(Color.red, nvColor, "Màu Night Vision phải chuyển sang đỏ");
//     }
// }


