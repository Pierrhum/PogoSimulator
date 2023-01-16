using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    public UIShiny EnjmiaounShiny;
    public Image Overlay;

    private void Start()
    {
        StartCoroutine(ScreenCoroutine(1f, 1f));
    }

    private IEnumerator ScreenCoroutine(float ShinyDuration, float FadeDuration)
    {
        yield return new WaitForSeconds(1f);
        // FadeIn
        float timer = 0f;
        while (timer < FadeDuration)
        {
            Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b,
                Mathf.Lerp(1f, 0f, timer / FadeDuration));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        // Enjmiaoun Shiny
        timer = 0f;
        while (timer < ShinyDuration)
        {
            EnjmiaounShiny.effectFactor = timer / ShinyDuration;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        // FadeOut
        timer = 0f;
        while (timer < FadeDuration)
        {
            Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b,
                Mathf.Lerp(0f, 1f, timer / FadeDuration));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainLevel");
    }
}
