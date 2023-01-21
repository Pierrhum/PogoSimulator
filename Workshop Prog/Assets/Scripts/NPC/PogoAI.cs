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

    [SerializeField] private List<MaterialSwitcher> MaterialSwitchers;
    private List<SkinnedMeshRenderer> MeshesRenderer;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        MeshesRenderer = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();

    }
    
    private void ChangeRenderMode(SkinnedMeshRenderer mesh)
    {
        var materialsCopy = mesh.materials;
        for (int index = 0; index < mesh.materials.Length; index++)
        {
            Material material = mesh.materials[index];
            
            // Switch to transparent
            Material transparent = MaterialSwitchers.Find(m => (m.Opaque.name + " (Instance)").Equals(material.name)).Transparent;
            transparent.SetFloat("_Mode", 2);
            transparent.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparent.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparent.SetInt("_ZWrite", 0);
            transparent.DisableKeyword("_ALPHATEST_ON");
            transparent.EnableKeyword("_ALPHABLEND_ON");
            transparent.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparent.renderQueue = 3000;
            
            materialsCopy[index] = transparent;
        }

        mesh.materials = materialsCopy;
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

        foreach (SkinnedMeshRenderer mesh in MeshesRenderer)
            ChangeRenderMode(mesh);

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
        Destroy(gameObject);
    }

    public void EnableRagdoll()
    {
        GetComponent<Collider>().enabled = Animator.enabled = alive = false;
        Agent.isStopped = true;
    }

    [Serializable]
    private struct MaterialSwitcher
    {
        public Material Opaque;
        public Material Transparent;
    }
}
