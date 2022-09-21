using System.Collections.Generic;
using UnityEngine;
using ObstacleCreation = LevelEditor.ObstacleCreation;

[System.Serializable]
public class ObstacleParentItem
{
    public List<ItemData> obstacles;
    public float anchor;
    public ObstacleCreation obstacleType;
    public Vector3 parentPosition;
    public Vector3 colliderCenter;
    public Vector3 colliderSize;
    public int id;

}
