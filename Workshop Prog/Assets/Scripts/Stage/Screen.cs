using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Screen : MonoBehaviour
{
    public float CameraSwapTime = 2f;
    public List<ScreenCamera> ScreenCameras;
    private MeshRenderer ScreenMesh;

    private void Awake()
    {
        ScreenMesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        foreach (ScreenCamera screenCamera in ScreenCameras)
        {
            screenCamera.associatedCamera.enabled = false;
        }
        StartCoroutine(CameraSwapCoroutine());
    }

    private IEnumerator CameraSwapCoroutine()
    {
        ScreenCameras[2].associatedCamera.enabled = true;
        yield return new WaitForSeconds(CameraSwapTime);
        ScreenCameras[2].associatedCamera.enabled = false;
        while (true)
        {
            ScreenCamera screenCamera = ScreenCameras[Random.Range(0, ScreenCameras.Count)];
            screenCamera.associatedCamera.enabled = true;
            ScreenMesh.material = screenCamera.material;
            
            yield return new WaitForSeconds(CameraSwapTime);
            screenCamera.associatedCamera.enabled = false;
        }
    }
}

[Serializable]
public struct ScreenCamera
{
    public Camera associatedCamera;
    public Material material;
}
