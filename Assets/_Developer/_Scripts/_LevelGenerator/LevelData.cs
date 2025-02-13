using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class LevelData
{
    public List<BrickEntry> bricks = new List<BrickEntry>();
}

[Serializable]
public class BrickEntry
{
    public Vector2 position;
    public string objectTag;
}

 