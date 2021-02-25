using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private bool active = false;
    public GameObject doorway;
    public Material change;

    private Renderer doorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        doorRenderer = doorway.GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(globals.won && !active)
        {
            doorRenderer.material = change;
            active = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && active)
        {
            Debug.Log("you win");
        }
    }
}
