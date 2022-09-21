#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class CellToGameObjectConverter
    {
        private Color _levelBoardColor;
        private GridCell[,] _cells;
        private float _scale;
        private int _numRows, _numCols;
        private List<ObstacleParent> _obstacleParents;

        public CellToGameObjectConverter(GridEditor grid)
        {
            _cells = grid.Cells;
            _scale = grid.Scale;
            _numCols = grid.NumCols;
            _numRows = grid.NumRows;
            _levelBoardColor = grid.LevelBoardColor;
            _obstacleParents = grid.ObstacleParents;
        }

        public void SetCellToGameObjectConvertor(GridEditor grid)
        {
            _cells = grid.Cells;
            _scale = grid.Scale;
            _numCols = grid.NumCols;
            _numRows = grid.NumRows;
            _levelBoardColor = grid.LevelBoardColor;
            _obstacleParents = grid.ObstacleParents;
        }

        public void Convert()
        {
            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    GridCell cell = _cells[i, j];
                    CellType cellType = cell.Type;

                    if (cellType != CellType.Empty)
                    {
                        cell.GameObject = CreateGameObject(cell, i, j);
                    }

                }
            }

            ObstacleParentConnection();
        }

        private void ObstacleParentConnection()
        {
            if (_obstacleParents == null)
            {
                return;
            }
            
            for(int i = 0; i < _obstacleParents.Count; i++ )
            {
                ObstacleParent parent = _obstacleParents[i];

                List<GridCell> childs = parent.obstacles;
                GameObject parentObject = ObjectManager.Instance.GetObstacleParentObjectFromPool();
                Vector3 firstChildPosition = childs[0].GameObject.transform.position;
                

                if (parent.obstacleType == ObstacleCreation.Horizontal)
                {
                    parentObject.transform.position = firstChildPosition + new Vector3((parent.anchor - 0.5f) * _scale, 0, 0);
                }
                else if (parent.obstacleType == ObstacleCreation.Vertical)
                {
                    parentObject.transform.position = firstChildPosition + new Vector3(0, 0, (parent.anchor + 0.5f) * _scale);
                }

                foreach (GridCell child in childs)
                {
                    child.GameObject.transform.SetParent(parentObject.transform);
                }
                

                if (parent.obstacleType == ObstacleCreation.Horizontal)
                {
                    ColliderGenerator.GenerateColliderX(childs.Count, parent.anchor, _scale);
                }
                else if (parent.obstacleType == ObstacleCreation.Vertical)
                {
                    ColliderGenerator.GenerateColliderZ(childs.Count, parent.anchor, _scale);
                }
                ColliderGenerator.SetBoxCollider(parentObject.GetComponent<BoxCollider>());
                parent.gameObject = parentObject;


            }
        }

        private GameObject CreateGameObject(GridCell gridCell, int row, int col)
        {
            CellType cellType = gridCell.Type;
            int colorIndex = gridCell.ColorPalleteIndex;
            int id = gridCell.ID;

            GameObject go = ObjectManager.Instance.GetObjectFromPoolEditor((int)(cellType));
            go.GetComponent<IDStore>().ID = id;

            switch (cellType)
            {   
                case CellType.FrozenPixel:
                    ObjectManager.Instance.PlacePixelToMapOutlineCell(go, _scale, row, col);
                    break;
                
                case CellType.ObstaclePixel:
                    ObjectManager.Instance.ColorizedObject(go, _levelBoardColor);
                    ObjectManager.Instance.PlacePixelToMapFullCell(go, _scale, row, col);
                    break;

                case CellType.SimpleCylinderPixel:
                    ObjectManager.Instance.ColorizedObject(go, LevelEditorManager.Instance.StyleManager.Colors[colorIndex]);
                    ObjectManager.Instance.PlacePixelToMapOutlineCellScaleVector(go, new Vector3(_scale, _scale * 0.2f, _scale), row, col);
                    break;

                default: // SimplePixels
                    ObjectManager.Instance.ColorizedObject(go, LevelEditorManager.Instance.StyleManager.Colors[colorIndex]);
                    ObjectManager.Instance.PlacePixelToMapOutlineCell(go, _scale, row, col);
                    break;
            }

            return go;
        }

        public List<ObstacleParent> ObstacleParents
        {
            get { return _obstacleParents; }
        }
    }


}
#endif