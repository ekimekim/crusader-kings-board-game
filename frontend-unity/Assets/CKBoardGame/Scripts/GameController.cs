using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; set; }
    public Texture2D RegionsMap;
    public RegionColorData RegionColorData;
    public Material RegionMaterial;
    public GameObject CastlePrefab;
    public GameObject KnightPrefab;

    public TextAsset GameStateSample;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var gameState = JsonConvert.DeserializeObject<GameState>(GameStateSample.text);

        foreach (var region in gameState.map.Values)
        {
            var colorData = RegionColorData.Data.FirstOrDefault(d => d.Name == region.name);

            if(colorData == null)
            {
                Debug.LogWarning($"skip region {region.name}");
                continue;
            }

            Debug.Log($"add region {region.name}");
            var regionScript = new GameObject("NewRegion").AddComponent<Region>();
            regionScript.transform.SetParent(transform, false);
            regionScript.InitRegion(region, colorData.Color, RegionsMap, RegionMaterial);
        }
    }

    void Update()
    {
        
    }
}
