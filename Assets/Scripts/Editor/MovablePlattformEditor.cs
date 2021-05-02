using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovablePlattform))]
public class MovablePlattformEditor : Editor
{
   
    private void OnSceneGUI()
    {
        MovablePlattform plattform = (MovablePlattform) target;
        Vector3 upPosition = plattform.upPosition;
        Vector3 downPosition = plattform.downPosition;
        
        
        Handles.color = default;
        EditorGUI.BeginChangeCheck();
        Vector3 newUpPosition = Handles.PositionHandle(upPosition, Quaternion.identity);
        Handles.Label(newUpPosition + Vector3.up, "Up Position");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(plattform, "Undo Up position");
            plattform.upPosition = newUpPosition;
        }
        EditorGUI.BeginChangeCheck();
        Vector3 newDownPosition = Handles.PositionHandle(downPosition, Quaternion.identity);
        Handles.Label(newDownPosition + Vector3.up, "Down Position");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(plattform, "Undo Down position");
            plattform.downPosition = newDownPosition;
        }
    }
}
