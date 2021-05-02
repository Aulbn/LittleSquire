using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(SceneLoadTrigger))]
public class SceneLoaderEditor : Editor
{

    SerializedObject sceneLoadProp;
    SerializedProperty loadingType;
    SerializedProperty loadModeEnum;
    SerializedProperty currentEnum;
    SerializedProperty multiSceneArray;
    SerializedProperty singleSceneString;
    SerializedProperty sceneTransBool;

    private void OnEnable()
    {
        sceneLoadProp = serializedObject;
        loadingType = sceneLoadProp.FindProperty("activationMode");
        sceneTransBool = sceneLoadProp.FindProperty("isTransition");
        loadModeEnum = sceneLoadProp.FindProperty("loadMode");
        currentEnum = sceneLoadProp.FindProperty("sceneAmmount");
        multiSceneArray = sceneLoadProp.FindProperty("multipleScenesArray");
        singleSceneString = sceneLoadProp.FindProperty("sceneToLoad");
    }

    public override void OnInspectorGUI()
    {
        sceneLoadProp.Update();

        EditorGUILayout.PropertyField(loadingType);
        EditorGUILayout.Space(5);

        EditorGUILayout.PropertyField(loadModeEnum);
        EditorGUILayout.Space(5);

        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(currentEnum);

        EditorGUILayout.Space(5);

        if (currentEnum.enumValueIndex == 0)
            EditorGUILayout.PropertyField(singleSceneString);
        else if (currentEnum.enumValueIndex == 1)
            EditorGUILayout.PropertyField(multiSceneArray);

        if (loadingType.enumValueIndex == 1)
        {
            if (loadModeEnum.enumValueIndex == 0)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(sceneTransBool); 
            }
        }

        sceneLoadProp.ApplyModifiedProperties();
    }

}
