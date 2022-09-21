using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class GridEditor
    {

        #region Variables

        private const float DEFAULT_CELL_WIDTH_3D = 3.6f;
        private const float DEFAULT_CELL_HEIGHT_3D = 4.2f;
        private const float DEFAULT_CELL_SIZE_3D = 0.2f;
        public const int DEFAULT_CELL_SIZE_2D = 30;

        private const int GRID_WIDTH = 510;
        private const int GRID_HEIGHT = 600;
        private const int CELL_INTERVAL_OFFSET = 3;
        private Color32 _levelBoardColor;
        private Color32 _ballHubColor;
        private Color32[] _colorPallete;
        private List<ObstacleParent> _obstacleParents;
        private List<AnimationData> _animationDatas;
       
        
        private int _health, _numRows, _numCols;
        private int _cellSize2D;
        private float _scale;
        private GridCell[,] _cells;
        private Vector2 _offset;

        #endregion 

        public GridEditor(int health, int numRows, int numCols, float scale, Vector2 offset, Color32 levelBoardColor, Color32 ballHubColor)
        {
            _health = health;
            _numRows = numRows;
            _numCols = numCols;
            _scale = scale;
            _cells = new GridCell[_numRows, _numCols];
            _offset = offset;

            _levelBoardColor = levelBoardColor;
            _ballHubColor = ballHubColor;
            SetUpPixelScaleAndOffset(numRows, numCols);
            CalculateCellSize2D();
            //CheckForParameters(numRows, numCols, scale, offset);


            _obstacleParents = new List<ObstacleParent>();
            _animationDatas = new List<AnimationData>();




        }


        public GridEditor(Level level, Dictionary<CellType, GUIStyle> cellTypeToGUI, GUIStyle[] colorPalatteToGUI)
        {
            _health = level.health;
            _numRows = level.numRows;
            _numCols = level.numCols;
            _levelBoardColor = level.levelBoardColor;
            _ballHubColor = level.ballHubColor;
            _scale = level.scale;
            _cells = new GridCell[_numRows, _numCols];
            _offset = level.offset;
            _colorPallete = level.colorPallete;

            CalculateCellSize2D();
            InitGridCells(cellTypeToGUI[CellType.Empty]);

            foreach (ItemData item in level.items)
            {
                int row = item.Row;
                int col = item.Col;

                _cells[row, col] = new GridCell(row, col, item._type, item._colorPalatteIndex);

                Vector2 position2D = new Vector2(_cellSize2D * col, _cellSize2D * row);
                Vector2 scale2D = Vector2.one * (_cellSize2D - CELL_INTERVAL_OFFSET);

                _cells[row, col].Cell2D = new GridCell2DInfo(position2D, scale2D, cellTypeToGUI[item._type], colorPalatteToGUI[item._colorPalatteIndex]);
            }
            _obstacleParents = new List<ObstacleParent>();
            _animationDatas = new List<AnimationData>();
        }

        private void CalculateCellSize2D()
        {
            _cellSize2D = Mathf.FloorToInt(Mathf.Min(GRID_HEIGHT/_numRows, GRID_WIDTH / _numCols));
        }
        
        public void InitGridCells(GUIStyle defaultCellStyle)
        {
            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    _cells[i, j] = new GridCell(i, j);

                    Vector2 position2D = new Vector2(_cellSize2D * j, _cellSize2D * i);
                    Vector2 scale2D = Vector2.one * (_cellSize2D - CELL_INTERVAL_OFFSET);
                    
                    _cells[i, j].Cell2D = new GridCell2DInfo(position2D, scale2D, defaultCellStyle);

                }
            }
        }

        public void InitGridCells(GUIStyle defaultCellStyle, GUIStyle unColorCellStyle)
        {
            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    _cells[i, j] = new GridCell(i, j);

                    Vector2 position2D = new Vector2(_cellSize2D * j, _cellSize2D * i);
                    Vector2 scale2D = Vector2.one * (_cellSize2D - CELL_INTERVAL_OFFSET);

                    _cells[i, j].Cell2D = new GridCell2DInfo(position2D, scale2D, defaultCellStyle, unColorCellStyle);

                }

            }
        }

        public Level GridToLevel()
        {
            Level level = ScriptableObject.CreateInstance<Level>();
            level.health = _health;
            level.numRows = _numRows;
            level.levelBoardColor = _levelBoardColor;
            level.ballHubColor = _ballHubColor;
            level.numCols = _numCols;
            level.scale = _scale;
            level.offset = _offset;
            level.colorPallete = LevelEditorManager.Instance.StyleManager.Colors;
            level.inGameObjectCountMapId = new int[(int)CellType.BitMapMaxLength];
            level.animationDatas = _animationDatas;

            int id = 0;
            List<ObstacleParentItem> obstacleParentItems = new List<ObstacleParentItem>();

            if (_obstacleParents != null)
            {
                foreach (ObstacleParent parent in _obstacleParents)
                {
                    ObstacleParentItem obstacleParent = new ObstacleParentItem();
                    obstacleParent.obstacles = new List<ItemData>();
                    obstacleParent.anchor = parent.anchor;

                    Vector3 firstChildPosition = parent.obstacles[0].GameObject.transform.position;

                    int childCount = parent.obstacles.Count;
                    if (parent.obstacleType == ObstacleCreation.Horizontal)
                    {
                        obstacleParent.parentPosition = firstChildPosition + new Vector3((parent.anchor - 0.5f) * _scale, 0, 0);
                        ColliderGenerator.GenerateColliderX(childCount, parent.anchor, _scale);
                    }
                    else if (parent.obstacleType == ObstacleCreation.Vertical)
                    {
                        obstacleParent.parentPosition = firstChildPosition + new Vector3(0, 0, (parent.anchor + 0.5f) * _scale);
                        ColliderGenerator.GenerateColliderZ(childCount, parent.anchor, _scale);
                    }

                    obstacleParent.colliderCenter = ColliderGenerator.ColliderCenter;
                    obstacleParent.colliderSize = ColliderGenerator.ColliderSize;

                    foreach (GridCell obstacleCell in parent.obstacles)
                    {
                        obstacleParent.obstacles.Add(new ItemData(obstacleCell.GameObject.transform, obstacleCell.Row, obstacleCell.Col, CellType.ObstaclePixel));
                    }

                    obstacleParent.id = id;
                    id++;

                    obstacleParentItems.Add(obstacleParent);
                }
            }

            level.obstacleParents = obstacleParentItems;

            List<ItemData> items = new List<ItemData>();

            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    GridCell cell = _cells[i, j];
                    CellType cellTypeOfUnit = cell.Type;
                    
                    if (cellTypeOfUnit != CellType.Empty && cellTypeOfUnit != CellType.ObstaclePixel)
                    {
                        ItemData item = new ItemData(cell.GameObject.transform, i, j, cellTypeOfUnit, cell.ColorPalleteIndex);
                        item.id = id;
                        items.Add(item);

                        level.inGameObjectCountMapId[(int)cellTypeOfUnit]++;                        

                        if (cellTypeOfUnit != CellType.FrozenPixel)
                        {
                            level.simplePixelCount++;
                        }
                        id++;
                    }

                    
                }
            }

            level.items = items;

            return level;
        }

        public void Convert2Dto3D()
        {
            IdAssignToCells();
            ObjectManager.Instance.Offset = Utility.Vector2Dto3D(_offset);
            ObjectManager.Instance.ChangeColorOfLevelBoard(_levelBoardColor);

            CellToGameObjectConverter converter = new CellToGameObjectConverter(this);
            converter.Convert();
        }

        private void IdAssignToCells()
        {
            int id = 0;

            foreach(GridCell cell in _cells)
            {
                if (cell.Type != CellType.Empty)
                {
                    cell.ID = id;
                    id++;
                }
            }

        }


        public void DestroyAllPixels3D()
        { 
            foreach (GridCell cell in _cells)
            {
                if (cell.Type != CellType.Empty)
                {
                    ObjectManager.Instance.ReturnToPool(cell);
                }
            }
        }


        public void SetGrid(int health, int numRows, int numCols, float scale, Vector2 offset, Color32 levelBoardColor)
        {
            _health = health;
            _numRows = numRows;
            _numCols = numCols;
            _scale = scale;
            _cells = new GridCell[_numRows, _numCols];
            _offset = offset;
            _levelBoardColor = levelBoardColor;
        
            SetUpPixelScaleAndOffset(numRows, numCols);
            CalculateCellSize2D();

        }
        public void SetGrid(int health, int numRows, int numCols, float scale, Vector2 offset, Color32 levelBoardColor, Color32 ballHubColor)
        {
            _health = health;
            _numRows = numRows;
            _numCols = numCols;
            _scale = scale;
            _cells = new GridCell[_numRows, _numCols];
            _offset = offset;
            _levelBoardColor = levelBoardColor;
            _ballHubColor = ballHubColor;
            SetUpPixelScaleAndOffset(numRows, numCols);
            CalculateCellSize2D();
        }

        public void DrawGrid(Vector2 gridStart)
        {
            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    _cells[i,j].Cell2D.Draw(gridStart);
                }
            }
        }

        public void DrawColoredGrid(Vector2 gridStart)
        {
            for (int i = 0; i < _numRows; i++)
            {
                for (int j = 0; j < _numCols; j++)
                {
                    _cells[i, j].Cell2D.DrawColored(gridStart);
                }
            }
        }

        public List<GridCell> GiveSelectedGridCellInRadius(Vector2 mousePosition, Vector2 gridStart, int mouseAreaRadius)
        {
            int colCenterCell = Mathf.FloorToInt((mousePosition.x - gridStart.x) / _cellSize2D);
            int rowCenterCell = Mathf.FloorToInt((mousePosition.y - gridStart.y) / _cellSize2D);

            List<GridCell> selectedCells = new List<GridCell>();

            for (int i = -mouseAreaRadius; i <= mouseAreaRadius; i++)
            {
                for(int j = -mouseAreaRadius; j <= mouseAreaRadius; j++)
                {
                    if ( (rowCenterCell + i < _numRows) && (rowCenterCell + i >= 0) && (colCenterCell + j < _numCols) && (colCenterCell + j >= 0))
                    {
                        selectedCells.Add(_cells[rowCenterCell + i, colCenterCell + j]);
                    }
                }
            }

            return selectedCells;
        }

        public List<GridCell> GiveSelectedGridCellInWidthHeightFromLeftUp(Vector2 mousePosition, Vector2 gridStart, int height, int width)
        {
            int colLeftUpCell = Mathf.FloorToInt((mousePosition.x - gridStart.x) / _cellSize2D);
            int rowLeftUpCell = Mathf.FloorToInt((mousePosition.y - gridStart.y) / _cellSize2D);

            List<GridCell> selectedCells = new List<GridCell>();
            

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((rowLeftUpCell + i < _numRows) && (rowLeftUpCell + i >= 0) && (colLeftUpCell + j < _numCols) && (colLeftUpCell + j >= 0))
                    {
                        selectedCells.Add(_cells[rowLeftUpCell + i, colLeftUpCell + j]);
                    }
                    
                }
            }
            

            return selectedCells;

            
        }


        private void SetUpPixelScaleAndOffset(int numRows, int numCols)
        {

            int numOffset = 2;
            
            float scaleByWidth = DEFAULT_CELL_WIDTH_3D / (numCols + numOffset);
            float scaleByHeight = DEFAULT_CELL_HEIGHT_3D / (numRows + numOffset);


            if (scaleByWidth < scaleByHeight)
            {
                _scale = scaleByWidth;
                _offset.x = scaleByWidth * 1.5f;
                _offset.y = (DEFAULT_CELL_HEIGHT_3D - _scale * numRows) / (numOffset + 1);
            }
            else
            {
                _scale = scaleByHeight;
                _offset.y = scaleByHeight * 1.5f;
                _offset.x = (DEFAULT_CELL_WIDTH_3D - _scale * NumCols) / (numOffset + 1);
            }

            _offset.y *= -1;


        }

        #region Getter Setter

        public int NumRows {

            get { return _numRows; }
            set { _numRows = value; }
            
        }

        public int NumCols
        {

            get { return _numCols; }
            set { _numCols = value; }

        }

        public int CellSize2D
        {

            get { return _cellSize2D; }
            set { _cellSize2D = value; }

        }

        public float Scale
        {

            get { return _scale; }
            set { _scale = value; }

        }

        public GridCell[,] Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        public Color32 LevelBoardColor
        {
            get { return _levelBoardColor; }
            set { _levelBoardColor = value; }
        }

        public List<ObstacleParent> ObstacleParents
        {
            get { return _obstacleParents; }
            set { _obstacleParents = value; }
        }

        public List<AnimationData> AnimationDatas
        {
            get { return _animationDatas; }
            set { _animationDatas = value; }
        }

        #endregion



    }

}
#endif