using UnityEngine;
using UnityEditor;
using System;

public class Texture2DManager
{
    private static Color32 orange = new Color32(255, 165, 0, 255);
    private  Color32 oranges = new Color32(255, 165, 0, 255);
    private static Color32 teal = new Color32(0, 128, 128, 255);
    private static Color32 iceBlue = new Color32(190,220, 220, 255);
    private static Color32 white = new Color32(255, 255, 255, 255);
    private static Color32 black = new Color32(0, 0, 0,0);
    private static Color32 pastelYellow = new Color32(233, 236, 107, 255);
    private static Color32 comptoPastelYellow = new Color32(161, 56, 153, 255);
    public static Color32 purble = new Color32(174, 31, 193, 255);

    #region Pixel Patterns

    public static void SimplePixelPattern(Texture2D texture)
    {
        InnerSquareColorPattern(texture);
    }


    public static void FrozenPixelPattern(Texture2D texture)
    {
        var data = texture.GetRawTextureData<Color32>();

        int index = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                data[index++] = ((x + y) >= texture.width / 2 ? white : black);
            }
        }
        // upload to the GPU
        texture.Apply();
    }

   

    #endregion

    public static void InnerSquareColorPattern(Texture2D texture)
    {
        var data = texture.GetRawTextureData<Color32>();

        int index = 0;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                bool condition = (x == 0) || (y == 0) || (x == texture.width - 1) || (y == texture.height - 1);


                data[index++] = (condition ? white : black);
            }
        }
        // upload to the GPU
        texture.Apply();
    }

    public static void CircularColorPattern(Texture2D texture)
    {
        var data = texture.GetRawTextureData<Color32>();

        int index = 0;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                bool condition = ((x == 0) || (x == (texture.width - 1))) && ((y == 1) || (y == 2)) ||
                    ((y == 0) || (y == (texture.height - 1))) && ((x == 1) || (x == 2));


                condition = !( (y == 0) && ((x == 0) || (x == (texture.width - 1))) ||
                    (y == texture.height - 1) && ((x == 0) || (x == (texture.width - 1))));

                data[index++] = (condition ? white : black);
            }
        }
        // upload to the GPU
        texture.Apply();
    }



    public static void TriangleColorPattern(Texture2D texture)
    {
        var data = texture.GetRawTextureData<Color32>();

        int index = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                data[index++] = ((x & y) == 0 ? white : black);
            }
        }
        // upload to the GPU
        texture.Apply();
    }
#if UNITY_EDITOR

    public static void FlatColorPatternArray(GridCellAsset[] gridCellAssets, Color32[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            
            FlatColorPattern(gridCellAssets[i].texture2D, colors[i]);
        }

    }
#endif

    public static void FlatColorPattern(Texture2D texture, Color32 color)
    {
        var data = texture.GetRawTextureData<Color32>();

        int index = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                data[index++] = color;
            }
        }
        // upload to the GPU
        texture.Apply();
    }

}

