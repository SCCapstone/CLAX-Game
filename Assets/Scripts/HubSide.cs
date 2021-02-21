﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubSide : MonoBehaviour
{
    public string sceneName;
    bool spawnEnabled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    { 
        //TODO add boss is dead logic
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}