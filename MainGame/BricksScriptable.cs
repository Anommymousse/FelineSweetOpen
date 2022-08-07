using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ScriptBricks", menuName = "ScriptableBricksList", order = 1)]
public class BricksScriptable : ScriptableObject
{
    public List<Tile> TiledBricks;
    public List<Sprite> TiledBricksSprites;
}
