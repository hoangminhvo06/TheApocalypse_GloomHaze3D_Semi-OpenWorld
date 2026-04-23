using UnityEngine;

public class FalloffMapGenerator
{
    public void Apply(
        TerrainContext context,
        float falloffStrength = 0.4f,
        float falloffPower = 3f
    )
    {
        int width = context.width;
        int height = context.height;

        float centerX = width / 2f;
        float centerY = height / 2f;
        float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float dx = x - centerX;
                float dy = y - centerY;

                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                float normalizedDistance = distance / maxDistance;

                float falloff = Mathf.Pow(normalizedDistance, falloffPower);

                context.heightMap[x, y] -= falloff * falloffStrength;
            }
        }
    }
}
