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
    public PlayerController Player;
    public CinemachineVirtualCamera BaseCamera;
    public CinemachineVirtualCamera RagdollCamera;
    public Transform Aim;
    
    private Camera _camera;
    private float SensitivityX = 2f;
    private float SensitivityY = 0.1f;
    private float yaw = 0f;
    private float pitch = 0f;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();        
        pitch = _camera.transform.eulerAngles.x;
        yaw = _camera.transform.eulerAngles.y;
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

    public void UseCameraInputs()
    {
        // Mouse input
        yaw += Input.GetAxis("Mouse X") * SensitivityX;
        pitch += Input.GetAxis("Mouse Y") * SensitivityY;
        pitch = Mathf.Clamp(pitch, 1.8f, 2.3f);
        // Player Transform direction according to yaw
        Player.transform.eulerAngles = new Vector3(0, yaw, 0);
        // Camera aim according to pitch
        Aim.position = new Vector3(Aim.position.x, pitch, Aim.position.z);
    }
}
