using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform Tracked;

    private void FixedUpdate()
    {
        transform.LookAt(Tracked);
    }
}
