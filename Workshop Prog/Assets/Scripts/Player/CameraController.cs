using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum CameraType
{
    PLAYER,
    RAGDOLL
};
public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera BaseCamera;
    public CinemachineVirtualCamera RagdollCamera;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void SetCamera(CameraType _type)
    {
        switch (_type)
        {
            case CameraType.PLAYER:
                BaseCamera.Priority = 10;
                RagdollCamera.Priority = 9;
                break;
            case CameraType.RAGDOLL:
                BaseCamera.Priority = 9;
                RagdollCamera.Priority = 10;
                RagdollCamera.transform.position = BaseCamera.transform.position;
                break;
            default:
                break;
        }
    }
}
