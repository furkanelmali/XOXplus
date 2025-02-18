using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AdjustCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AdjustCameraSize()
{
    Camera cam = Camera.main;
    float targetAspect = 16f / 9f; 
    float currentAspect = (float)Screen.width / Screen.height;

    if (currentAspect >= targetAspect)
    {
        cam.orthographicSize = 7f;
    }
    else
    {
        float difference = targetAspect / currentAspect;
        cam.orthographicSize = 2 * difference;
    }
}
}
