#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    [CustomEditor(typeof(AnimationManager))]
    [CanEditMultipleObjects]
    public class AnimationManagerEditor : Editor
    {
        public int direction = 1;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimationManager animationManager = (AnimationManager)target;

            EditorGUILayout.Space();
            if (GUILayout.Button("Inverse Rotation"))
            {
                direction *= -1;
            }

            if (GUILayout.Button("Rotate 360"))
            {
                animationManager.animationPoints[0] = new Vector3(0, direction * 360, 0);
            }

            if (GUILayout.Button("Rotate 180"))
            {
                animationManager.animationPoints[0] = new Vector3(0, direction * 180, 0);
            }

            if (GUILayout.Button("Rotate 90"))
            {
                animationManager.animationPoints[0] = new Vector3(0, direction * 90, 0);
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Build Object"))
            {
                animationManager.CreateAnimationParent();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Save Animations"))
            {
                LevelEditorManager.Instance.Grid.AnimationDatas = animationManager.animationDatas;
            }


        }
    }
}
#endif