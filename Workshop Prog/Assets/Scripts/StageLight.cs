using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLight : MonoBehaviour
{
    private Light Light;
    
    public Vector3 MinRotation;
    public Vector3 MaxRotation;

    public Color MinColor;
    public Color MaxColor;

    public float Duration = 2f;

    private void Awake()
    {
        Light = GetComponent<Light>();
    }

    private void Start()
    {
        StartCoroutine(LightCoroutine());
    }

    private IEnumerator LightCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            transform.eulerAngles = Vector3.Lerp(MinRotation, MaxRotation, (timer%Duration) / Duration);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
