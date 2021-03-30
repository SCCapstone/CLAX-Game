using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spawnPoint;
    public GameObject player;
    private Renderer spawnPointRenderer;
    private Material original;
    public Material change;

    public int spawnNum;

    public bool canSetSpawn;

    public AudioSource activationSound;

    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        spawnPointRenderer = spawnPoint.GetComponentInChildren<Renderer>();
        original = spawnPointRenderer.material;
        if (spawnNum == globals.spawnPoint)
        {
            player = (GameObject)Instantiate(player, Vector3.zero, Quaternion.identity);
            player.transform.position = spawnPoint.transform.position;
            active = true;
            spawnPointRenderer.material = change;
        }
        else
        {
            active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnNum != globals.spawnPoint && active)
        {
            active = false;
            spawnPointRenderer.material = original;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canSetSpawn)
        {
            globals.spawnPoint = spawnNum;
            if (spawnPointRenderer.material != change)
            {
                spawnPointRenderer.material = change;
                activationSound.Play();

            }
        }
    }
}
