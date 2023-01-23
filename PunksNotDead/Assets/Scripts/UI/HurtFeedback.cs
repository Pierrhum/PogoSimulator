using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HurtFeedback : MonoBehaviour
{

    private Image _Image;
    private UITransitionEffect _FadeIn;

    private void Awake()
    {
        _Image = GetComponent<Image>();
        _FadeIn = GetComponent<UITransitionEffect>();

        _FadeIn.effectFactor = 0;
    }

    private void Start()
    {
        transform.localPosition = new Vector3(Random.Range(-300f, 300f), Random.Range(-200f, 200f), 0f);
        transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-10f, 10f));
    }

    public void Pop()
    {
        StartCoroutine(PopCoroutine());
    }

    private IEnumerator PopCoroutine()
    {
        float timer = 0f;
        while (timer < .1f)
        {
            _FadeIn.effectFactor = Mathf.Lerp(0f, 0.8f, timer / .1f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        timer = 0f;
        Color BaseColor = _Image.color;
        while (timer < 2f)
        {
            _Image.color = new Color(BaseColor.r, BaseColor.g, BaseColor.b, Mathf.Lerp(BaseColor.a, 0f, timer / 2f));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }
}
