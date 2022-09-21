using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class GridEditorInputManager
    {
        public const int RIGHT_CLICK_BUTTON = 1;
        public const int MIDDLE_CLICK_BUTTON = 2;
        public bool setColorToogleEnable = false;
        public bool pixelSizeToogleEnable = false;
        private const float OBSTACLE_DISCRET_PRECESION = 0.5f;

        public bool isErasing = false;
        public bool isApplyPressed = false;
        public bool isResetPressed = false;
        public bool isColorizedPressed = false;
        public bool[] isStylePressed;
        public bool[] isColorStylePressed;

        public bool isPrintPressed = false;
        public LevelTypes levelTypes;
        public int health;
        public int numRows;
        public int numCols;
        public int cellSize;
        public int mouseAreaRadius;
        public float pixelScale;
        public float offsetX;
        public float offsetY;
        public Color32[] colors;
        public Color32 levelBoardColor;
        public Color32 ballHubColor;
        public ObstacleCreation obstacleCreateType;
        public int obstacleLength;
        public float obstacleAnchor;


        private const int GRID_WIDTH = 510;
        private const int GRID_HEIGHT = 600;

        public GridEditorInputManager()
        {
            levelTypes = LevelTypes.NormalLevel;
            health = LevelEditorManager.DEFAULT_HEALTH;
            numRows = LevelEditorManager.DEFAULT_NUM_ROWS;
            numCols = LevelEditorManager.DEFAULT_NUM_COLS;
            cellSize = LevelEditorManager.DEFAULT_CELL_SIZE;
            mouseAreaRadius = LevelEditorManager.DEFAULT_MOUSE_AREA_RADIUS;
            pixelScale = LevelEditorManager.DEFAULT_PIXEL_SCALE;
            offsetX = LevelEditorManager.DEFAULT_OFFSET_X;
            offsetY = LevelEditorManager.DEFAULT_OFFSET_Y;
            levelBoardColor = LevelEditorManager.DEFAULT_MAIN_LEVEL_COLOR;
            ballHubColor = LevelEditorManager.DEFAULT_MAIN_LEVEL_COLOR;
            isStylePressed = new bool[LevelEditorManager.Instance.StyleManager.Styles.Length];

            int colorStylesLength = LevelEditorManager.Instance.StyleManager.ColorStyles.Length;
            isColorStylePressed = new bool[colorStylesLength];
            colors = new Color32[colorStylesLength];
            
            LevelEditorManager.Instance.StyleManager.ColorMenuSetColorSettings(colors);
            LevelEditorManager.Instance.StyleManager.Colors = colors;


            LevelEditorManager.OnLoadLevelPressed += SetInputColorsOnLoad;
        }

        #region Mouse

        public void OnMouseEvents(Event e, Vector2 gridStart)
        {
            Vector3 currentMousePosition = e.mousePosition;
            EventType currentEventType = e.type;

            if (IsMouseInGrid(currentMousePosition, gridStart))
            {
                if (currentEventType == EventType.MouseDown || currentEventType == EventType.MouseDrag)
                {
                    if (e.button == MIDDLE_CLICK_BUTTON)
                    {
                        obstacleAnchor = Utility.FloatRounder(OBSTACLE_DISCRET_PRECESION, obstacleAnchor);

                        if (obstacleCreateType == ObstacleCreation.Horizontal)
                        { 
                              LevelEditorManager.Instance.ChooseGridCellsInHorizontalForObstacles(currentMousePosition, gridStart, obstacleLength, obstacleAnchor);
                        }
                        else if (obstacleCreateType == ObstacleCreation.Vertical)
                        {
                            LevelEditorManager.Instance.ChooseGridCellsInVerticalForObstacles(currentMousePosition, gridStart, obstacleLength, obstacleAnchor);
                        }

                        LevelEditorManager.Instance.isObstacleCreating = true;
                    }
                    else
                    {
                        if (e.button == RIGHT_CLICK_BUTTON)
                        {
                            LevelEditorManager.Instance.isErasing = true;
                        }

                        LevelEditorManager.Instance.ChooseGridCellsInRadius(currentMousePosition, gridStart, mouseAreaRadius);
                    }
                                      
                    GUI.changed = true;

                    if (currentEventType == EventType.MouseDrag)
                    {
                        e.Use();
                    }

                }
                LevelEditorManager.Instance.isErasing = false;

                return;

            }

        }


        private bool IsMouseInGrid(Vector2 mousePosition, Vector2 gridStart)
        {
            return (mousePosition.x > gridStart.x) &&
                    (mousePosition.x < (gridStart.x + cellSize * numCols)) &&
                    (mousePosition.y > gridStart.y) &&
                    (mousePosition.y < (gridStart.y + cellSize * numRows));
        }

        #endregion

        #region Buttons


        public void OnButtonEvents()
        {

            if (isApplyPressed)
            {
                OnSetParsButtonEvents();
                return;
            }

            if (isResetPressed)
            {
                OnResetButtonEvents();
                return;
            }

            if (isPrintPressed)
            {
                OnPrintButtonEvents();
                return;
            }

            OnStyleButtonEvents();
        }
 

        public void OnColorButtonEvents()
        {
            if (isResetPressed)
            {
                OnResetButtonEvents();
                return;
            }

            if (isColorizedPressed)
            {
                OnColorizedButtonEvents();
                return;
            }

            OnColorStyleButtonEvents();
        }



        private void OnSetParsButtonEvents()
        {
            //Vector2 offset = new Vector2(offsetX, offsetY);
            cellSize = Mathf.FloorToInt(Mathf.Min(GRID_HEIGHT / numRows, GRID_WIDTH / numCols));
            pixelSizeToogleEnable = false; // DIKKAT DEĞİŞTR SONRA

            if (!pixelSizeToogleEnable)
            {
                Vector2 offset = new Vector2(pixelScale, pixelScale);

                if (!LevelEditorManager.Instance.isGridInit)
                {
                    LevelEditorManager.Instance.InitGrid(health,numRows, numCols, pixelScale, offset, levelBoardColor, ballHubColor);
                }
                else
                {
                    LevelEditorManager.Instance.DestroyAllPixels();
                    LevelEditorManager.Instance.Grid.SetGrid(health,numRows, numCols, pixelScale, offset, levelBoardColor, ballHubColor);
                    LevelEditorManager.Instance.Grid.InitGridCells(LevelEditorManager.Instance.StyleManager.EmptyCellStyle,
                        LevelEditorManager.Instance.StyleManager.UnColorCellStyle);
                }
            }
            else
            {



            }


            return;
            
        }

        private void OnResetButtonEvents()
        {
            LevelEditorManager.Instance.DestroyAllPixels();

            LevelEditorManager.Instance.Grid.SetGrid(
                 LevelEditorManager.DEFAULT_HEALTH,
                LevelEditorManager.DEFAULT_NUM_ROWS,
                LevelEditorManager.DEFAULT_NUM_COLS,
                LevelEditorManager.DEFAULT_PIXEL_SCALE,
                new Vector2(LevelEditorManager.DEFAULT_OFFSET_X, LevelEditorManager.DEFAULT_OFFSET_Y),
                LevelEditorManager.DEFAULT_MAIN_LEVEL_COLOR,
                LevelEditorManager.DEFAULT_MAIN_LEVEL_COLOR);

            LevelEditorManager.Instance.Grid.ObstacleParents = null;
            LevelEditorManager.Instance.Grid.InitGridCells(LevelEditorManager.Instance.StyleManager.EmptyCellStyle,
                LevelEditorManager.Instance.StyleManager.UnColorCellStyle);


            return;
        }


        private void OnPrintButtonEvents()
        {
            if (LevelEditorManager.Instance.Grid != null)
            {
                LevelEditorManager.Instance.Grid.LevelBoardColor = levelBoardColor;
                LevelEditorManager.Instance.Convert2Dto3D();
            }

            
            return;
        }

        private void OnColorizedButtonEvents()
        {
            LevelEditorManager.Instance.Grid.LevelBoardColor = levelBoardColor;

            Texture2DManager.FlatColorPatternArray(LevelEditorManager.Instance.StyleManager.ColorAssets, colors);

            for (int i = 0; i < colors.Length; i++)
            {
                LevelEditorManager.Instance.StyleManager.ColorAssets[i].color = colors[i];
            }

            return;
        }




        private void OnStyleButtonEvents()
        {
            for (int i = 0; i < isStylePressed.Length; i++)
            {
                if (isStylePressed[i])
                {
                    LevelEditorManager.Instance.StyleManager.ChooseSelectedStyle(i);
                    return;
                }

            }
        }


        private void OnColorStyleButtonEvents()
        {
            for (int i = 0; i < isColorStylePressed.Length; i++)
            {
                if (isColorStylePressed[i])
                {
                    LevelEditorManager.Instance.StyleManager.ChooseSelectedStyle(i);
                    return;
                }

            }
        }

        public void SetInputColorsOnLoad()
        {
            colors = LevelEditorManager.Instance.StyleManager.Colors;
        }


    }

    #endregion






}
#endif