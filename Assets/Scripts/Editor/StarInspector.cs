using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star))]
public class StarInspector : Editor {
    private static GUIContent
        insertContent = new GUIContent("+", "duplicate this point"),
        deleteContent = new GUIContent("-", "delete this point"),
        pointContent = GUIContent.none;

    private static GUILayoutOption
        buttonWidth = GUILayout.MaxWidth(20f),
        colorWidth = GUILayout.MaxWidth(50f);

    private SerializedObject star;
    private SerializedProperty
        points,
        frequency,
        centerColor;

    void OnEnable()
    {
        star = new SerializedObject(target);
        points = star.FindProperty("points");
        frequency = star.FindProperty("frequency");
        centerColor = star.FindProperty("centerColor");
    }

    public override void OnInspectorGUI() {
        star.Update();

        GUILayout.Label("Points");
        for (int i = 0; i < points.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            SerializedProperty point = points.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(point.FindPropertyRelative("offset"), pointContent);
            EditorGUILayout.PropertyField(point.FindPropertyRelative("color"), pointContent, colorWidth);

            if (GUILayout.Button(insertContent, EditorStyles.miniButtonLeft, buttonWidth))
            {
                points.InsertArrayElementAtIndex(i);
            }
            if (GUILayout.Button(deleteContent, EditorStyles.miniButtonRight, buttonWidth))
            {
                points.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.PropertyField(frequency);
        EditorGUILayout.PropertyField(centerColor);

        star.ApplyModifiedProperties();
    }
}
