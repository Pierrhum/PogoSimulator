using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Screen : MonoBehaviour
{
    public float CameraSwapTime = 2f;
    public List<Material> CamerasMaterials;
    private MeshRenderer ScreenMesh;

    private void Awake()
    {
        ScreenMesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(CameraSwapCoroutine());
    }

    private IEnumerator CameraSwapCoroutine()
    {
        while (true)
        {
            ScreenMesh.material = CamerasMaterials[Random.Range(0, CamerasMaterials.Count)];
            yield return new WaitForSeconds(CameraSwapTime);
        }
    }
}
