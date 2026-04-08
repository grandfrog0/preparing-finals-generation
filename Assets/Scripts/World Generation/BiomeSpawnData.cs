using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "entitySpawn", menuName = "SO/Generation/Biomes/Entity Spawn Data")]
public class BiomeSpawnData : ScriptableObject
{
    [Range(0, 100)] 
    public float Density = 50f;
    [Range(0, 100)] 
    public float Variance = 50f;

    public List<BiomeSpawn> EntitySpawn;

    public GameObject GetRandomPrefab()
    {
        float totalProbability = EntitySpawn.Sum(x => x.Probability);

        float value = Random.Range(0, totalProbability);
        foreach (BiomeSpawn spawn in EntitySpawn)
        {
            if (value < spawn.Probability)
            {
                return spawn.Prefab;
            }
            value -= spawn.Probability;
        }

        return null;
    }
}
