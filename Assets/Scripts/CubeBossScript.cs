using UnityEngine;
/**
 * Controls the movement and attack pattern of the Cube boss, holds all the functionality that requires
 */
public class CubeBossScript : AliveObject
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
    public AudioSource launchSound;

    int gridDimension = 3;
    int lastIdLaunched = 0;

    float lastCubeLaunchTime = 0;
    GameObject[] currentCubes;

    string side;
    string movementDirection;

    Vector3 currentTargetPos;

    enum BossPhase
    {
        SINGLE_SHOT,
        MOVING,
        ALL_SHOOT
    }

    BossPhase currentPhase = BossPhase.ALL_SHOOT;

    void Awake()
    {
        onDeath += OnDeath;
    }

    void Start()
    {
        Globals.boss = true;
    }


    /**
     * on boss death set globals
     */
    private void OnDestroy()
    {
        DestroyCubes();

        Globals.boss = false;
        Globals.cube = true;
    }


    /**
     * controls boss phases
     */
    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        if (target == null)
        {
            Setup();

            return;
        }

        switch (currentPhase)
        {
            case BossPhase.SINGLE_SHOT:
                MakeNewBatch();
                LaunchAsNeeded();

                break;
            case BossPhase.ALL_SHOOT:
                MakeNewBatch();
                LaunchRowAsNeeded();

                break;
            case BossPhase.MOVING:
                MoveOnAxis();

                break;
            default:
                break;
        }
    }
    /**
     * on boss death destroy all boss spawns
     */
    void OnDeath()
    {
        DestroyCubes();

        Destroy(gameObject, 1.0f);
    }

    /**
     * sets the player instance as the boss target
     */
    void Setup()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            //Debug.LogError("Recalced target");

            target = playerObject.transform;
        }

        lastCubeLaunchTime = 0;
    }

    // Calculate which side to place the cubes on
    string CalcSidePlayerOn()
    {
        Vector3 diff = target.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
        {
            return "x";
        }
        else
        {
            return "z";
        }
    }

    /**
     * spawns cubes in front of boss in 3X3 formation
     */
    void SpawnCubes()
    {
        //Vector3 midPoint = new Vector3((transform.position.x + target.position.x) / 2,
        //transform.position.y, (transform.position.z + target.position.z) / 2);

        currentCubes = new GameObject[gridDimension * gridDimension];

        side = CalcSidePlayerOn();

        Vector3 midPoint;

        if (side == "x")
        {
            if (transform.position.x > target.position.x)
            {
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss) * -1.0f;
            }
            else
            {
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss);
            }

            midPoint = transform.position + new Vector3(cubeDistFromBoss, 0.0f, 0.0f);
        }
        else
        {
            if (transform.position.z > target.position.z)
            {
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss) * -1.0f;
            }
            else
            {
                cubeDistFromBoss = Mathf.Abs(cubeDistFromBoss);
            }

            midPoint = transform.position + new Vector3(0.0f, 0.0f, cubeDistFromBoss);
        }

        int curNum = 0;

        for (int j = (gridDimension / 2); j >= -(gridDimension / 2); j--)
        {
            for (int i = (gridDimension / 2); i >= -(gridDimension / 2); i--)
            {
                GameObject newCube;

                // Spawn the grid of cubes on the x side
                if (side == "x")
                {
                    newCube = Instantiate(cubeAttacker,
                    new Vector3(transform.position.x + cubeDistFromBoss,
                    midPoint.y + j * distBetweenCubes, midPoint.z + i * distBetweenCubes),
                    new Quaternion());
                }
                // Spawn the grid of cubes on the z side
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

    // Destroy old cubes
    void DestroyCubes()
    {
        var lastCubes = GameObject.FindGameObjectsWithTag("EnemyAttack");

        for (int i = 0; i < lastCubes.Length; i++)
        {
            Destroy(lastCubes[i]);
        }
    }


    /**
     * launches the cubes till they are all gone
     * 
     */
    void LaunchAsNeeded()
    {
        // Launch the cubes one at a time
        float timeBetweenSingleCubeLaunch = (delayBetweenNewGrid * .65f) / (gridDimension * gridDimension);

        //Debug.Log("current cubes " + currentCubes);

        if (currentCubes != null && Time.time - lastCubeLaunchTime > timeBetweenSingleCubeLaunch && lastIdLaunched < currentCubes.Length)
        {
            if (currentCubes[lastIdLaunched] == null)
            {
                return;
            }

            currentCubes[lastIdLaunched].GetComponent<CubeAttackPlayer>().shouldMove = true;
            lastIdLaunched += 1;
            lastCubeLaunchTime = Time.time;
            delayBetweenNewGrid = 5;
            launchSound.Play();
        }
    }


    /**
     * launches a full row of cubes till they are all gone
     */
    void LaunchRowAsNeeded()
    {
        // Launch the cubes one at a time
        float timeBetweenSingleCubeLaunch = 1;

        //Debug.Log("current cubes " + currentCubes);

        if (currentCubes != null && Time.time - lastCubeLaunchTime > timeBetweenSingleCubeLaunch
            && lastIdLaunched < currentCubes.Length)
        {
            for (int i = 0; i < gridDimension; i++)
            {
                if (currentCubes != null)
                {
                    currentCubes[lastIdLaunched].GetComponent<CubeAttackPlayer>().shouldMove = true;
                    lastIdLaunched += 1;
                    lastCubeLaunchTime = Time.time;
                }
            }

            launchSound.Play();
        }

        delayBetweenNewGrid = 4;
    }


    /**
     * creates a new batch of 3X3 cubes
     */
    void MakeNewBatch()
    {
        if (Time.time - lastSpawnTime > delayBetweenNewGrid)
        {
            Debug.Log("Making new grid");

            DestroyCubes();
            batchesMade += 1;

            if (batchesMade == 3)
            {
                GoToNextPhase();
                batchesMade = 0;
            }
            else
            {
                SpawnCubes();
            }
        }
    }


    /**
     * Moves the cube boss to a new phase
     */
    void GoToNextPhase()
    {
        int totalPhases = System.Enum.GetNames(typeof(BossPhase)).Length;
        //currentPhase = (bossPhases)((((int)currentPhase) + 1) % totalPhases);
        currentPhase = (BossPhase)Random.Range(0, totalPhases);

        timesDestinationReached = 0;
    }


    void ComputeDestination()
    {
        if (timesDestinationReached >= 3)
        {
            GoToNextPhase();
            //timesDestinationReached = 1;

            return;
        }

        movementDirection = CalcSidePlayerOn();
        currentTargetPos = new Vector3(target.position.x, 0, target.position.z);
        timesDestinationReached += 1;
    }

    /**
     * moves the boss at the same height they spawned in on
     */
    void MoveOnAxis()
    {
        // Set the target at the beginning
        if (movementDirection == null || currentTargetPos == null)
        {
            timesDestinationReached -= 1;
            ComputeDestination();
        }

        //Debug.Log("movement direction " + movementDirection);

        // Move on different axis based on which one the player is farther away on
        if (movementDirection == "x")
        {
            moveAmount = Mathf.Abs(moveAmount) * (transform.position.x > currentTargetPos.x ? -1.0f : 1.0f);

            transform.position += new Vector3(moveAmount, 0, 0);

            if (Mathf.Abs(transform.position.x - currentTargetPos.x) < 0.5f)
            {
                ComputeDestination();
            }
        }
        else if (movementDirection == "z")
        {
            moveAmount = Mathf.Abs(moveAmount) * (transform.position.z > currentTargetPos.z ? -1.0f : 1.0f);

            transform.position += new Vector3(0, 0, moveAmount);

            if (Mathf.Abs(transform.position.z - currentTargetPos.z) < 0.5f)
            {
                ComputeDestination();
            }
        }
    }
}
