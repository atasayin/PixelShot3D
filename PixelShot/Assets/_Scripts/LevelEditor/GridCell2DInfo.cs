using System;
using UnityEngine;
public class GridCell2DInfo
{

    private Vector2 _position2D, _size;
    private GUIStyle _style;
    private GUIStyle _coloredStyle;

    public GridCell2DInfo(Vector2 position2D, Vector2 size, GUIStyle defaultStyle)
    {
        _position2D = position2D;
        _size = size;
        _style = defaultStyle;
        _coloredStyle = defaultStyle;
    }

    public GridCell2DInfo(Vector2 position2D, Vector2 size, GUIStyle defaultStyle, GUIStyle coloredStyle)
    {
        _position2D = position2D;
        _size = size;
        _style = defaultStyle;
        _coloredStyle = coloredStyle;
    }


    public void Draw(Vector2 gridStart)
    {
        GUI.Box(new Rect(_position2D + gridStart, _size), "", _style);
    }

    public void DrawColored(Vector2 gridStart)
    {
        GUI.Box(new Rect(_position2D + gridStart, _size), "", _coloredStyle);
    }


    public Vector2 Position2D
    {
        get { return _position2D; }
        set { _position2D = value; }
    }


    public GUIStyle Style
    {

        get { return _style; }
        set { _style = value; }

    }

    public GUIStyle ColorStyle
    {
        get { return _coloredStyle; }
        set { _coloredStyle = value; }

    }
}
