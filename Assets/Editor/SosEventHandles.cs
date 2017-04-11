using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SosHandle))]

public class SosEventHandles : Editor
{

    Vector3[] positions;

    void OnSceneGUI()
    {
        float width = HandleUtility.GetHandleSize(Vector3.zero) * 0.5f;
        SosHandle evtAttr = (SosHandle)target;
        //Handles.BeginGUI();
        Handles.color = new Color(1, 1, 0, .7f);
        //Handles.DrawSolidArc(evtAttr.transform.position, evtAttr.transform.up,
        //    -evtAttr.transform.right, 360, evtAttr.shieldArea);
        //Handles.DrawWireDisc(evtAttr.transform.position, Vector3.up, evtAttr.shieldArea);
        //Handles.DrawLine(Vector3.zero, new Vector3(100, 0, 0));
        //Handles.CircleCap(0, evtAttr.transform.position + new Vector3(5, 0, 0), evtAttr.transform.rotation, 1);
        //Handles.EndGUI();
        //Handles.SphereCap(0, evtAttr.transform.position, evtAttr.transform.localRotation, 5);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(evtAttr);
        }

        Vector2 min = new Vector2(200, 200);
        Vector2 max = new Vector2(300, 300);
        Rect rect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        Handles.BeginGUI();
        //GUI.Box(rect, rect.ToString());
        Handles.EndGUI();
        //char[] cc = "adfaafdsa";
        //string aaa = new string(cc);
    }
}