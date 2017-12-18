using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DependedByWindow : EditorWindow
{
    static Object selectedObject;
    static Vector2 scrollPosition = Vector2.zero;
    static bool dirty = false;

    [MenuItem("Assets/Select Depended by", false, 1000)]
    static void SelectDependedBy()
    {
        GetWindow<DependedByWindow>(false, "DependedBy", true);
        selectedObject = Selection.activeObject;
        dirty = true;
        Debug.Log("selecting object path = " + AssetDatabase.GetAssetPath(selectedObject));
    }

    void OnGUI()
    {
        //if (dirty)
        {
            dirty = false;
            GUILayoutOption[] tmp = null;
            EditorGUILayout.BeginScrollView(scrollPosition, tmp);
            string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(selectedObject));
            Object itemObj = null;
            for (int i = 0; i < paths.Length; i++)
            {
                //Debug.Log("dep[" + i + "]=" + paths[i]);
                itemObj = AssetDatabase.LoadMainAssetAtPath(paths[i]);
                EditorGUILayout.ObjectField(itemObj, itemObj.GetType(), true, null);
            }

            EditorGUILayout.EndScrollView();
        }
    }

    static void FindDepend()
    {
        string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(selectedObject));
        for (int i = 0; i < paths.Length; i++)
        {
            Debug.Log("dep[" + i + "]=" + paths[i]);
            if (i == 1)
            {
                //UnityEditor.EditorApplication.
                //AssetDatabase.ImportAsset(paths[i]);
                //UnityEditor.ProjectWindowUtil.CreateAsset()
                //UnityEditor.SearchableEditorWindow.
            }
        }
    }
}
