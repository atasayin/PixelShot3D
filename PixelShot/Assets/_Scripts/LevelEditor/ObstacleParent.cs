#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
namespace LevelEditor
{
    public struct ObstacleParent
    {
        public List<GridCell> obstacles;
        public float anchor;
        public ObstacleCreation obstacleType;
        public GameObject gameObject;

        public ObstacleParent(List<GridCell> obstacles, float anchor, ObstacleCreation obstacleType, GameObject gameObject)
        {
            this.obstacles = obstacles;
            this.anchor = anchor;
            this.obstacleType = obstacleType;
            this.gameObject = gameObject;
        }

    }
}
#endif
