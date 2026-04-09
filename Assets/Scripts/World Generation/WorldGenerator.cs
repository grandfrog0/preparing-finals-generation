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
    }
}
