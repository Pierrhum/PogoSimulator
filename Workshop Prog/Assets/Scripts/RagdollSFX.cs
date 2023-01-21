using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RagdollSFX : MonoBehaviour
{
    public List<AudioClip> GroundImpact;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponentInParent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Terrain"))
            _source.PlayOneShot(GroundImpact[Random.Range(0, GroundImpact.Count)]);
    }
}
