using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public GameObject PogoZoneColliders;
    private List<GameObject> dancers;
    private bool HideDancers = true;

    private void Awake()
    {
        
        dancers = new List<GameObject>();
        PogoZoneColliders.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HideDancers && other.GetComponent<NPCDancer>() != null)
        {
            if (!dancers.Contains(other.gameObject))
            {
                dancers.Add(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HideDancers && other.gameObject.GetComponent<PlayerController>() != null)
        {
            ShowPeople();
        }
    }

    public void HidePeople()
    {
        HideDancers = true;
        dancers.ForEach(d => d.SetActive(false));
        PogoZoneColliders.SetActive(false);
    }

    public void ShowPeople()
    {
        HideDancers = false;
        dancers.ForEach(d => d.SetActive(true));
        PogoZoneColliders.SetActive(true);
    }
}
