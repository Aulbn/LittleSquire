using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PuzzleLeverController))]
public class PuzzleLeverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Pull"))
        {
            (target as PuzzleLeverController).Pull();
        }
    }
}
