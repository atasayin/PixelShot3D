using UnityEngine;

#if UNITY_EDITOR
namespace LevelEditor 
{
    public class GridCell
    {
        private int _id;
        private int _row, _col;
        private CellType _type;
        private int _colorPalleteIndex;
        private GameObject _gameObject;
        private GridCell2DInfo _cell2D;

        public GridCell(int row, int col)
        {
            _row = row;
            _col = col;
            _type = CellType.Empty;
        }

        public GridCell(int row, int col, CellType type, int colorPalleteIndex)
        {
            _row = row;
            _col = col;
            _type = type;
            _colorPalleteIndex = colorPalleteIndex;
        }

        #region Getter Setter Function

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }


        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }


        public GameObject GameObject
        {
            get { return _gameObject; }
            set { _gameObject = value; }
        }

        public GridCell2DInfo Cell2D
        {
            get { return _cell2D; }
            set { _cell2D = value; }

        }

        public CellType Type
        {

            get { return _type; }
            set { _type = value; }

        }

        public int ColorPalleteIndex
        {
            get { return _colorPalleteIndex; }
            set { _colorPalleteIndex = value; }
        }

       
        #endregion


    }
}
#endif