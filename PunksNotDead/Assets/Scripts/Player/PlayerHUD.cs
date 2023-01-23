using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image GetUp;
    public GameObject KeySpam;
    public Camera FaceCamera;

    private void Awake()
    {
        GetUp.enabled = false;
        KeySpam.SetActive(false);
    }

    public void SetGetUp(float fill)
    {
        if (!GetUp.enabled) GetUp.enabled = true;
        GetUp.fillAmount = fill;

        if (fill >= 1f)
            StartCoroutine(FadeOut(GetUp, 1f));
    }


    private IEnumerator FadeOut(Image img, float duration)
    {
        KeySpam.SetActive(false);
        float timer = 0f;

        while (timer < duration)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f - timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetUp.enabled = false;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
    }
}
