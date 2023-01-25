using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecocup : MonoBehaviour
{
    public float HealAmount = 15f;
    private MeshFader Fader;
    private bool hasFallen = false;

    private void Awake()
    {
        Fader = GetComponent<MeshFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFallen && other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Heal(HealAmount);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasFallen && collision.gameObject.tag.Equals("Terrain"))
        {
            hasFallen = true;
            StartCoroutine(FadeOutCoroutine(5f));
        }
    }

    private IEnumerator FadeOutCoroutine(float FadeOutDuration)
    {
        yield return StartCoroutine(Fader.FadeOut(new List<Renderer>(){GetComponent<Renderer>()}, FadeOutDuration));
        Destroy(gameObject);
    }
}
