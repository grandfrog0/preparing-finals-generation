using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "biome", menuName = "SO/Generation/Biomes/Biome Data")]
public class BiomeData : ScriptableObject
{
    public string ID;
    public float Probability;

    public List<BiomeSpawnData> SpawnDatas;
}
