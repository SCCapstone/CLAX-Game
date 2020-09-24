﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePlayerOnTouch : MonoBehaviour
{
    public float damageAmount = 10;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("other tag" + other.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            var enemy = other.gameObject.GetComponent<AliveObject>();
            enemy.takeDamage(damageAmount);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}