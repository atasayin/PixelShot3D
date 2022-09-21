using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class LevelEditorManager : AbstarctMonoSingleton<LevelEditorManager>
    {
        public const int DEFAULT_HEALTH = 5;
        public const int DEFAULT_MOUSE_AREA_RADIUS = 0;
        public const int DEFAULT_NUM_ROWS = 20; 
        public const int DEFAULT_NUM_COLS = 17; 
        public const int DEFAULT_CELL_SIZE = 30;
        public const float DEFAULT_PIXEL_SCALE = 0.2f;
        public const int EMPTY_ASSET_SHIFT = 1;
        public const float DEFAULT_OFFSET_X = 0.2f;
        public const float DEFAULT_OFFSET_Y = 0.2f;
        public const int NUMBER_COLORS = 8;

        public static Color32 DEFAULT_MAIN_LEVEL_COLOR = Texture2DManager.purble; 

        private Level _level;
        private StyleManager styleManager;

        private GridEditor _grid;

        public bool isGridInit = false;        
               
        public bool isErasing = false;
        public bool isObstacleCreating= false;

        public WindowType SELECTED_WINDOW;

        public delegate void LoadLevel();
        public static event LoadLevel OnLoadLevelPressed;


        private void Start()
        {
            styleManager = new StyleManager();
            styleManager.Init();
        }

        public void InitGrid(int health, int numRows, int numCols, float scale, Vector2 offset, Color32 levelBoardColor, Color32 ballHubColor)
        {            
            _grid = new GridEditor(health, numRows, numCols, scale, offset, levelBoardColor, ballHubColor);
            _grid.InitGridCells(styleManager.EmptyCellStyle, styleManager.UnColorCellStyle);
            isGridInit = true;

        }

        public bool IsLevelExist(int levelNumber)
        {
            return !(null == LevelLoader.LoadLevel(levelNumber));
        }

        public void SaveCurrentLevelToAsset(int levelNumber)
        {
            LevelLoader.SaveLevel(_grid, levelNumber);
        }

        public void LoadGivenLevelFromAssets(int levelNumber)
        {
            _level = LevelLoader.LoadLevel(levelNumber);
            
           // styleManager.ColorMenuSetColorSettings(_level.colorPallete);
            styleManager.Colors = _level.colorPallete;
            styleManager.CreateColorToGUIMapping();

            OnLoadLevelPressed?.Invoke();

            if (_level != null)
            {
                _grid = new GridEditor(_level, styleManager.CellTypeToGUI, styleManager.ColorPalleteIndexToGUI);
                isGridInit = true;
                _grid.Convert2Dto3D();
            }

        }

        public void ChooseGridCellsInRadius(Vector2 mousePosition, Vector2 gridStart, int mouseAreaRadius)
        { 
            List<GridCell> selectedCells = _grid.GiveSelectedGridCellInRadius(mousePosition, gridStart, mouseAreaRadius);
            
            foreach(GridCell cell in selectedCells)
            {
                if (isErasing)
                {
                    cell.Type = CellType.Empty;
                    cell.Cell2D.Style = styleManager.EmptyCellStyle;
                    cell.Cell2D.ColorStyle = styleManager.UnColorCellStyle;
                }
                else 
                {
                    if (SELECTED_WINDOW == WindowType.GridEditorWindow)
                    {
                        cell.Type = styleManager.SelectedCellType;
                        cell.Cell2D.Style = styleManager.SelectedCellStyle;
                        cell.Cell2D.ColorStyle = styleManager.EmptyCellStyle;

                    } else if (SELECTED_WINDOW == WindowType.GridColor)
                    {
                        if (cell.Type != CellType.Empty)
                        {
                            cell.Cell2D.ColorStyle = styleManager.SelectedCellStyle;
                            cell.ColorPalleteIndex = styleManager.SelectedColorIndex; 
                        }

                    }
                }
            }

        }

        public void ChooseGridCellsInVerticalForObstacles(Vector2 mousePosition, Vector2 gridStart, int lenght, float anchor)
        {
            List<GridCell> selectedCells = _grid.GiveSelectedGridCellInWidthHeightFromLeftUp(mousePosition, gridStart, lenght, 1);
            
            SetUpObstacleStylesForCells(selectedCells);
            SetObstacleParent(selectedCells, anchor, ObstacleCreation.Vertical);
            
        }


        public void ChooseGridCellsInHorizontalForObstacles(Vector2 mousePosition, Vector2 gridStart, int lenght, float anchor)
        {
            List<GridCell> selectedCells = _grid.GiveSelectedGridCellInWidthHeightFromLeftUp(mousePosition, gridStart, 1, lenght);

            SetUpObstacleStylesForCells(selectedCells);
            SetObstacleParent(selectedCells, anchor, ObstacleCreation.Horizontal);
            
        }

        private void SetUpObstacleStylesForCells(List<GridCell> selectedCells)
        {
            List<GridCell> newObstacleCells = new List<GridCell>();
            
            foreach (GridCell cell in selectedCells)
            {
                cell.Type = CellType.ObstaclePixel;
                cell.Cell2D.Style = styleManager.ObstacleCellStyle;
                cell.Cell2D.ColorStyle = styleManager.ObstacleCellStyle;

                newObstacleCells.Add(cell);

            }

        }

        private void SetObstacleParent(List<GridCell> selectedCells, float anchor, ObstacleCreation obstacleType)
        {
            ObstacleParent obstacleParent;

            obstacleParent.anchor = anchor;
            obstacleParent.obstacles = selectedCells;
            obstacleParent.obstacleType = obstacleType;
            obstacleParent.gameObject = null;
            _grid.ObstacleParents.Add(obstacleParent);

           
        }

        public void Convert2Dto3D()
        {
            _grid.Convert2Dto3D();
        }

        public void DestroyAllPixels()
        {
            _grid.DestroyAllPixels3D();
        }

      
        public void ChooseGridEditorWindow()
        {
            SELECTED_WINDOW = WindowType.GridEditorWindow;
            styleManager.SelectedCellStyle = styleManager.Styles[0];
        }

        public void ChooseGridColorWindow()
        {
            SELECTED_WINDOW = WindowType.GridColor;
            styleManager.SelectedCellStyle = styleManager.ColorStyles[0];
        }
        private void OnApplicationQuit()
        {
            ObjectManager.Instance.ChangeColorOfLevelBoard(DEFAULT_MAIN_LEVEL_COLOR);
        }

        #region Getter Setter Function

        public GridEditor Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }


        public StyleManager StyleManager
        {

            get { return styleManager; }
            set { styleManager = value; }

        }

        #endregion

    }
}
#endif