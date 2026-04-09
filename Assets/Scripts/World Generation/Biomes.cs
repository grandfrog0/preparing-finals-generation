using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "biomes", menuName = "SO/Generation/Biomes/Biomes")]
public class Biomes : ScriptableObject
{
    public List<BiomeData> AvailableBiomes;

    public BiomeData GetRandomBiome()
    {
        float totalProbability = AvailableBiomes.Sum(x => x.Probability);

        float value = Random.Range(0, totalProbability);
        foreach (BiomeData biome in AvailableBiomes)
        {
            if (value < biome.Probability)
            {
                return biome;
            }
            value -= biome.Probability;
        }

        return null;
    }
}
