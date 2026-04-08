using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public BiomeData BiomeData;
    public int Iterations = 1000;

    [SerializeField] Collider _zoneCollider;
    private int _seed;

    private HashSet<GameObject> _spawnedObjects = new();
    private HashSet<GameObject> _spawnedBiomeObjects = new();

    private void Start()
    {
        LoadWorld();
    }

    public void LoadWorld()
    {
        //if (has not save)
        GenerateWorld();
    }

    public void GenerateWorld()
    {
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
        for (int i = 0; i < BiomeData.SpawnDatas.Count; i++)
        {
            SpawnBiomeGroup(BiomeData.SpawnDatas[i], i);
        }
    }

    private void SpawnBiomeGroup(BiomeSpawnData data, int index)
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
                return;
            }

            Vector3 pos = new Vector3(
                Random.Range(min.x, max.x),
                _zoneCollider.bounds.center.y,
                Random.Range(min.z, max.z)
            );

            Debug.Log((pos, Vector3.down, _zoneCollider.bounds.extents.y, ~0, QueryTriggerInteraction.Ignore, Physics.Raycast(pos, Vector3.down, out _, _zoneCollider.bounds.extents.y, ~0, QueryTriggerInteraction.Ignore)));
            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, _zoneCollider.bounds.extents.y, ~0, QueryTriggerInteraction.Ignore))
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
        }
    }

    private bool HasAnyNearby(Vector3 pos, float collidingSize)
    {
        return _spawnedObjects.Any(x => Vector3.Distance(x.transform.position, pos) < collidingSize);
    }

    private bool IsFitDensity(Vector3 pos, float densityDistance, float size)
    {
        return _spawnedBiomeObjects.Any(x => Vector3.Distance(x.transform.position, pos) <= densityDistance);
    }
}
