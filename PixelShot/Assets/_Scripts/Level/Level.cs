using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    [Header("Header Data")]
    public int levelNumber; 
    public int health;
    public LevelTypes levelType;
    public Vector3 shinningPosition;

    public Color levelBoardColor;
    public Color ballHubColor;
    public Color32[] colorPallete;
    public int numRows;
    public int numCols;
    public float scale;
    public Vector2 offset;
    public int simplePixelCount;

    [Header("Item Data")]
    public int[] inGameObjectCountMapId;

    public List<ItemData> items;

    public List<ObstacleParentItem> obstacleParents;

    public List<AnimationData> animationDatas;

}
