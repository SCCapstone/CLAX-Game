using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveObject : MonoBehaviour
{
    //main class for anything that should have health
    public float health = 500;
    public float lastHealth = 500;

    // Start is called before the first frame update
    void Start()
    {

    }

    void CheckHealth()
    {
        //mainly for debugging to know each time this object was hit
        if (lastHealth != health)
        {
            Debug.Log("New health for " + transform.name + ": " + health);
            lastHealth = health;
        }
        if (health <= 0)
        {
            //TODO
            //Show death of some kind
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
}
