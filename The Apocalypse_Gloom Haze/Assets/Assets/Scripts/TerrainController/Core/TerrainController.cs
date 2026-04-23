using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainController : MonoBehaviour
{
    [Header("Config")]
    public TerrainConfig config;

    private TerrainContext context;

    private void Start()
    {
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();

        if (config == null)
        {
            Debug.LogError("TerrainConfig chưa được gán!");
            return;
        }

        PrepareTerrainData(terrain);

        context = new TerrainContext(terrain, config);

        InitSeed();

        // =========================
        // PIPELINE (chưa gọi module)
        // =========================
        // 1. HeightMap
        // 2. Biome
        // 3. Destruction
        // 4. Structures
        // 5. Decoration
        // 6. Gameplay Map

        ApplyHeightMap();
    }

    private void PrepareTerrainData(Terrain terrain)
    {
        TerrainData data = terrain.terrainData;

        data.heightmapResolution = config.heightMapResolution;
        data.size = new Vector3(
            config.terrainWidth,
            config.terrainHeight,
            config.terrainLength
        );
    }

    private void InitSeed()
    {
        if (config.useRandomSeed)
        {
            context.seed = Random.Range(0, int.MaxValue);
        }
        else
        {
            context.seed = config.seed;
        }

        Random.InitState(context.seed);
    }

    private void ApplyHeightMap()
    {
        context.terrainData.SetHeights(0, 0, context.heightMap);
    }
}
