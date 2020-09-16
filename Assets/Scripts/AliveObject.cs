using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveObject : MonoBehaviour
{
    //main class for anything that should have health
    public float health = 500;
    float lastHealth = 500;
    public float maxHitCooldown = .5f;
    public float hitCooldown = .5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("reduceHitCooldown", .01f, .1f);
        lastHealth = health;

    }
    void reduceHitCooldown()
    {
        hitCooldown = Mathf.Max(0, hitCooldown - .1f);
    }

    void CheckHealth()
    {
        //mainly for debugging to know each time this object was hit
        if (lastHealth != health)
        {
            Debug.Log("New health for " + transform.name + ": " + health);
            lastHealth = health;
            //hitCooldown = maxHitCooldown;
        }
        if (health <= 0)
        {
            //TODO
            //Show death of some kind
        }
    }

    //should be called by things trying to damage this object
    public void takeDamage(float amount)
    {
        //give some invincible frames between hits
        if (hitCooldown <= 0)
        {
            health -= amount;
            hitCooldown = maxHitCooldown;
        }

    }


    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
}
