using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; set; }
    public Texture2D RegionsMap;
    public RegionColorData RegionColorData;
    public Material RegionMaterial;
    public GameObject CastlePrefab;
    public GameObject KnightPrefab;

    void Start()
    {
        Instance = this;
        foreach (var row in RegionColorData.Data)
        {
            var region = new GameObject("NewRegion").AddComponent<Region>();
            region.transform.SetParent(transform, false);
            region.InitRegion(row.Name, row.Color, RegionsMap, RegionMaterial);
        }
    }

    void Update()
    {
        
    }
}
