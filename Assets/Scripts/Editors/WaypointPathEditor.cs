using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointPath))]
public class WaypointPathEditor : Editor
{
    private bool showHandles = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button($"{(showHandles ? "Hide" : "Show")} Handles"))
        {
            showHandles = !showHandles;

            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        if (!showHandles)
        {
            return;
        }

        WaypointPath path = (WaypointPath)target;

        for (int i = 0; i < path.ControlPointsCount; i++)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 point = path.GetControlPoint(i);

            Vector3 newPoint = Handles.PositionHandle(point, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Change point position");

                path.SetControlPoint(i, newPoint);
            }
        }
    }
}
