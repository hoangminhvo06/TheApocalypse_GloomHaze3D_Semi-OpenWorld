// using UnityEngine;
// using Fusion;
// using System.Collections.Generic;

// public class PlayerRunner : SimulationBehaviour, IPlayerJoined
// {
//     [SerializeField] 
//     GameObject playerPrefabs;

//     [SerializeField] 
//     GameObject enemyPrefab;

//     [SerializeField] 
//     Transform[] enemySpawnPoints;

//     private bool enemySpawnRequested = false;

//     // Track player nào đã được spawn rồi, tránh spawn lại
//     private HashSet<PlayerRef> spawnedPlayers = new HashSet<PlayerRef>();

//     public void PlayerJoined(PlayerRef player)
//     {
//         // Nếu player này đã spawn rồi thì bỏ qua
//         // (PlayerJoined fire lại khi có người mới join)
//         if (spawnedPlayers.Contains(player)) return;
//         spawnedPlayers.Add(player);

//         // Chỉ spawn player của chính mình
//         if (player == Runner.LocalPlayer)
//         {
//             Runner.Spawn(
//                 playerPrefabs,
//                 new Vector3(0, 1, 0),
//                 Quaternion.identity,
//                 player
//             );
//         }

//         // Master Client spawn enemy 1 lần duy nhất
//         if (Runner.IsSharedModeMasterClient && !enemySpawnRequested)
//         {
//             enemySpawnRequested = true;

//             for (int i = 0; i < enemySpawnPoints.Length; i++)
//             {
//                 Runner.Spawn(
//                     enemyPrefab,
//                     enemySpawnPoints[i].position,
//                     Quaternion.identity
//                 );
//             }
//         }
//     }
// }


// Code runner mới Spawn Player random trong map cho lab 3
// using UnityEngine;
// using Fusion;
// using System.Collections.Generic;

// public class PlayerRunner : SimulationBehaviour, IPlayerJoined
// {
//     [SerializeField]
//     GameObject playerPrefabs;

//     [SerializeField]
//     GameObject enemyPrefab;

//     [SerializeField]
//     Transform[] enemySpawnPoints;

//     [SerializeField]
//     Transform[] playerSpawnPoints;

//     [SerializeField]
//     GameObject itemPrefab;

//     [SerializeField]
//     Transform[] itemSpawnPoints;

//     private bool enemySpawnRequested = false;
//     private HashSet<PlayerRef> spawnedPlayers = new HashSet<PlayerRef>();

//     private void Awake()
//     {
//         spawnedPlayers.Clear();
//         enemySpawnRequested = false;
//     }

//     public void PlayerJoined(PlayerRef player)
//     {
//         if (spawnedPlayers.Contains(player)) return;
//         spawnedPlayers.Add(player);

//         if (player == Runner.LocalPlayer)
//         {
//             Vector3 spawnPos = GetRandomSpawnPoint();

//             Runner.Spawn(
//                 playerPrefabs,
//                 spawnPos,
//                 Quaternion.identity,
//                 player
//             );
//         }

//         if (Runner.IsSharedModeMasterClient && !enemySpawnRequested)
//         {
//             enemySpawnRequested = true;

//             // Spawn enemy
//             for (int i = 0; i < enemySpawnPoints.Length; i++)
//             {
//                 Runner.Spawn(
//                     enemyPrefab,
//                     enemySpawnPoints[i].position,
//                     Quaternion.identity
//                 );
//             }

//             // Spawn item — chỉ Master Client spawn 1 lần
//             // Tất cả client đều thấy vì có NetworkObject
//             if (itemPrefab != null && itemSpawnPoints != null)
//             {
//                 for (int i = 0; i < itemSpawnPoints.Length; i++)
//                 {
//                     Runner.Spawn(
//                         itemPrefab,
//                         itemSpawnPoints[i].position,
//                         Quaternion.identity
//                     );
//                 }
//             }
//         }
//     }

//     private Vector3 GetRandomSpawnPoint()
//     {
//         if (playerSpawnPoints == null || playerSpawnPoints.Length == 0)
//             return new Vector3(0, 1, 0);

//         int randomIndex = Random.Range(0, playerSpawnPoints.Length);
//         Debug.Log($"Spawn tại: {playerSpawnPoints[randomIndex].name} | vị trí: {playerSpawnPoints[randomIndex].position}");
//         return playerSpawnPoints[randomIndex].position;
//     }
// }
