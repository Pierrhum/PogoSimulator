using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PogoSpawner : MonoBehaviour
{
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
            GameObject go = Instantiate(PogoGuysPrefabs[Random.Range(0, PogoGuysPrefabs.Count)], transform.position + RandomPointOnCircleEdge(), Quaternion.identity, transform);
            yield return new WaitForSeconds(2f);
        }
    }
    
    private Vector3 RandomPointOnCircleEdge()
    {
        var vector2 = Random.insideUnitCircle.normalized * Radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
    
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
