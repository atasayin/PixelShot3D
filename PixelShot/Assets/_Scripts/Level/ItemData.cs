using UnityEngine;


[System.Serializable]
public class ItemData
{
    public Vector3 _position;

    public Vector3 _scale;

    public int id;

    public int _colorPalatteIndex;

    public CellType _type;

    public Quaternion _rotation;

    public int _row, _col;

    public ItemData(Transform transform, int row, int col, CellType type, int colorPalatteIndex)
    {

        _position = transform.position;
        _scale = transform.localScale;
        _rotation = transform.rotation;
        _row = row;
        _col = col;
        _type = type;
        _colorPalatteIndex = colorPalatteIndex;

    }

    public ItemData(Transform transform, int row, int col, CellType type)
    {

        _position = transform.position;
        _scale = transform.localScale;
        _rotation = transform.rotation;
        _row = row;
        _col = col;
        _type = type;

    }


    #region Getter Setter Functions

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

    #endregion



}
