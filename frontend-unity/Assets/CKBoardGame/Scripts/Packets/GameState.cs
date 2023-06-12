using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public Dictionary<string, RegionData> map;
}

public class RegionData
{
    public string name;
    public string culture;
    //public string[][] edges;
    //public Dictionary<string, string[]> seas;
    //public object owner;
    public bool castle;
    public bool mobilized;
    //public object duke;
    //public object[] tokens;
}