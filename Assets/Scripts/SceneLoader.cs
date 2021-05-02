using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour
{
    public enum LoadType
    {
        LoadAddAsync, UnloadAsync, LoadAdd
    }

    [Serializable]
    public class SceneLoadObject
    {
        public LoadType loadType;
        public string sceneName;
    }

    private void Awake()
    {
        if (onAwake)
            LoadLevelAction();
    }

    public bool onAwake;
    public List<SceneLoadObject> sceneLoadObjects = new List<SceneLoadObject>();

    public void LoadLevelAction()
    {
        foreach (SceneLoadObject loadObject in sceneLoadObjects)
        {
            switch (loadObject.loadType)
            {
                case LoadType.LoadAddAsync:
                    if (!SceneManager.GetSceneByName(loadObject.sceneName).isLoaded)
                    {
                        SceneManager.LoadSceneAsync(loadObject.sceneName, LoadSceneMode.Additive);
                        Debug.Log("Loading scene addative: " + loadObject.sceneName);
                    }
                    else
                        Debug.Log("Scene already loaded.");
                    break;

                case LoadType.UnloadAsync:
                    if (SceneManager.GetSceneByName(loadObject.sceneName).isLoaded)
                    {
                        SceneManager.UnloadSceneAsync(loadObject.sceneName);
                        Debug.Log("Is unloading scene: " + loadObject.sceneName);
                    }
                    else
                        Debug.Log("Scene already unloaded.");
                    break;
                case LoadType.LoadAdd:
                    if (!SceneManager.GetSceneByName(loadObject.sceneName).isLoaded)
                    {
                        SceneManager.LoadScene(loadObject.sceneName,LoadSceneMode.Additive);
                        Debug.Log("Loading scene: " + loadObject.sceneName);
                    }
                    else
                        Debug.Log("Scene already loaded.");
                    break;

                default:
                    break;
            }
        }
    }
}
