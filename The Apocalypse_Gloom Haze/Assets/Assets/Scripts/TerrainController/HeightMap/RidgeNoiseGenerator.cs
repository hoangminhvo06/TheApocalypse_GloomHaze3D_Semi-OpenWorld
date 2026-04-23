// using UnityEngine;

// public class RidgeNoiseGenerator
// {
//     public void Apply(TerrainContext context, float strength = 0.5f)
//     {
//         int width = context.width;
//         int height = context.height;

//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 float ridge = NoiseUtils.RidgedNoise(
//                     x,
//                     y,
//                     3,
//                     40f,
//                     context.seed
//                 );

//                 context.heightMap[x, y] -= ridge * strength;
//             }
//         }
//     }
// }
