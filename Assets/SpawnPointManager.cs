using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnPointManager : MonoBehaviour
{
    public SpawnPoint defaultSpawn;
    public SpawnPoint currentSpawn;

    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    void Start()
    {
        foreach (GameObject o in FindObjectsOfType<GameObject>())
        {
            if (!o.activeInHierarchy)
            {
                continue;
            }

            SpawnPoint sp = o.GetComponent<SpawnPoint>();

            if (sp != null)
            {
                spawnPoints.Add(sp);

                sp.OnPlayerEnter.AddListener(delegate
                {
                    SetSpawn(sp);
                });

                if (Globals.desiredSpawnName == sp.spawnName)
                {
                    currentSpawn = sp;
                }
            }
        }

        if (defaultSpawn == null)
        {
            Assert.IsTrue(spawnPoints.Count > 0, "No spawn locations found!");

            defaultSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        }
        
        if (currentSpawn == null)
        {
            currentSpawn = defaultSpawn;
        }

        currentSpawn.SetActiveSilent(true);
        currentSpawn.SpawnPlayer();
    }

    private void SetSpawn(SpawnPoint sp)
    {
        if (sp == null)
        {
            return;
        }

        foreach (SpawnPoint other in spawnPoints)
        {
            if (other == sp)
            {
                continue;
            }

            other.SetActiveSilent(false);
        }

        sp.SetActive(true);
        currentSpawn = sp;

        Globals.desiredSpawnName = sp.name;
    }
}
