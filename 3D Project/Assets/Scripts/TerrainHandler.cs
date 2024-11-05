using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainHandler : MonoBehaviour
{
    // Start is called before the first frame update

    Terrain terrain;
    TerrainCollider collider;

    public float resolution;


    public int width = 513;
    public int height = 20;
    public int depth = 513;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }
    private void Update()
    {
        collider = GetComponent<TerrainCollider>();
        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        collider.terrainData = terrain.terrainData;
    }
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);

        terrainData.SetHeights(0, 0, GenerateHeightMap());
        return terrainData;
    }

    public float[,] GenerateHeightMap()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / depth * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
