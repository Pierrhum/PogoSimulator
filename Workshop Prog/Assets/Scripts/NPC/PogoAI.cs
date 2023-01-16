using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PogoAI : MonoBehaviour
{
    public int Score = 10;
    public float Strength = 10;
    public float FadeOutDuration = 2f;
    private PogoSpawner spawner;
    private Animator Animator;
    private NavMeshAgent Agent;
    private float Speed = 3f;
    private bool alive = true;

    private List<SkinnedMeshRenderer> MeshesRenderer;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        MeshesRenderer = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        
        MeshesRenderer.ForEach(m => ChangeRenderMode(m.material));
    }
    
    private void ChangeRenderMode(Material material)
    {
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    public void StartRun(PogoSpawner _spawner)
    {
        spawner = _spawner;
        StartCoroutine(PogoCoroutine());
    }

    private IEnumerator PogoCoroutine()
    {
        Agent.destination = spawner.RandomPointOnCircle();
        while (alive)
        {
            if(!GameManager.instance.Freezed && Agent.remainingDistance <= 5f) Agent.destination = spawner.RandomPointOnCircle();
            else yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);
        float timer = 0f;
        
        while (timer < FadeOutDuration)
        {
            MeshesRenderer.ForEach(r => r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b,
                                                                    Mathf.Lerp(1f, 0f, timer/FadeOutDuration)));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameManager.instance.PogoGuys.Remove(this);
        Destroy(this);
    }

    public void EnableRagdoll()
    {
        GetComponent<Collider>().enabled = Animator.enabled = alive = false;
        Agent.isStopped = true;
    }
}
