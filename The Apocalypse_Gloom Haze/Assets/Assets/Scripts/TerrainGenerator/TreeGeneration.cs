using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;
    public int treeCount = 100;

    void Start()
    {
        for (int i = 0; i < treeCount; i++)
            AddTree();
    }

    public void AddTree()
    {
        TerrainData data = terrain.terrainData;
        float terrainwidth = data.size.x;
        float terrainlength = data.size.z;
        float x = Random.Range(0, terrainwidth);
        float z = Random.Range(0, terrainlength);
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;
        Vector3 normal = data.GetInterpolatedNormal(x / terrainwidth, z / terrainlength);
        float slope = Vector3.Angle(normal, Vector3.up);
        if (slope < 30 && y < 50)
            Instantiate(treePrefab, new Vector3(x, y, z), Quaternion.identity);

    }



}
