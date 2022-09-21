using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class GridEditorWindow : EditorWindow
    {
        #region Variables

        private const float BACKGRID_START_WIDTH_COF = 0.3f;
        private const float BACKGRID_START_HEIGHT_COF = 0.01f;
        private bool isEditorOpenWithPlaying = false;

        private int paramaterMenuBarWidth, styleMenuBarWidth;
     
        private Rect menuBar;

        private GridEditorInputManager inputManager;

        private Vector2 backGridStart, gridStart, scrollPosition;


        #endregion


        [MenuItem("Window/Grid Editor Window")]
        private static void OpenWindow()
        {
            GridEditorWindow window = (GridEditorWindow)GetWindow(typeof(GridEditorWindow));
            window.titleContent = new GUIContent("Grid Editor Window");
            window.Show();
        }


        private void OnEnable()
        {
            if (EditorApplication.isPlaying)
            {
                inputManager = new GridEditorInputManager();

                InitUIPositions();
            }
            else
            {
                isEditorOpenWithPlaying = true;
            }
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (isEditorOpenWithPlaying)
                {
                    OnEnable();
                    isEditorOpenWithPlaying = false;
                }

                DrawParametersMenu();

                UpdateUIPositions();

                DrawBackgroundGrid();

                DrawStyleCellMenu();

                inputManager.OnMouseEvents(Event.current, gridStart);

                inputManager.OnButtonEvents();

                DrawGrid();

                if (GUI.changed)
                {
                    Repaint();
                }

            }

        }
        private void OnFocus()
        {
            LevelEditorManager.Instance.ChooseGridEditorWindow();
        }


        private void UpdateUIPositions()
        {
            backGridStart.x = position.width * BACKGRID_START_WIDTH_COF;
            backGridStart.y = position.height * BACKGRID_START_HEIGHT_COF;
            paramaterMenuBarWidth = (int)(backGridStart.x * 0.9f);

            if (LevelEditorManager.Instance.isGridInit)
            {

                GridEditor grid = LevelEditorManager.Instance.Grid;
                gridStart.x = ((position.width - backGridStart.x) / 2 + backGridStart.x) - (grid.CellSize2D * grid.NumCols / 2);
                gridStart.y = (position.height / 2 - (grid.CellSize2D * grid.NumRows / 2) > 0) ?
                    position.height / 2 - (grid.CellSize2D * grid.NumRows / 2) :
                    35;
            }

        }


        #region Draw Helper Functions

        private void InitUIPositions()
        {
            backGridStart = new Vector2(position.width, 0);
            gridStart = new Vector2(position.width, 0);
            styleMenuBarWidth = 200;
        }

        private void DrawParametersMenu()
        {

            GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
            inputManager.levelTypes = (LevelTypes)EditorGUILayout.EnumPopup("Level Type: ", inputManager.levelTypes, GUILayout.Width(paramaterMenuBarWidth));
            inputManager.health = EditorGUILayout.IntField("Health: ", inputManager.health, GUILayout.Width(paramaterMenuBarWidth));
            inputManager.numRows = EditorGUILayout.IntField("Number of Rows: ", inputManager.numRows, GUILayout.Width(paramaterMenuBarWidth));
            inputManager.numCols = EditorGUILayout.IntField("Number of Columns: ", inputManager.numCols, GUILayout.Width(paramaterMenuBarWidth));
            GUILayout.Label("Mouse Area Radius: ", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.mouseAreaRadius = EditorGUILayout.IntSlider(inputManager.mouseAreaRadius, 0, 3, GUILayout.Width(paramaterMenuBarWidth));

            GUILayout.Label("Obstacle Settings", EditorStyles.boldLabel);
            inputManager.obstacleCreateType = (ObstacleCreation) EditorGUILayout.EnumPopup("Obstacle Group Type", inputManager.obstacleCreateType, GUILayout.Width(paramaterMenuBarWidth));
            int obstacleLenghtUpperBound;

            if (inputManager.obstacleCreateType == ObstacleCreation.Horizontal)
            {
                obstacleLenghtUpperBound = inputManager.numCols;
            }
            else
            {
                obstacleLenghtUpperBound = inputManager.numRows;
            }
            GUILayout.Label("Obstacle Anchor : ", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.obstacleAnchor = EditorGUILayout.Slider(inputManager.obstacleAnchor, 0, obstacleLenghtUpperBound, GUILayout.Width(paramaterMenuBarWidth));
            GUILayout.Label("Obstacle Length : ", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.obstacleLength = EditorGUILayout.IntSlider(inputManager.obstacleLength, 0, obstacleLenghtUpperBound, GUILayout.Width(paramaterMenuBarWidth));

            inputManager.isApplyPressed = GUILayout.Button("Set Parameters", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.isResetPressed = GUILayout.Button("Reset", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.isPrintPressed = GUILayout.Button("Print", GUILayout.Width(paramaterMenuBarWidth));

        }

        private void DrawBackgroundGrid()
        {
           
            int backgroundCellSize = LevelEditorManager.DEFAULT_CELL_SIZE / 2;
            int numBackWidth = Mathf.CeilToInt(position.width / backgroundCellSize);
            int numBackHeight = Mathf.CeilToInt(position.height / backgroundCellSize);

            Vector3 backGridOffset = new Vector3(backGridStart.x, 0, 0);
            Handles.BeginGUI();

            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
            for (int i = 0; i < numBackWidth + 1; i++)
            {
                Handles.DrawLine(new Vector3(backgroundCellSize * i, 0, 0) + backGridOffset,
                    new Vector3(backgroundCellSize * i, position.height, 0) + backGridOffset);
            }


            for (int i = 0; i < numBackHeight + 1; i++)
            {
                Handles.DrawLine(new Vector3(0, backgroundCellSize * i, 0) + backGridOffset,
                    new Vector3(position.width, backgroundCellSize * i, 0) + backGridOffset);
            }

            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(0, 0, 0) + backGridOffset,
                    new Vector3(0, position.height, 0) + backGridOffset);

            Handles.EndGUI();

        }
        
        private void DrawStyleCellMenu()
        {
            menuBar = new Rect(backGridStart, new Vector2(position.width - backGridStart.x, 30));

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginHorizontal();
           
            for (int i = 0; i < LevelEditorManager.Instance.StyleManager.Styles.Length; i++)
            {
   
                inputManager.isStylePressed[i] = GUILayout.Button(
                    new GUIContent(LevelEditorManager.Instance.StyleManager.CellAssets[i + LevelEditorManager.EMPTY_ASSET_SHIFT].menuName),
                    EditorStyles.toolbarButton,
                    GUILayout.Width(styleMenuBarWidth)
                    );
            }
        
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }


        private void DrawGrid()
        {

            if (LevelEditorManager.Instance.isGridInit)
            {
                LevelEditorManager.Instance.Grid.DrawGrid(gridStart);
            }

        }


        #endregion

        
    }
}
#endif