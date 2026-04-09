using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [SerializeField] float _mapSize = 200;
    [SerializeField] int _pointCount = 100;
    private int _meshLayer;

    public void Generate(out HashSet<Collider> meshColliders)
    {
        _meshLayer = LayerMask.NameToLayer("BiomeCollider");

        Vector2[] sites = new Vector2[_pointCount];
        for (int i = 0; i < _pointCount; i++)
        {
            sites[i] = new Vector2(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize));
        }

        MeshCollider meshCollider;
        meshColliders = new();
        foreach (Vector2 site in sites)
        {
            CreateCell(site, sites, out meshCollider);
            meshColliders.Add(meshCollider);
        }
    }

    private void CreateCell(Vector2 center, Vector2[] sites, out MeshCollider meshCollider)
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

        CreateMesh(center, poly, out meshCollider);
    }

    private void CreateMesh(Vector2 center, List<Vector2> poly, out MeshCollider meshCollider)
    {
        float height = 5.0f;
        GameObject obj = new GameObject("Cell");
        obj.transform.Translate(0, -height, 0);
        MeshFilter mf = obj.AddComponent<MeshFilter>();

        int c = poly.Count;
        Vector3[] verts = new Vector3[(c + 1) * 2];
        int[] tris = new int[c * 12];

        verts[0] = new Vector3(center.x, 0, center.y);
        verts[c + 1] = new Vector3(center.x, height, center.y);

        for (int i = 0; i < c; i++)
        {
            verts[i + 1] = new Vector3(poly[i].x, 0, poly[i].y);
            verts[i + c + 2] = new Vector3(poly[i].x, height, poly[i].y);
        }

        for (int i = 0; i < c; i++)
        {
            // Следующая точка в кольце
            int next = (i + 1 == c) ? 1 : i + 2;

            // Нижняя грань
            tris[i * 3] = 0;
            tris[i * 3 + 1] = i + 1;
            tris[i * 3 + 2] = next;

            // Верхняя грань
            int offsetTop = c * 3;
            tris[offsetTop + i * 3] = c + 1;
            tris[offsetTop + i * 3 + 1] = next + c + 1;
            tris[offsetTop + i * 3 + 2] = i + 1 + c + 1;

            // Боковые грани (два треугольника на одну сторону)
            int offsetSides = c * 6 + i * 6;
            int b1 = i + 1;           // bottom current
            int b2 = next;            // bottom next
            int t1 = i + 1 + c + 1;   // top current
            int t2 = next + c + 1;    // top next

            // Первый треугольник стенки
            tris[offsetSides] = b1;
            tris[offsetSides + 1] = t1;
            tris[offsetSides + 2] = b2;

            // Второй треугольник стенки
            tris[offsetSides + 3] = b2;
            tris[offsetSides + 4] = t1;
            tris[offsetSides + 5] = t2;
        }

        Mesh mesh = new Mesh { vertices = verts, triangles = tris };
        //mesh.RecalculateNormals();

        mf.mesh = mesh;
        meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        obj.layer = _meshLayer;
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
