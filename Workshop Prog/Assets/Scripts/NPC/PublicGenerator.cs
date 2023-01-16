using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PublicGenerator : MonoBehaviour
{
    public TerrainCollider TerrainCollider;
    public int Nombre = 50;
    public List<GameObject> PublicPrefabs;
    public GameObject Surface;
    public PogoSpawner SpawnerSurface;


    private void Start()
    {
        for (int i = 0; i < Nombre; i++)
        {
            GameObject go = Instantiate(PublicPrefabs[Random.Range(0, PublicPrefabs.Count)], GetRandomPoint(), Quaternion.identity);
            isCapsuleColliding(go);
            go.transform.parent = transform;
            
            GameManager.instance.Public.Add(go.GetComponent<NPCDancer>());
        }
    }

    private bool isCapsuleColliding(GameObject go)
    {
        CapsuleCollider col = go.GetComponent<CapsuleCollider>();
        Vector3 direction = new Vector3 {[col.direction] = 1};
        float offset = col.height / 2 - col.radius;
        Vector3 localPoint0 = col.center - direction * offset;
        Vector3 localPoint1 = col.center + direction * offset;
        
        Collider[] hitColliders = Physics.OverlapCapsule(localPoint0, localPoint1, col.radius);
        Debug.Log(hitColliders.Length);
        return true;
    }

    private Vector3 GetRandomPointInPlane()
    {
        List<Vector3> VerticeList = new List<Vector3>(Surface.GetComponent<MeshFilter>().sharedMesh.vertices);
        
        Vector3 leftTop = Surface.transform.TransformPoint(VerticeList[0]);
        Vector3 rightTop = Surface.transform.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = Surface.transform.TransformPoint(VerticeList[110]);
        
        Vector3 XAxis = rightTop - leftTop;
        Vector3 ZAxis = leftBottom - leftTop;
        
        return leftTop + XAxis * Random.value + ZAxis * Random.value;
    }
    
    private Vector3 GetRandomPoint()
    {
        Vector3 point = GetRandomPointInPlane();

        if (SpawnerSurface != null) 
            while (Terrain.activeTerrain.SampleHeight(point) > 0 ||
                Vector3.Distance(point, SpawnerSurface.transform.position) < SpawnerSurface.Radius)
                point = GetRandomPointInPlane();
        
        return point;
    }
}
