using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [SerializeField] float _mapSize = 200;
    [SerializeField] int _pointCount = 100;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Vector2[] sites = new Vector2[_pointCount];
        for (int i = 0; i < _pointCount; i++)
        {
            sites[i] = new Vector2(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize));
        }

        foreach (Vector2 site in sites)
        {
            CreateCell(site, sites);
        }
    }

    private void CreateCell(Vector2 center, Vector2[] sites)
    {
        List<Vector2> poly = new List<Vector2>() { 
            new Vector2(-_mapSize, -_mapSize), new Vector2(_mapSize, -_mapSize),
            new Vector2(_mapSize, _mapSize), new Vector2(-_mapSize, _mapSize),
        };

        foreach (Vector2 other in sites)
        {
            if (center == other)
            {
                continue;
            }

            Vector2 middle = (center + other) / 2;
            Vector2 direction = (other - center).normalized;

            poly = ClipPolygon(poly, middle, direction);
        }

        CreateMesh(center, poly);
    }

    private void CreateMesh(Vector2 center, List<Vector2> poly)
    {
        GameObject obj = new GameObject("Cell");
        MeshFilter mf = obj.AddComponent<MeshFilter>();

        Vector3[] verts = new Vector3[poly.Count + 1];
        int[] tris = new int[poly.Count * 3];
        verts[0] = new Vector3(center.x, 0, center.y);

        for (int i = 0; i < poly.Count; i++)
        {
            verts[i + 1] = new Vector3(poly[i].x, 0, poly[i].y);
            tris[i * 3] = 0;
            tris[i * 3 + 1] = i + 1;
            tris[i * 3 + 2] = (i + 1 == poly.Count) ? 1 : i + 2;
        }

        mf.mesh = new Mesh { vertices = verts, triangles = tris };
    }

    private List<Vector2> ClipPolygon(List<Vector2> points, Vector2 planePoint, Vector2 planeNormal)
    {
        List<Vector2> res = new();
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[(i + 1) % points.Count];

            float distanceA = Vector3.Dot(a - planePoint, planeNormal);
            float distanceB = Vector3.Dot(b - planePoint, planeNormal);

            if (distanceA <= 0) res.Add(a);
            if ((distanceA > 0) != (distanceB > 0))
            {
                float t = distanceA / (distanceA - distanceB);
                res.Add(Vector2.Lerp(a, b, t));
            }
        }
        return res;
    }
}
