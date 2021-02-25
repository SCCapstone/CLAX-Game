using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeBossScript : MonoBehaviour
{


    Transform target;
    float distBetweenCubes = 2;
    float cubeDistFromBoss = 5;

    //float maxMoveAtATime = 10;
    //float movedSoFar = 0;
    float moveAmount = 0.15f;

    int timesDestinationReached = 0;
    int batchesMade = 0;


    public GameObject cubeAttacker;

    //float spawnTimeCooldown = 0;
    float lastSpawnTime = 0;

    //float delayBetweenRowLaunch = 1;
    public float delayBetweenNewGrid = 3;


    int gridDimension = 3;
    int lastIdLaunched = 0;

    float lastCubeLaunchTime = 0;
    GameObject[] currentCubes;


    string side;
    string movementDirection;

    Vector3 currentTargetPos;


    enum bossPhases
    {
        singleShot = 0, moving, allShoot
    }

    bossPhases currentPhase = bossPhases.allShoot;

    // Start is called before the first frame update
    void Start()
    {
        globals.boss = true;
    }

    void setup()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            //Debug.LogError("Recalced target");

            target = playerObject.transform;
        }
        lastCubeLaunchTime = 0;
    }

    string calcSidePlayerOn()
    {
        //calculate which side to place the cubes on
        if (Mathf.Abs(transform.position.x - target.position.x) >
            Mathf.Abs(transform.position.z - target.position.z))
            return "x";
        else
            return "z";
    }

    void spawnCubes()
    {
        //Vector3 midPoint = new Vector3((transform.position.x + target.position.x) / 2,
        //    transform.position.y, (transform.position.z + target.position.z) / 2);

        currentCubes = new GameObject[gridDimension * gridDimension];

        side = calcSidePlayerOn();
        Vector3 midPoint;
        if (side == "x")
        {
            if (transform.position.x > target.position.x)
                cubeDistFromBoss = -1 * Mathf.Abs(cubeDistFromBoss);
            else
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss);

            midPoint = new Vector3((transform.position.x + cubeDistFromBoss),
                transform.position.y, (transform.position.z));
        }
        else
        {
            if (transform.position.z > target.position.z)
                cubeDistFromBoss = -1 * Mathf.Abs(cubeDistFromBoss);
            else
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss);

            midPoint = new Vector3(transform.position.x,
            transform.position.y, (transform.position.z + cubeDistFromBoss));
        }


        int curNum = 0;
        for (int j = (gridDimension / 2); j >= -(gridDimension / 2); j--)
        {
            for (int i = (gridDimension / 2); i >= -(gridDimension / 2); i--)
            {
                GameObject newCube;
                //spawn the grid of cubes on the x side
                if (side == "x")
                {
                    newCube = Instantiate(cubeAttacker,
                    new Vector3(transform.position.x + cubeDistFromBoss,
                    midPoint.y + j * distBetweenCubes, midPoint.z + i * distBetweenCubes),
                    new Quaternion());
                }
                //spawn the grid of cubes on the z side
                else
                {

                    newCube = Instantiate(cubeAttacker,
                    new Vector3(midPoint.x + i * distBetweenCubes,
                    midPoint.y + j * distBetweenCubes, transform.position.z + cubeDistFromBoss),
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

    private void OnDestroy()
    {
        destroyCubes();
        globals.cube = true;
        globals.boss = false;
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
        float timeBetweenSingleCubeLaunch = (delayBetweenNewGrid * .65f) / (gridDimension * gridDimension);
        //Debug.Log("current cubes " + currentCubes);

        if (currentCubes != null && Time.time - lastCubeLaunchTime > timeBetweenSingleCubeLaunch && lastIdLaunched < currentCubes.Length)
        {

            currentCubes[lastIdLaunched].GetComponent<cubeAttackPlayer>().shouldMove = true;
            lastIdLaunched += 1;
            lastCubeLaunchTime = Time.time;
            delayBetweenNewGrid = 5;
        }

    }

    void launchRowAsNeeded()
    {
        //launch the cubes one at a time
        float timeBetweenSingleCubeLaunch = 1;
        //Debug.Log("current cubes " + currentCubes);
        if (currentCubes != null && Time.time - lastCubeLaunchTime > timeBetweenSingleCubeLaunch &&
            lastIdLaunched < currentCubes.Length)
        {
            for (int i = 0; i < gridDimension; i++)
            {

                currentCubes[lastIdLaunched].GetComponent<cubeAttackPlayer>().shouldMove = true;
                lastIdLaunched += 1;
                lastCubeLaunchTime = Time.time;
            }
        }
        delayBetweenNewGrid = 3;
    }

    void makeNewBatch()
    {

        if (Time.time - lastSpawnTime > delayBetweenNewGrid)
        {
            Debug.Log("making new grid");

            destroyCubes();
            batchesMade += 1;
            if (batchesMade == 3)
            {
                goToNextPhase();
                batchesMade = 0;
            }
            else
            {
                spawnCubes();

            }
        }
    }

    void goToNextPhase()
    {
        int totalPhases = System.Enum.GetNames(typeof(bossPhases)).Length;
        //currentPhase = (bossPhases)((((int)currentPhase) + 1) % totalPhases);
        currentPhase = (bossPhases)Random.Range(0, totalPhases - 1);

        timesDestinationReached = 0;

    }

    void computeDestination()
    {
        if (timesDestinationReached >= 3)
        {
            goToNextPhase();
            //timesDestinationReached = 1;
            return;
        }
        movementDirection = calcSidePlayerOn();
        currentTargetPos = new Vector3(target.position.x, 0, target.position.z);
        timesDestinationReached += 1;

    }

    void moveOnAxis()
    {
        //set the target at the beginning
        if (movementDirection == null || currentTargetPos == null)
        {
            timesDestinationReached -= 1;
            computeDestination();

        }

        //Debug.Log("movement direction " + movementDirection);

        //move on different axis based on which one the player is farther away on
        if (movementDirection == "x")
        {
            if (transform.position.x > currentTargetPos.x)
                moveAmount = -1 * Mathf.Abs(moveAmount);
            else
                moveAmount = Mathf.Abs(moveAmount);

            transform.position += new Vector3(moveAmount, 0, 0);
            if (Mathf.Abs(transform.position.x - currentTargetPos.x) < 0.5f)
                computeDestination();

        }
        else if (movementDirection == "z")
        {
            if (transform.position.z > currentTargetPos.z)
                moveAmount = -1 * Mathf.Abs(moveAmount);
            else
                moveAmount = Mathf.Abs(moveAmount);
            transform.position += new Vector3(0, 0, moveAmount);
            if (Mathf.Abs(transform.position.z - currentTargetPos.z) < 0.5f)
                computeDestination();

        }

    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            setup();
            return;
        }

        switch (currentPhase)
        {
            case bossPhases.singleShot:
                makeNewBatch();
                launchAsNeeded();
                break;
            case bossPhases.allShoot:
                makeNewBatch();
                launchRowAsNeeded();
                break;
            case bossPhases.moving:
                moveOnAxis();
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (target == null)
        //{
        //    setup();
        //    return;
        //}


    }
}