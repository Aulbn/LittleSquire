using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;


public class HideCrosshair : MonoBehaviour
{
    private Image crossHair = null;

    private float maxCrossHairAlpha = 0.0f;
    private float crosshairAlpha = 0.0f;

    Color alphaColor;

    [Range(1f, 10f)]
    [SerializeField] float fadeSpeed = 1f;

    [SerializeField] bool shouldStartHidden = false;

    void Awake()
    {
        crossHair = gameObject.GetComponent<Image>();
        maxCrossHairAlpha = crossHair.color.a;
        alphaColor = crossHair.color;
        crosshairAlpha = alphaColor.a;
        if (shouldStartHidden)
        {
            crosshairAlpha = 0.0f;
            alphaColor.a = crosshairAlpha;
            crossHair.color = alphaColor;
           // StartCoroutine(FadeInCrosshair());
        }
    }

    public void HideTheCrosshair()
    {
        if (gameObject.activeSelf == true)
        {
            // crossHair.gameObject.SetActive(false);
            StartCoroutine(FadeOutCrosshair());
        }
    }

    private IEnumerator FadeOutCrosshair()
    {
        while (crosshairAlpha > 0.0f)
        {
            crosshairAlpha -= Time.deltaTime * fadeSpeed;
            alphaColor.a = crosshairAlpha;
            crossHair.color = alphaColor;
            yield return null;
        }
    }


    public void UnHideCrosshair()
    {
        if (gameObject.activeSelf == true)
        {
            //crossHair.gameObject.SetActive(true);
            StartCoroutine(FadeInCrosshair());
        }
    }

    private IEnumerator FadeInCrosshair()
    {
        while (crosshairAlpha < maxCrossHairAlpha)
        {
            crosshairAlpha += Time.deltaTime * fadeSpeed;
            alphaColor.a = crosshairAlpha;
            crossHair.color = alphaColor;
            yield return null;
        }
    }

}
