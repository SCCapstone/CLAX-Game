using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spawnPoint;
    public GameObject player;
    
    bool respawn = false;
    // Start is called before the first frame update
    void Start()
    {
        player = (GameObject)Instantiate(player, Vector3.zero, Quaternion.identity);
        player.transform.position = spawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collision has occured");
        }
    }
}
