// using UnityEngine;

// public class BaseHeightGenerator
// {
//     public void Generate(TerrainContext context, TerrainConfig config)
//     {
//         int width = context.width;
//         int height = context.height;

//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 float noise = NoiseUtils.FBM(
//                     x,
//                     y,
//                     config.octaves,
//                     config.persistence,
//                     config.lacunarity,
//                     config.baseNoiseScale,
//                     context.seed
//                 );

//                 context.heightMap[x, y] = noise;
//             }
//         }
//     }
// }
