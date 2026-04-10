using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Voronoi _voronoi;
    [SerializeField] BiomeGenerator _biomeGenerator;
    [SerializeField] TerrainGenerator _terrainGenerator;

    public Biomes AvailableBiomes;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        _voronoi.Generate(out HashSet<Collider> colliders);
        _biomeGenerator.Generate(AvailableBiomes, colliders, out Dictionary<Collider, BiomeData> biomes);
        _terrainGenerator.Generate(biomes);
        GenerateBorders();
    }

    public void GenerateBorders()
    {
        Vector3 worldSize = _terrainGenerator.WorldSize / 2.5f;
        BoxCollider border;

        // left
        border = new GameObject("LeftBorder").AddComponent<BoxCollider>();
        border.center = new Vector3(-worldSize.x / 2, 0, 0);
        border.size = new Vector3(10, 40, worldSize.z);

        // top
        border = new GameObject("TopBorder").AddComponent<BoxCollider>();
        border.center = new Vector3(0, 0, worldSize.z / 2);
        border.size = new Vector3(worldSize.x, 40, 10);

        // right
        border = new GameObject("RightBorder").AddComponent<BoxCollider>();
        border.center = new Vector3(worldSize.x / 2, 0, 0);
        border.size = new Vector3(10, 40, worldSize.z);

        // bottom
        border = new GameObject("BottomBorder").AddComponent<BoxCollider>();
        border.center = new Vector3(0, 0, -worldSize.z / 2);
        border.size = new Vector3(worldSize.x, 40, 10);
    }
}
