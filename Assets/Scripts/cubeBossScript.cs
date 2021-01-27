using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeBossScript : MonoBehaviour
{

    bool hasSpawnedCubes = false;

    Transform target;
    float distBetween = 2;
    float distFromBoss = 5;

    public GameObject cubeAttacker;

    float spawnTimeCooldown = 0;
    float lastSpawnTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void spawnCubes()
    {
        //Vector3 midPoint = new Vector3((transform.position.x + target.position.x) / 2,
        //    transform.position.y, (transform.position.z + target.position.z) / 2);

        var side = "";
        if (Mathf.Abs(transform.position.x - target.position.x) >
            Mathf.Abs(transform.position.z - target.position.z))
            side = "x";
        else
            side = "z";

        Vector3 midPoint = new Vector3();
        if (side == "x")
        {
            if (transform.position.x > target.position.x)
                distFromBoss = -1 * Mathf.Abs(distFromBoss);
            else
                distFromBoss = Mathf.Abs(distFromBoss);

            midPoint = new Vector3((transform.position.x + distFromBoss),
       transform.position.y, (transform.position.z));
        }
        else
        {
            if (transform.position.z > target.position.z)
                distFromBoss = -1 * Mathf.Abs(distFromBoss);
            else
                distFromBoss = Mathf.Abs(distFromBoss);

            midPoint = new Vector3(transform.position.x,
       transform.position.y, (transform.position.z + distFromBoss));
        }


        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //spawn the grid of cubes on the x side
                if (side == "x")
                {

                    Instantiate(cubeAttacker,
                    new Vector3(transform.position.x + distFromBoss,
                    midPoint.y + j * distBetween, midPoint.z + i * distBetween),
                    new Quaternion());
                }
                //spawn the grid of cubes on the z side
                else
                {

                    Instantiate(cubeAttacker,
                    new Vector3(midPoint.x + i * distBetween,
                    midPoint.y + j * distBetween, transform.position.z + distFromBoss),
                    new Quaternion());
                }
                //Instantiate(cubeAttacker,
                //    new Vector3(midPoint.x + i * 3, midPoint.y + 5, midPoint.z + j * 3),
                //    new Quaternion());
            }
        }
        hasSpawnedCubes = true;
        lastSpawnTime = Time.time;


    }

    void destroyCubes()
    {
        var lastCubes = GameObject.FindGameObjectsWithTag("cubeAttack");
        for (int i = 0; i < lastCubes.Length; i++)
        {
            Destroy(lastCubes[i]);

        }
    }

    void gridAttack()
    {
        if (Time.time - lastSpawnTime > 4)
        {
            destroyCubes();
            spawnCubes();
        }
    }

    // Update is called once per frame
    void Update()
    {
        gridAttack();
    }
}