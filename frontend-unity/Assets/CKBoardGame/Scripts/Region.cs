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

    public void InitRegion(RegionData data, Color32 serializeColor, Texture2D regionsMap, Material regionMaterial)
    {
        this.name = data.name;
        this.Color = serializeColor;

        Vector3? castlePos = null;
        Vector3? knightPos = null;
        var regionMesh = GenerateMesh(regionsMap, out castlePos, out knightPos);

        if(castlePos.HasValue && data.castle)
        {
            var castleInst = GameController.Instantiate(GameController.Instance.CastlePrefab, this.transform);
            castleInst.transform.position = castlePos.Value;
        }
        if(knightPos.HasValue && data.mobilized)
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
        renderer.sharedMaterial.color = serializeColor;
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

        var verts = new List<Vector3>();
        var tris = new List<int>();

        for (int x = 0; x < regionsMap.width; x++)
        {
            for (int z = 0; z < regionsMap.height; z++)
            {
                var origin3d = new Vector3(z, 0, x);

                var pixel = GetPixel(pixels, x, z, regionsMap.width);
                var isRegionPixel = IsSameColor(pixel, this.Color);
                var isCastlePixel = IsSameColor(pixel, (Color32)UnityEngine.Color.white);
                var isKnightPixel = IsSameColor(pixel, (Color32)UnityEngine.Color.black);

                if (isCastlePixel && x > 0 && IsSameColor(GetPixel(pixels, x - 1, z, regionsMap.width), this.Color))
                {
                    castlePos = origin3d + new Vector3( 0.5f, 0f,0.5f);
                    isRegionPixel = true;
                }

                if (isKnightPixel && x > 0 && IsSameColor(GetPixel(pixels, x - 1, z, regionsMap.width), this.Color))
                {
                    knightPos = origin3d + new Vector3(0.5f, 0f, 0.5f);
                    isRegionPixel = true;
                }

                if (!isRegionPixel)
                    continue;


                var quadStartIndex = verts.Count;
                verts.Add(origin3d + new Vector3(0, 0, 0));
                verts.Add(origin3d + new Vector3(1, 0, 0));
                verts.Add(origin3d + new Vector3(0, 0, 1));
                verts.Add(origin3d + new Vector3(1, 0, 1));

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