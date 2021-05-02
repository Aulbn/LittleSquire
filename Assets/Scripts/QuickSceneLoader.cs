using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class QuickSceneLoader : MonoBehaviour
{
    public GameObject loadScreen;
    public Image loadingBar;
    public bool isLoading;

    [Header("Scenes")]
    public SceneLoadObject[] scenes;

    [Serializable]
    public struct SceneLoadObject
    {
        public string[] sceneName;
    }

    private Keyboard kb;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        kb = InputSystem.GetDevice<Keyboard>();
        loadScreen.SetActive(false);
    }

    private void Update()
    {
        if (isLoading)
            return;

        if (kb.numpad1Key.wasPressedThisFrame)
            LoadLevel(0);
        if (kb.numpad2Key.wasPressedThisFrame)
            LoadLevel(1);
        if (kb.numpad3Key.wasPressedThisFrame)
            LoadLevel(2);
        if (kb.numpad4Key.wasPressedThisFrame)
            LoadLevel(3);
        if (kb.numpad5Key.wasPressedThisFrame)
            LoadLevel(4);
    }

    private void LoadLevel(int index)
    {
        Debug.Log("Load level: " + (index + 1));
        loadScreen.SetActive(true);
        isLoading = true;
        StartCoroutine(LoadScenesAsync(scenes[index].sceneName));
    }

    private IEnumerator LoadScenesAsync(string[] scenes)
    {
        bool isDone;
        float progress;
        AsyncOperation[] operations = new AsyncOperation[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            operations[i] = SceneManager.LoadSceneAsync(scenes[i], i == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);
        }

        do {
            progress = 0;
            isDone = true;

            foreach (AsyncOperation operation in operations)
            {
                if (!operation.isDone)
                {
                    isDone = false;
                }
                progress += Mathf.Clamp01(operation.progress / .9f / scenes.Length);
            }

            loadingBar.fillAmount = progress;
            yield return null;

        } while (!isDone);

        loadScreen.SetActive(false);
        isLoading = false;
        Debug.Log("Done Loading");
    }

}
