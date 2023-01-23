using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCDancer : MonoBehaviour
{
    private Animator Animator;
    public int AnimNB = 5;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
       // Animator.SetInteger("Anim", Random.Range(0,AnimNB));
    }
}
