using UnityEditor;
using UnityEngine;


public class LevelLoader
{
#if UNITY_EDITOR
    private const string LEVEL_PATH = "Assets/Levels/Level";


    

    public static Level LoadLevel(int levelNumber)
    {
        return (Level)AssetDatabase.LoadAssetAtPath(LEVEL_PATH + levelNumber + ".asset", typeof(Level));

    }


    public static void SaveLevel(LevelEditor.GridEditor grid, int levelNumber)
    {
        Level level = grid.GridToLevel();

        level.levelNumber = levelNumber;
        AssetDatabase.CreateAsset(level, LEVEL_PATH + levelNumber + ".asset");
        AssetDatabase.SaveAssets();
    }






    public static GridCellAsset[] LoadPixelCellAssets()
    {
        Object[] objects = Resources.LoadAll("EditorCells", typeof(GridCellAsset));
        GridCellAsset[] assets = new GridCellAsset[objects.Length];

        for (int i = 0; i < assets.Length; i++)
        {
            assets[i] = (GridCellAsset)objects[i];
        }

        

        return assets;
    }


    public static GridCellAsset[] LoadColorAssets()
    {
        Object[] objects = Resources.LoadAll("EditorColorCells", typeof(GridCellAsset));
        GridCellAsset[] assets = new GridCellAsset[objects.Length];
        
        for (int i = 0; i < assets.Length; i++)
        {
            assets[i] = (GridCellAsset)objects[i];
        }



        return assets;
    }

#endif











}
