using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace LevelEditor
{
    public class StyleManager
    {

        private bool isGPUApply = false;

        private GUIStyle _emptyCellStyle;
        private GUIStyle _selectedCellStyle;
        private GUIStyle _unColoredCellStyle;
        private GUIStyle _obstacleCellStyle;
        private int _selectedColorIndex;
        private Color32[] _colors;
        private GUIStyle[] _styles, _colorStyles;
        private CellType _selectedType;
        private GridCellAsset[] _cellAssets, _colorAssets;
        private Dictionary<CellType, GUIStyle> _cellTypeToGUI;
        private GUIStyle[] _colorPalleteToGUI;


        public void Init()
        {
            LoadGridCellAssets();
            LoadGridColorAssets();
            CreateCellStyles();
            CreateColorStyles();
            CreateCellTypeToGUIMapping();

            if (isGPUApply)
            {
                Texture2DManager.SimplePixelPattern(_cellAssets[1].texture2D);
                Texture2DManager.FrozenPixelPattern(_cellAssets[2].texture2D);
                Texture2DManager.CircularColorPattern(_cellAssets[3].texture2D);
                Texture2DManager.TriangleColorPattern(_cellAssets[4].texture2D);
            }

        }


        private void LoadGridCellAssets()
        {
             _cellAssets = Utility.GridCellAssetArraySort(LevelLoader.LoadPixelCellAssets());
        }

        
        private void CreateCellStyles()
        {
            int lenght = _cellAssets.Length - 1;
            _styles = new GUIStyle[lenght];
            
            _emptyCellStyle = new GUIStyle();
            _emptyCellStyle.normal.background = _cellAssets[0].texture2D;

            _obstacleCellStyle = new GUIStyle();


            for (int i = 0; i < lenght; i++)
            {

                if (_cellAssets[i + 1].id.Value == 5)
                {
                    _obstacleCellStyle.normal.background = _cellAssets[i + 1].texture2D;
                }

                _styles[i] = new GUIStyle();
                _styles[i].normal.background = _cellAssets[i + 1].texture2D;

                
                
            }

            _selectedCellStyle = _styles[0];
            _selectedType = CellType.SimpleSquarePixel;

        }

        

        private void CreateCellTypeToGUIMapping()
        {
            _cellTypeToGUI = new Dictionary<CellType, GUIStyle>();

            _cellTypeToGUI.Add(CellType.Empty, _emptyCellStyle);

            for(int i = 0; i < (int)CellType.BitMapMaxLength - 1; i++)
            {
                _cellTypeToGUI.Add((CellType)(i+1), _styles[i]);
            }
            
        }

        public void ChooseSelectedStyle(int index)
        {
            if (LevelEditorManager.Instance.SELECTED_WINDOW == WindowType.GridEditorWindow)
            {
                _selectedCellStyle = _styles[index];
                _selectedType = (CellType)(++index);
            }
            else if (LevelEditorManager.Instance.SELECTED_WINDOW == WindowType.GridColor)
            {
                _selectedCellStyle = _colorStyles[index];
                _selectedColorIndex = index;
            }

        }

        #region Color Styles Functions

        private void LoadGridColorAssets()
        {
            _colorAssets = LevelLoader.LoadColorAssets();
        }

        private void CreateColorStyles()
        {
            int lenght = _colorAssets.Length;
            _colorStyles = new GUIStyle[lenght];

            for (int i = 0; i < lenght; i++)
            {
                _colorStyles[i] = new GUIStyle();
                _colorStyles[i].normal.background = _colorAssets[i].texture2D;
            }
            
            _unColoredCellStyle = _colorStyles[0];

        }

        public void ColorMenuSetColorSettings(Color32[] color)
        {
            for (int i = 0; i< _colorStyles.Length; i++)
            {
                color[i] = _colorAssets[i].color;
            }
        }

        public void CreateColorToGUIMapping()
        {
            int length = _colors.Length;
            _colorPalleteToGUI = new GUIStyle[length];
            
            for (int i = 0; i < length; i++)
            {
                _colorPalleteToGUI[i] =  _colorStyles[i];
                
            }
        }
     
        public int SelectedColorIndex
        {
            get { return _selectedColorIndex; }
            set { _selectedColorIndex = value; }

        }

           

        #endregion


        #region Getter Setter Functions

        public GUIStyle EmptyCellStyle
        {
            get { return _emptyCellStyle; }
            set { _emptyCellStyle = value; }
        }

        public GUIStyle SelectedCellStyle
        {
            get { return _selectedCellStyle; }
            set { _selectedCellStyle = value; }
        }

        public GUIStyle UnColorCellStyle
        {
            get { return _unColoredCellStyle; }
            set { _unColoredCellStyle = value; }
        }

        public GUIStyle ObstacleCellStyle
        {
            get { return _obstacleCellStyle; }
            set { _obstacleCellStyle = value; }
        }


        public Color32[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        public CellType SelectedCellType
        {
            get { return _selectedType; }
            set { _selectedType = value; }
        }

        public GUIStyle[] Styles
        {
            get { return _styles; }
            set { _styles = value; }
        }

        public GUIStyle[] ColorStyles
        {
            get { return _colorStyles; }
            set { _colorStyles = value; }
        }


        public GridCellAsset[] CellAssets
        {
            get { return _cellAssets; }
            set { _cellAssets = value; }
        }


        public GridCellAsset[] ColorAssets
        {
            get { return _colorAssets; }
            set { _colorAssets = value; }
        }

        public Dictionary<CellType, GUIStyle> CellTypeToGUI
        {
            get { return _cellTypeToGUI; }
            set { _cellTypeToGUI = value; }
        }

        public GUIStyle[] ColorPalleteIndexToGUI
        {
            get { return _colorPalleteToGUI; }
            set { _colorPalleteToGUI = value; }
        }

        #endregion

    }
}
#endif