using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterBounds))]
public class CustomBoundsEditor : Editor
{
    private void OnSceneGUI()
    {
        CharacterBounds t = (CharacterBounds)target;
        Vector3 center = t.bounds.center;
        Vector3 min = t.bounds.min;
        Vector3 max = t.bounds.max;

        // Draw custom handles
        EditorGUI.BeginChangeCheck();
        min = Handles.FreeMoveHandle(min, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);
        max = Handles.FreeMoveHandle(max, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);
        center = Handles.PositionHandle(t.bounds.center, Quaternion.identity);
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Changed Bounds");
            t.bounds.min = min;
            t.bounds.max = max;
            t.bounds.center = center;
        }
    }
}