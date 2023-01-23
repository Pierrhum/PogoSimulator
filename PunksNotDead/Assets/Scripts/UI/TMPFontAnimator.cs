using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPFontAnimator : MonoBehaviour
{
    private TextMeshProUGUI Text;
    private bool isAnimating = false;

    [SerializeField]
    [Range(-1, 0)]
    float m_FaceDilate = -0.5f;
    
    [SerializeField]
    [Range(0, 1)]
    float m_GlowPower = 0f;

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
        UpdateFontMaterialKeys();
        
    }

    private void UpdateFontMaterialKeys()
    {
        Text.font.material.SetFloat("_FaceDilate", m_FaceDilate);
        Text.font.material.SetFloat("_GlowPower", m_GlowPower);
    }

    public void StartAnimation()
    {
        isAnimating = true;
        StartCoroutine(AnimationCoroutine());
    }

    public void StopAnimation()
    {
        isAnimating = false;
    }

    private IEnumerator AnimationCoroutine()
    {
        while (isAnimating)
        {
            UpdateFontMaterialKeys();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
