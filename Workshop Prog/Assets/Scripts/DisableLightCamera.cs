using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DisableLightCamera : MonoBehaviour
{
    public List<Light> Lights;

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera){
        if (GameManager.instance.Player.CanPlay && 
            camera.name == gameObject.GetComponent<Camera>().name)
        {
            foreach (Light light in Lights){
                light.enabled = false;
            }
        }
    }
    
    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera){
        if (GameManager.instance.Player.CanPlay && 
            camera.name == gameObject.GetComponent<Camera>().name)
        {
            foreach (Light light in Lights){
                light.enabled = true;
            }
        }
    }
}
