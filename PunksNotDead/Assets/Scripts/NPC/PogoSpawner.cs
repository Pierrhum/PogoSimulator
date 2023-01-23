using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PogoSpawner : MonoBehaviour
{
    public int MaxAI = 30;
    public List<GameObject> PogoGuysPrefabs;
    public float Radius = 5f;

    private void Start()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    private IEnumerator SpawnerCoroutine()
    {
        while (true)
        {
            if (!GameManager.instance.Freezed && GameManager.instance.PogoGuys.Count < MaxAI)
            {
                AddPogoGuy();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void AddPogoGuy()
    {
        GameObject go = Instantiate(PogoGuysPrefabs[Random.Range(0, PogoGuysPrefabs.Count)], transform.position + RandomPointOnCircleEdge(), Quaternion.identity, transform);
        go.GetComponent<PogoAI>().StartRun(this);
        GameManager.instance.PogoGuys.Add(go.GetComponent<PogoAI>());
    }
    
    public Vector3 RandomPointOnCircleEdge()
    {
        var vector2 = Random.insideUnitCircle.normalized * Radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
    
    public Vector3 RandomPointOnCircle()
    {
        var vector2 = Random.insideUnitCircle * Radius;
        return transform.position + new Vector3(vector2.x, 0, vector2.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
