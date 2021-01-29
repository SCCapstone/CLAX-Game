using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeBossScript : MonoBehaviour
{


    Transform target;
    float distBetween = 2;
    float distFromBoss = 5;

    public GameObject cubeAttacker;

    float spawnTimeCooldown = 0;
    float lastSpawnTime = 0;

    float delayBetweenRowLaunch = 1;
    float delayBetweenNewGrid = 7;


    int gridDimension = 3;
    int lastIdLaunched = 0;

    float lastCubeLaunchTime = 0;
    GameObject[] currentCubes;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        lastCubeLaunchTime = delayBetweenNewGrid;

    }

    void spawnCubes()
    {
        //Vector3 midPoint = new Vector3((transform.position.x + target.position.x) / 2,
        //    transform.position.y, (transform.position.z + target.position.z) / 2);

        currentCubes = new GameObject[gridDimension * gridDimension];


        string side;
        //calculate which side to place the cubes on
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


        int curNum = 0;
        for (int i = -(gridDimension / 2); i <= (gridDimension / 2); i++)
        {
            for (int j = -(gridDimension / 2); j <= (gridDimension / 2); j++)
            {
                GameObject newCube;
                //spawn the grid of cubes on the x side
                if (side == "x")
                {
                    newCube = Instantiate(cubeAttacker,
                    new Vector3(transform.position.x + distFromBoss,
                    midPoint.y + j * distBetween, midPoint.z + i * distBetween),
                    new Quaternion());
                }
                //spawn the grid of cubes on the z side
                else
                {

                    newCube = Instantiate(cubeAttacker,
                    new Vector3(midPoint.x + i * distBetween,
                    midPoint.y + j * distBetween, transform.position.z + distFromBoss),
                    new Quaternion());
                }
                currentCubes[curNum] = newCube;
                curNum += 1;
            }
        }
        lastSpawnTime = Time.time;
        lastIdLaunched = 0;


    }
    //destroy old cubes
    void destroyCubes()
    {
        var lastCubes = GameObject.FindGameObjectsWithTag("cubeAttack");
        for (int i = 0; i < lastCubes.Length; i++)
        {
            Destroy(lastCubes[i]);

        }
    }

    void launchRowByRow()
    {

        //for (int i = lastIdLaunched; i < lastIdLaunched + gridDimension; i++)
        //{

        //}
        //lastIdLaunched += gridDimension;
    }

    void launchAsNeeded()
    {
        //launch the cubes one at a time
        float timeBetweenSingleCubeLaunch = (delayBetweenNewGrid * .75f) / (gridDimension * gridDimension);

        if (Time.time - lastCubeLaunchTime > timeBetweenSingleCubeLaunch && lastIdLaunched < currentCubes.Length)
        {

            currentCubes[lastIdLaunched].GetComponent<cubeAttackPlayer>().shouldMove = true;
            lastIdLaunched += 1;
            lastCubeLaunchTime = Time.time;
        }
    }

    void makeNewBatch()
    {
        if (Time.time - lastSpawnTime > delayBetweenNewGrid)
        {
            destroyCubes();
            spawnCubes();
        }
    }

    // Update is called once per frame
    void Update()
    {
        makeNewBatch();
        launchAsNeeded();
    }
}