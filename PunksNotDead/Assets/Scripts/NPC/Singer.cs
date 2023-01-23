using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singer : MonoBehaviour
{
    private Vector3 InitPos;
    private Vector3 InitRot;
    private Animator Animator;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        InitPos = transform.position;
        InitRot = transform.eulerAngles;
    }

    public void EndState()
    {
        transform.position = InitPos;
        transform.eulerAngles = InitRot;
        Animator.SetTrigger("End");
        Animator.speed = 1f;
    }
}
