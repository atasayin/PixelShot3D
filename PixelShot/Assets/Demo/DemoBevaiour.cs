#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class DemoBevaiour : MonoBehaviour
{
 
    Mesh mainMesh;

    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        mainMesh = transform.GetComponent<MeshFilter>().mesh;

        transform.gameObject.SetActive(true);


        AssetDatabase.CreateAsset(mainMesh, "Assets/Meshes/"+gameObject.name+".asset");
        AssetDatabase.SaveAssets();

    }

}
#endif


