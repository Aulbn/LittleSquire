using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

enum ActivationType { OnAwake, OnTriggerBox}
enum TriggerLoadMode { loadScene, unloadScene, loadAndUnload};
public enum SceneAmmount { loadSingleScene, loadMultipleScenes };

public class SceneLoadTrigger : MonoBehaviour
{

    public delegate void OnEnterSceneBounds(string sceneString, string[] multiLoadList, bool isMultiMode);
    public static OnEnterSceneBounds onEnterSceneBound;
    public delegate void OnExitSceneBounds(string sceneString, string[] multiLoadList, bool isMultiMode);
    public static OnExitSceneBounds onExitSceneBound;
    public delegate void OnEnterNewSceneBounds(string sceneString, string[] multiLoadList, bool isMultiMode);
    public static OnEnterNewSceneBounds onEnterNewSceneBound;
    public delegate void MergeAwakeScene(string sceneString);
    public static MergeAwakeScene onMergeAwakeScene;


    [SerializeField]
    TriggerLoadMode loadMode;

    [SerializeField]
    SceneAmmount sceneAmmount;

    [SerializeField]
    ActivationType activationMode;

    [SerializeField]
    private string sceneToLoad = null;

    [SerializeField]
    string[] multipleScenesArray;
    bool isMultiLoad = false;

    [SerializeField]
    private bool isTransition = false;

    private BoxCollider triggerBox = null;



    void Awake()
    {
        if (activationMode == ActivationType.OnTriggerBox)
        {
            triggerBox = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            triggerBox.isTrigger = true;
            triggerBox.size = new Vector3(1, 1, 1);
        }
    }



    void Start()
    {
        if (activationMode == ActivationType.OnAwake)
        {
            onMergeAwakeScene.Invoke(sceneToLoad);
        }
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            if (loadMode == TriggerLoadMode.unloadScene)
                UnloadTheScene();
            else
                ActivationCheck();
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
                if (loadMode == TriggerLoadMode.loadAndUnload)
                    UnloadTheScene();
        }
    }

    public void ActivationCheck()
    {
        switch (sceneAmmount)
        {
            case SceneAmmount.loadSingleScene:
                isMultiLoad = false;
                LoadTheScene();
                break;
            case SceneAmmount.loadMultipleScenes:
                isMultiLoad = true;
                LoadTheScene();
                break;
            default:
                break;
        }
    }

   public void LoadTheScene()
    {
        switch (sceneAmmount)
            {
                case SceneAmmount.loadSingleScene:
                if (isTransition)
                    onEnterNewSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                else
                    onEnterSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                    break;
                case SceneAmmount.loadMultipleScenes:
                if (isTransition)
                    onEnterNewSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                else
                    onEnterSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                break;
                default:
                    break;
            }
    }

    void UnloadTheScene()
    {
        switch (sceneAmmount)
        {
            case SceneAmmount.loadSingleScene:
                isMultiLoad = false;
                onExitSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                break;
            case SceneAmmount.loadMultipleScenes:
                isMultiLoad = true;
                onExitSceneBound?.Invoke(sceneToLoad, multipleScenesArray, isMultiLoad);
                break;
            default:
                break;
        }
    }


    void OnDrawGizmosSelected()
    {
        if (activationMode == ActivationType.OnTriggerBox)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(1.0f, 0.1f, 0.0f, 0.4f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}