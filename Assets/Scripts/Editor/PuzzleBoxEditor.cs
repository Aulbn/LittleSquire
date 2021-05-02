using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PuzzleBoxController))]
public class PuzzleBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("move positive x"))
        {
            (target as PuzzleBoxController).Move(Vector3.right, false);
        }

        if (GUILayout.Button("move negative x"))
        {
            (target as PuzzleBoxController).Move(Vector3.left, false);
        }

        if (GUILayout.Button("move Positive z"))
        {
            (target as PuzzleBoxController).Move(Vector3.forward, false);
        }

        if (GUILayout.Button("move negative z"))
        {
            (target as PuzzleBoxController).Move(Vector3.back, false);
        }
    }
}
