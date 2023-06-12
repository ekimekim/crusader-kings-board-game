using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Region : MonoBehaviour
{
    public Color32 Color;

    public void InitRegion(string name, Color32 color, Texture2D regionsMap, Material regionMaterial)
    {
        this.name = name;
        this.Color = color;

        Vector3? castlePos = null;
        Vector3? knightPos = null;
        var regionMesh = GenerateMesh(regionsMap, out castlePos, out knightPos);

        if(castlePos.HasValue)
        {
            var castleInst = GameController.Instantiate(GameController.Instance.CastlePrefab, this.transform);
            castleInst.transform.position = castlePos.Value;
        }
        if(knightPos.HasValue)
        {
            var knightInst = GameController.Instantiate(GameController.Instance.KnightPrefab, this.transform);
            knightInst.transform.position = knightPos.Value;
        }

        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = regionMesh;

        var collider = GetComponent<MeshCollider>();
        collider.sharedMesh = regionMesh;

        var renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = new Material(regionMaterial);
        renderer.sharedMaterial.color = color;
    }

    static bool IsSameColor(Color32 color, Color32 regionCol)
    {
        //var isMatch = Mathf.Abs(color.r - regionCol.r) < 1 && Mathf.Abs(color.g - regionCol.g) < 1 && Mathf.Abs(color.b - regionCol.b) < 1;
        var isMatch = (color.r == regionCol.r) && (color.g == regionCol.g) && (color.b == regionCol.b);
        return isMatch;
    }

    Mesh GenerateMesh(Texture2D regionsMap, out Vector3? castlePos, out Vector3? knightPos)
    {
        castlePos = null;
        knightPos = null;

        var pixels = regionsMap.GetPixels(0, 0, regionsMap.width, regionsMap.height);

        Debug.Log($"Color pixel look for {this.Color.r},{this.Color.g},{this.Color.b} for {this.gameObject.name}");

        var verts = new List<Vector3>();
        var tris = new List<int>();

        for (int x = 0; x < regionsMap.width; x++)
        {
            for (int z = 0; z < regionsMap.height; z++)
            {
                var pixel = GetPixel(pixels, x, z, regionsMap.width);
                var isRegionPixel = IsSameColor(pixel, this.Color);
                var isCastlePixel = IsSameColor(pixel, (Color32)UnityEngine.Color.white);
                var isKnightPixel = IsSameColor(pixel, (Color32)UnityEngine.Color.black);

                if (isCastlePixel && x > 0 && IsSameColor(GetPixel(pixels, x - 1, z, regionsMap.width), this.Color))
                {
                    castlePos = new Vector3(x + 0.5f, 0f, z + 0.5f);
                    isRegionPixel = true;
                }

                if (isKnightPixel && x > 0 && IsSameColor(GetPixel(pixels, x - 1, z, regionsMap.width), this.Color))
                {
                    knightPos = new Vector3(x + 0.5f, 0f, z + 0.5f);
                    isRegionPixel = true;
                }

                if (!isRegionPixel)
                    continue;

                var quadStartIndex = verts.Count;
                verts.Add(new Vector3(x, 0, z));
                verts.Add(new Vector3(x + 1, 0, z));
                verts.Add(new Vector3(x, 0, z + 1));
                verts.Add(new Vector3(x + 1, 0, z + 1));

                tris.Add(quadStartIndex + 1);
                tris.Add(quadStartIndex + 0);
                tris.Add(quadStartIndex + 2);

                tris.Add(quadStartIndex + 1);
                tris.Add(quadStartIndex + 2);
                tris.Add(quadStartIndex + 3);
            }
        }

        var mesh = new Mesh();
        mesh.name = $"{verts.Count} {this.name}";
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = verts.Select(v => Vector3.up).ToArray();
        return mesh;
    }
    
    static Color32 GetPixel(Color[] pixels, int x, int z, int width)
    {
        Color32 color = pixels[(x * width) + z];
        return color;
    }

}