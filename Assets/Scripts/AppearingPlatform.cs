using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingPlatform : MonoBehaviour
{
    public GameObject platform;

    private Renderer platformRenderer;
    private BoxCollider platformCollider;

    // Start is called before the first frame update
    void Start()
    {
        platformRenderer = platform.GetComponentInChildren<Renderer>();
        platformCollider = platform.GetComponentInChildren<BoxCollider>();
        if (!globals.won)
        {
            platformRenderer.enabled = false;
            platformCollider.enabled = false;
        }

    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(globals.won && !platformRenderer.enabled)
        {
            platformRenderer.enabled = true;
            platformCollider.enabled = true;
        }
        
    }
    */
}
