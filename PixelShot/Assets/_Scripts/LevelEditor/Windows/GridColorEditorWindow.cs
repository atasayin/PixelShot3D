using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class GridColorEditorWindow : EditorWindow
    {
        #region Variables

        private const float BACKGRID_START_WIDTH_COF = 0.3f;
        private const float BACKGRID_START_HEIGHT_COF = 0.01f;
        private bool isEditorOpenWithPlaying = false;

        private int paramaterMenuBarWidth;

        private GridEditorInputManager inputManager;

        private Vector2 backGridStart, gridStart, scrollPosition;
        private Rect menuBar;

        #endregion


        [MenuItem("Window/Grid Color Editor Window")]
        private static void OpenWindow()
        {
            GridColorEditorWindow window = (GridColorEditorWindow)GetWindow(typeof(GridColorEditorWindow));
            window.titleContent = new GUIContent("Grid Color Editor Window");
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

                inputManager.OnMouseEvents(Event.current, gridStart);

                inputManager.OnColorButtonEvents();

                DrawStyleCellMenu();

                DrawGrid();

                if (GUI.changed)
                {
                    Repaint();
                }

            }

            

        }

        private void OnFocus()
        {
            
            LevelEditorManager.Instance.ChooseGridColorWindow();
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
            
        }

        private void DrawParametersMenu()
        {
            
            GUILayout.Label("Color Settings", EditorStyles.boldLabel);
            GUILayout.Label("Mouse Area Radius: ", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.levelBoardColor = EditorGUILayout.ColorField("Level Board Color: ", inputManager.levelBoardColor, GUILayout.Width(paramaterMenuBarWidth));
            inputManager.ballHubColor = EditorGUILayout.ColorField("Ball Hub Color: ", inputManager.ballHubColor, GUILayout.Width(paramaterMenuBarWidth));
            inputManager.mouseAreaRadius = EditorGUILayout.IntSlider(inputManager.mouseAreaRadius, 0, 3, GUILayout.Width(paramaterMenuBarWidth));

            for (int i = 0; i < inputManager.colors.Length; i++)
            {
                inputManager.colors[i] = EditorGUILayout.ColorField("Set Color " + (i + 1) +": ", inputManager.colors[i], GUILayout.Width(paramaterMenuBarWidth));
            }

            inputManager.isResetPressed = GUILayout.Button("Reset", GUILayout.Width(paramaterMenuBarWidth));
            inputManager.isColorizedPressed = GUILayout.Button("Colorized", GUILayout.Width(paramaterMenuBarWidth));

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

            for (int i = 0; i < LevelEditorManager.Instance.StyleManager.ColorAssets.Length; i++)
            {

                inputManager.isColorStylePressed[i] = GUILayout.Button(
                    new GUIContent(LevelEditorManager.Instance.StyleManager.ColorAssets[i].menuName),
                    EditorStyles.toolbarButton,
                    GUILayout.Width(paramaterMenuBarWidth)
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
                LevelEditorManager.Instance.Grid.DrawColoredGrid(gridStart);
            }

        }

        
        

        #endregion


    }
}
#endif