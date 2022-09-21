using System;
using UnityEngine;

public class Utility
{
    public static Vector3 VectorNormalZ = new Vector3(0, 0, 1);

    public static void CreateMainColorGradient(Gradient gradient, Color mainColor, float alphaStart, float alphaEnd)
    {
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        colorKey = gradient.colorKeys;
        colorKey[0].color = mainColor;
        colorKey[0].time = 0.0f;
        colorKey[1].color = mainColor;
        colorKey[1].time = 1.0f;

        alphaKey = gradient.alphaKeys;
        alphaKey[0].alpha = alphaStart;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = alphaEnd;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

    }

    public static int GetRandomInt(int min, int max)
    {
        return Mathf.RoundToInt(UnityEngine.Random.Range(min, max));
    }

    public static Vector3 GetRandomVector3(float min, float max)
    {
        return new Vector3(
            UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max));
    }

    public static Vector3 Vector2Dto3D(Vector2 v2)
    {
        return new Vector3(v2.x, 0, v2.y);
    }

    public static LayerMask CreateLayerMask(string layer)
    {
        LayerMask collisionMask = new LayerMask();
        int ballLayer = LayerMask.NameToLayer(layer);
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(ballLayer, i))
            {
                collisionMask |= 1 << i;
            }
        }

        return collisionMask;
    }

    public static float FloatRounder(float discretePrecesition, float input)
    {
        float k = input / discretePrecesition;

        return discretePrecesition * Mathf.Round(k);


    }


#if UNITY_EDITOR
    public static GridCellAsset[] GridCellAssetArraySort(GridCellAsset[] assets)
    {
        int length = assets.Length;
        GridCellAsset[] sorted = new GridCellAsset[length];

        for (int i = 0; i < length; i++)
        {
            GridCellAsset temp = null;
            for (int j = 0; j < length; j++)
            {
                if (assets[j].id.Value == i)
                {
                    temp = assets[j];
                }
            }

            sorted[i] = temp;
        }

        return sorted;
    }
#endif

    /*
    public static void SortArray<Comparable>(ref Comparable[] arr)
    {
        int n = arr.length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j].compareTo(arr[j + 1])) > 0 {
                    T temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }*/
    // C# implementation of QuickSort





}
