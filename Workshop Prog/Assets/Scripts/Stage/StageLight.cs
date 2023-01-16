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
    public bool PingPong = true;
    private bool ping = true;

    private void Awake()
    {
        Light = GetComponent<Light>();
        if(PingPong)
            Duration /= 2;
    }

    private void Start()
    {
        if(PingPong)
            StartCoroutine(PingPongCoroutine());
        else 
            StartCoroutine(LightCoroutine());
    }

    private IEnumerator LightCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            if (timer > Duration) timer = 0f;
            
            transform.eulerAngles = Vector3.Lerp(MinRotation, MaxRotation, timer / Duration);
            Light.color = Color.Lerp(MinColor, MaxColor, timer / Duration);
            
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator PingPongCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            if (timer > Duration)
            {
                timer = 0f;
                ping = !ping;
            }
            
            // In
            if (ping)
            {
                transform.eulerAngles = Vector3.Lerp(MinRotation, MaxRotation, timer / Duration);
                Light.color = Color.Lerp(MinColor, MaxColor, timer / Duration);
            }
            else
            {
                transform.eulerAngles = Vector3.Lerp(MaxRotation, MinRotation, timer / Duration);
                Light.color = Color.Lerp(MaxColor, MinColor, timer / Duration);
            }
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
    }
}
