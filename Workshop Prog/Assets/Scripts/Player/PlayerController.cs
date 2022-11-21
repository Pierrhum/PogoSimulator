using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float Life = 1.0f;
    private Animator Animator;
    private Camera Camera;
    private Rigidbody Rigidbody;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector2 inputVector;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Camera = GetComponentInChildren<Camera>();
        Rigidbody = GetComponent<Rigidbody>();

        pitch = Camera.transform.eulerAngles.x;
        yaw = Camera.transform.eulerAngles.y;
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>() * 5;
        Animator.SetBool("isRunning", context.performed);
        Animator.SetFloat("Left", inputVector.x);
    }

    private void Update()
    {
        Animator.SetFloat("Life", Life);

        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        Camera.transform.eulerAngles = new Vector3(Mathf.Clamp(pitch, -50,50), Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z);
        transform.eulerAngles = new Vector3(0, yaw, 0);
        Rigidbody.velocity = transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y));
        
    }
}
