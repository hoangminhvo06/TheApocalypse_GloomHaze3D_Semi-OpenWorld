using UnityEngine;

[CreateAssetMenu(
    fileName = "TerrainConfig",
    menuName = "Terrain Controller/Terrain Config"
)]
public class TerrainConfig : ScriptableObject
{
    [Header("Terrain Size")]
    public int heightMapResolution = 513;   // nên là 2^n + 1
    public int terrainWidth = 500;
    public int terrainLength = 500;
    public int terrainHeight = 80;

    [Header("Noise Settings")]
    public float baseNoiseScale = 50f;
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2f;

    [Header("Seed")]
    public int seed = 0;
    public bool useRandomSeed = true;

    [Header("Biome Ratio")]
    [Range(0f, 1f)] public float deadCityRatio = 0.25f;
    [Range(0f, 1f)] public float burnedForestRatio = 0.25f;
    [Range(0f, 1f)] public float toxicSwampRatio = 0.25f;
    [Range(0f, 1f)] public float craterZoneRatio = 0.25f;
}
