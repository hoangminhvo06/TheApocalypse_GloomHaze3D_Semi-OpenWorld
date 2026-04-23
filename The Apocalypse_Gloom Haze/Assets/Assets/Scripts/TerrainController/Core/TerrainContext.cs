using UnityEngine;

public class TerrainContext
{
    public Terrain terrain;
    public TerrainData terrainData;

    // Map dữ liệu
    public float[,] heightMap;
    public int[,] biomeMap;
    public float[,] dangerMap;

    // Kích thước
    public int width;
    public int height;

    // Seed dùng chung
    public int seed;

    public TerrainContext(Terrain terrain, TerrainConfig config)
    {
        this.terrain = terrain;
        this.terrainData = terrain.terrainData;

        width = config.heightMapResolution;
        height = config.heightMapResolution;

        heightMap = new float[width, height];
        biomeMap = new int[width, height];
        dangerMap = new float[width, height];
    }
}
