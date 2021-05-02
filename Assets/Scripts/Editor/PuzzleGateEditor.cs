using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PuzzleGateController))]
public class PuzzleGateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Open Gate"))
        {
            (target as PuzzleGateController).OpenGate();
        }
        if (GUILayout.Button("Close Gate"))
        {
            (target as PuzzleGateController).CloseGate();
        }
    }
}
