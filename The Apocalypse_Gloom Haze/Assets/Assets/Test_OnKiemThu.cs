// using UnityEngine;
// using System.Collections;
// using NUnit.Framework;
// using UnityEngine.TestTools;
// public class Test_OnKiemThu
// {
//     [UnityTest]
//     public IEnumerator Test_Player_Movement_Logic()
//     {
//         //Arrange
//         GameObject player = new GameObject("TestPlayer");

//         player.transform.position = Vector3.zero;

//         //Act
//         player.transform.position = new Vector3(5, 0, 0);
//         yield return null;

//         //Assert
//         Assert.AreEqual(5f, player.transform.position.x, "Lỗi: Player không di chuyển đến đúng vị trí");

//         //Clean up
//         Object.Destroy(player);
//     }
// }
