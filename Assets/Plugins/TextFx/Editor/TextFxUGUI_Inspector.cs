#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace TextFx
{
	[CustomEditor(typeof(TextFxUGUI))]
	public class TextFxUGUI_Inspector : UnityEditor.UI.TextEditor
	{
		public override void OnInspectorGUI ()
		{
            TextFxUGUI uguiEffect = (TextFxUGUI) target;

            base.OnInspectorGUI ();

			GUILayout.Space(10);

            GUILayout.Label("Mesh Effect", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            bool guiChanged = GUI.changed;

            uguiEffect.m_effect_type = (TextFxUGUI.UGUI_MESH_EFFECT_TYPE)EditorGUILayout.EnumPopup("Type", uguiEffect.m_effect_type);

            GUI.enabled = uguiEffect.m_effect_type != TextFxUGUI.UGUI_MESH_EFFECT_TYPE.None;

            uguiEffect.m_effect_offset = EditorGUILayout.Vector2Field("Offset", uguiEffect.m_effect_offset);

            uguiEffect.m_effect_colour = EditorGUILayout.ColorField("Colour", uguiEffect.m_effect_colour);

            if (!guiChanged && GUI.changed)
            {
                uguiEffect.ForceUpdateGeometry();
            }

            GUI.enabled = true;
            EditorGUI.indentLevel--;

            GUILayout.Space(10);

            GUILayout.Label ("TextFx", EditorStyles.boldLabel);

			if (GUILayout.Button("Open Animation Editor", GUILayout.Width(150)))
			{
				TextEffectsManager.Init();
			}
		}
	}
}
#endif