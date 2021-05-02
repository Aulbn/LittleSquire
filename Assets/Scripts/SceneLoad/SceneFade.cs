using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{

    [SerializeField]
    [Range(0, 1)]
    private float fadeSpeed = 1.0f;
    private float fadeAlpha = 0.0f;

    private int fadeDirection = -1;
    private int drawDepth = -1000;

    private Texture2D fade2DTexture;

    [SerializeField]
    private Color theColorOfTheFade = Color.black;

    void Awake()
    {
        fade2DTexture = new Texture2D(1, 1, TextureFormat.ARGB4444, false);
        fade2DTexture.SetPixel(1, 1, Color.black);
        fade2DTexture.Apply();
    }

    void OnEnable()
    {
        SceneLoadManager.OnChangeScene += FadeOut;
        SceneLoadManager.OnReturnToScene += FadeIn;
        SceneLoadManager.OnRestartScene += FadeInInstant;
    }
    void OnDisable()
    {
        SceneLoadManager.OnChangeScene -= FadeOut;
        SceneLoadManager.OnReturnToScene -= FadeIn;
        SceneLoadManager.OnRestartScene -= FadeInInstant;
    }

    private void FadeIn()
    {
        fadeDirection = 1;
    }

    private void FadeOut()
    {
        fadeDirection = -1;
    }

    private void FadeInInstant()
    {
        fadeAlpha = 1;
        fadeDirection = 1;
    }

    private void OnGUI()
    {
        fadeAlpha += fadeDirection * fadeSpeed * Time.deltaTime;
        fadeAlpha = Mathf.Clamp01(fadeAlpha);

        GUI.color = new Color(theColorOfTheFade.r, theColorOfTheFade.g, theColorOfTheFade.b, fadeAlpha);
        GUI.depth = -999;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fade2DTexture);
    }
}
