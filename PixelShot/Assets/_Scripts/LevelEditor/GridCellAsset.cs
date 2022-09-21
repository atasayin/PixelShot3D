using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "ScriptableObjects/GridCellAsset")]
public class GridCellAsset : ScriptableObject
{
    public IntAsset id;
    public string menuName;
    public Texture2D texture2D;
    public Color color;
}

#endif
