using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int Iterations = 1000;

    [SerializeField] LayerMask _floorMask;
    private BiomeData _biomeData;
    private Collider _zoneCollider;
    private int _seed;

    private Dictionary<Collider, BiomeData> _biomes;

    private HashSet<GameObject> _spawnedObjects = new();
    private HashSet<GameObject> _spawnedBiomeObjects = new();

    public void Generate(Biomes availableBiomes, HashSet<Collider> colliders, out Dictionary<Collider, BiomeData> biomes)
    {
        biomes = colliders.ToDictionary(x => x, x => availableBiomes.GetRandomBiome());
        _biomes = biomes;
        GenerateWorld(Random.Range(int.MinValue, int.MaxValue));
    }

    public void GenerateWorld(int seed)
    {
        _seed = seed;
        Random.InitState(seed);

        ClearBiomeObjects();

        GenerateObjects();
    }

    private void ClearBiomeObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        _spawnedObjects.Clear();
        _spawnedBiomeObjects.Clear();
    }

    public void GenerateObjects()
    {
        StartCoroutine(GenerateObjectsRoutine());
    }

    private IEnumerator GenerateObjectsRoutine()
    {
        foreach ((Collider zone, BiomeData biome) in _biomes)
        {
            _zoneCollider = zone;
            _biomeData = biome;

            for (int i = 0; i < _biomeData.SpawnDatas.Count; i++)
            {
                yield return StartCoroutine(SpawnBiomeGroupRoutine(_biomeData.SpawnDatas[i], i));
            }
        }
    }
    private IEnumerator SpawnBiomeGroupRoutine(BiomeSpawnData data, int index)
    {
        Random.InitState(_seed + index);
        _spawnedBiomeObjects.Clear();

        float size = _zoneCollider.bounds.size.x * _zoneCollider.bounds.size.z;
        float densityDistance = (150 - data.Variance) / data.Density;
        int spawnMax = Mathf.RoundToInt(data.Density * size / (10f * data.Variance));

        Transform parent = new GameObject(data.name).transform;
        parent.SetParent(transform);
        parent.transform.localPosition = Vector3.zero;

        Vector3 min = _zoneCollider.bounds.min;
        Vector3 max = _zoneCollider.bounds.max;

        for (int i = 0; i < Iterations; i++)
        {
            if (_spawnedBiomeObjects.Count > spawnMax)
            {
                yield break;
            }

            Vector3 pos = new Vector3(
                Random.Range(min.x, max.x),
                max.y + 2,
                Random.Range(min.z, max.z)
            );

            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, _zoneCollider.bounds.size.y + 0.5f, _floorMask) && hit.collider == _zoneCollider)
            {
                pos = hit.point;

                GameObject prefab = data.GetRandomPrefab();

                float groupSize = 0.25f;
                float collidingSize = 0.25f;

                if (!HasAnyNearby(pos, collidingSize) && IsFitDensity(pos, densityDistance, size))
                {
                    GameObject obj = Instantiate(prefab, parent);
                    obj.transform.position = pos;

                    _spawnedObjects.Add(obj);
                    _spawnedBiomeObjects.Add(obj);
                }
            }

            if (i % 100 == 0)
            {
                yield return null;
            }
        }
    }

    private bool HasAnyNearby(Vector3 pos, float collidingSize)
    {
        return _spawnedObjects.Any(x => Vector3.Distance(x.transform.position, pos) < collidingSize);
    }

    private bool IsFitDensity(Vector3 pos, float densityDistance, float size)
    {
        return _spawnedBiomeObjects.All(x => Vector3.Distance(x.transform.position, pos) > densityDistance);
    }
}
