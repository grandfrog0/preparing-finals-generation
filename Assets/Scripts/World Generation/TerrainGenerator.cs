using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Vector3 WorldSize => _terrain.terrainData.size;
    [SerializeField] Terrain _terrain;
    [SerializeField] LayerMask _floorMask;
    private Dictionary<Collider, BiomeData> _biomes;

    public void Generate(Dictionary<Collider, BiomeData> biomes)
    {
        _biomes = biomes;
        StartCoroutine(GenerateRoutine());
    }
    public IEnumerator GenerateRoutine()
    {
        yield return null;

        TerrainData data = _terrain.terrainData;

        int width = data.alphamapWidth;
        int height = data.alphamapHeight;
        int layersCount = data.alphamapLayers;

        float[,,] alphaMap = new float[width, height, layersCount];

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                float worldX = (float)x / width * data.size.x + _terrain.transform.position.x;
                float worldZ = (float)y / height * data.size.z + _terrain.transform.position.z;
                Vector3 worldPos = new Vector3(worldZ, 5, worldX);

                int layer = GetBiomeAtPoint(worldPos)?.TerrainLayer ?? 0;

                for (int i = 0; i < layersCount; i++)
                {
                    alphaMap[x, y, i] = 0f;
                }

                alphaMap[x, y, layer] = 1f;
            }
            yield return null;
        }

        data.SetAlphamaps(0, 0, alphaMap);
    }

    public BiomeData GetBiomeAtPoint(Vector3 pos)
    {
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 10, _floorMask) && _biomes.ContainsKey(hit.collider))
        {
            return _biomes[hit.collider];
        }
        Debug.Log(("NULL", pos));
        return null;
    }
}
