using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RegionColorData", menuName = "RegionColorData", order = 51)]
public class RegionColorData : ScriptableObject
{
    [SerializeField]
    public List<RegionColorPair> Data;

    [Serializable]
    public class RegionColorPair
    {
        public string Name;
        public Color32 Color;
    }
}
