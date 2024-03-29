﻿using UnityEngine;

/*
 * Controls the movement and attacks for the rolling boss (pill boss)
 */
public class RollingBossScript : AliveObject
{
    float lastExpandedTime;
    float expandInverseFrequency = .03f;
    float maxYSize = 4.0f;
    float normalSize;
    float expansionAmount = 1.25f;
    float distanceToPlayerForAttack = 8.0f;
    float rollPower = 40.0f;
    float rotateAmount = 140.0f;


    int currentPhaseNum = 0;

    public GameObject wallPrefab;
    GameObject madeWall;
    Rigidbody body;


    bool isGrowing = true;

    public Transform goalTransform;
    float startMovingTime;
    float rollTime = 2;

    bool hasStartedMoveAndWall = false;

    float phaseTimeCooldown = 0;
    float cooldownUpdateTime = .5f;

    bool isDoneWithCurrentPhase = true;
    bool hasDoneOneTimePhaseCode = false;

    private float startY;


    enum bossPhases
    {
        moveAndMakeWall = 0, spinAttackPhase
    }

    bossPhases currentPhase = bossPhases.moveAndMakeWall;


    // Start is called before the first frame update
    void Start()
    {
        Globals.boss = true;
        //scale = gameObject.transform.localScale;
        lastExpandedTime = Time.time;
        body = GetComponent<Rigidbody>();
        normalSize = transform.localScale.y;

        InvokeRepeating("Cooldowns", 0.5f, cooldownUpdateTime);

        Invoke("FindPlayer", 0.5f);

        startY = transform.position.y;


    }

    void Awake()
    {
        onDeath += OnDeath;
    }

    void FindPlayer()
    {
        if (goalTransform == null)
            goalTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnDestroy()
    {
        if (madeWall)
        {
            Destroy(madeWall);
        }
        Globals.pill = true;
        Globals.boss = false;
    }
    /**
     * controls the boss behavior
     */
    void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        if (goalTransform == null)
            return;
        ChoosePhase();

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    void OnDeath()
    {
        if (madeWall)
        {
            Destroy(madeWall);
        }

        Destroy(gameObject, 1.0f);
    }

    void Cooldowns()
    {
        phaseTimeCooldown = Mathf.Max(phaseTimeCooldown - cooldownUpdateTime, 0);

    }
    /**
     * Controls the size of the pill boss
     */
    bool ChangeSize()
    {
        if (isGrowing)
            expansionAmount = Mathf.Abs(expansionAmount);
        else
            expansionAmount = -1 * Mathf.Abs(expansionAmount);

        if (Time.time - lastExpandedTime >= expandInverseFrequency)
        {

            if ((isGrowing && transform.localScale.y + expansionAmount < maxYSize) ||
                (isGrowing == false && transform.localScale.y + expansionAmount > normalSize))
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + expansionAmount,
                    transform.localScale.z);
                lastExpandedTime = Time.time;
                //ball successfully changed size
                return true;
            }
            if (isGrowing == false)
            {
                hasStartedMoveAndWall = false;
                //lastExpandedTime = Time.time;

            }

            if (transform.localScale.y + expansionAmount >= maxYSize)
            {
                //the ball is already the correct size
                return false;
            }

        }
        //did not attempt to change size
        return true;
    }
    /*
     * make a wall after target is reached
     */
    void BallExpandAndMakeWall(bool lockSize)
    {
        if (madeWall != null && madeWall.GetComponent<ExpandingWallScript>().isDone)
        {
            Destroy(madeWall);
            isGrowing = false;
        }
        bool changedSize = ChangeSize();
        if (changedSize == false)
        {
            //if the ball has expanded to the correct size, then make the new wall
            if (madeWall == null && body.velocity == Vector3.zero)
            {
                Debug.Log("Made wall");

                MakeWall(lockSize);
            }
        }

    }

    void MakeWall(bool lockSize)
    {
        madeWall = Instantiate(wallPrefab, this.transform.position,
                    this.transform.rotation);
        madeWall.transform.Rotate(new Vector3(90, 90, 0));
        madeWall.GetComponent<ExpandingWallScript>().lockWallSize = lockSize;
    }

    void MoveTowardPoint(float powerMuliplier = 1)
    {
        //have to do this the long way since we want to keep the current y rotation
        transform.LookAt(new Vector3(goalTransform.position.x, transform.position.y, goalTransform.position.z),
            new Vector3(1, 0, 0));

        Vector3 distance = (goalTransform.position - this.transform.position) * powerMuliplier * rollPower;


        body.AddForce(distance);
        //body.velocity = (goalTransform.position - this.transform.position);
        //Debug.Log("moved " + distance);

    }
    /*
     * stop moving when target is reached
     */
    void StopMomentum()
    {
        if (Time.time - startMovingTime > rollTime)
        {
            body.velocity = Vector3.zero;

        }
    }

    void SpinAttack()
    {

        //debug code when only testing the spin attack phase
        //if (isDoneWithCurrentPhase)
        //{
        //    isDoneWithCurrentPhase = false;
        //    hasDoneOneTimePhaseCode = false;
        //    isGrowing = true;
        //}

        //purposely have this if statement twice
        if (hasDoneOneTimePhaseCode == false)
        {
            Debug.Log("before rotation " + transform.eulerAngles);
            //transform.eulerAngles =
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, 0);
            Debug.Log("after rotation " + transform.eulerAngles);

        }

        BallExpandAndMakeWall(true);

        if (hasDoneOneTimePhaseCode == false)
        {
            if (madeWall != null)
            {
                madeWall.GetComponent<ExpandingWallScript>().lockWallSize = true;
                Debug.Log("locked wall size");

                hasDoneOneTimePhaseCode = true;
                phaseTimeCooldown = 15f;
                madeWall.transform.eulerAngles =
                    new Vector3(madeWall.transform.eulerAngles.x, madeWall.transform.eulerAngles.y, 0);
                madeWall.transform.position = transform.position;


            }
            return;
        }

        var tempRotation = rotateAmount;
        var bossHealthPercent = GetComponentInParent<AliveObject>().health / GetComponentInParent<AliveObject>().maxHealth;
        //if the boss is in phase 2 in terms of health, turn slower
        if (bossHealthPercent > .33f && bossHealthPercent < .67f)
        {
            rotateAmount /= 3;
        }

        transform.Rotate(new Vector3(0, 0, rotateAmount * Time.fixedDeltaTime));
        if (madeWall != null)
        {
            madeWall.transform.Rotate(0, -rotateAmount * Time.fixedDeltaTime, 0);
            madeWall.GetComponent<ExpandingWallScript>().lockWallSize = true;
            //madeWall.transform.position = transform.position;
        }

        rotateAmount = tempRotation;
        //Debug.Log("phase cooldown " + phaseTimeCooldown);

        if (phaseTimeCooldown <= 0)
        {
            if (madeWall != null)
            {
                madeWall.GetComponent<ExpandingWallScript>().lockWallSize = false;
                //Debug.Break();

                //Debug.Log("set to false");
                //Debug.Log("is done: " + madeWall.GetComponent<expandingWallScript>().isDone);

                if (madeWall.GetComponent<ExpandingWallScript>().isDone)
                {

                    Destroy(madeWall);
                    isDoneWithCurrentPhase = true;
                }
            }

        }
        //mad.GetComponent<Transform>().Rotate(new Vector3(0, 0, 2));

    }

    void MoveAndMakeWallAttack()
    {
        if (hasDoneOneTimePhaseCode == false)
        {
            hasDoneOneTimePhaseCode = true;
            phaseTimeCooldown = 10f;
            //have to do this the long way since we want to keep the current y rotation
            transform.LookAt(new Vector3(goalTransform.position.x, transform.position.y, goalTransform.position.z),
            new Vector3(1, 0, 0));

        }




        if (Vector3.Distance(transform.position, goalTransform.position) < distanceToPlayerForAttack)
        {
            body.velocity = Vector3.zero;
            isGrowing = true;
        }

        if (hasStartedMoveAndWall == false)
        {
            MoveTowardPoint();
            startMovingTime = Time.time;
            hasStartedMoveAndWall = true;
            //isGrowing = true;
            lastExpandedTime = Time.time;
        }
        else
        {
            StopMomentum();
            //isGrowing = true;
            BallExpandAndMakeWall(false);
        }

        if (phaseTimeCooldown <= 0)
        {
            if (madeWall != null)
            {

                if (madeWall.GetComponent<ExpandingWallScript>().isDone)
                {

                    Destroy(madeWall);
                    isDoneWithCurrentPhase = true;
                }
            }

        }

    }

    void ChoosePhase()
    {
        if (phaseTimeCooldown <= 0 && isDoneWithCurrentPhase)
        {
            //get the total number of phases the boss can have
            int totalPhases = System.Enum.GetNames(typeof(bossPhases)).Length;
            currentPhase = (bossPhases)(currentPhaseNum % totalPhases);

            currentPhaseNum++;
            Debug.Log("Changed phase");
            isDoneWithCurrentPhase = false;
            isGrowing = true;

            hasDoneOneTimePhaseCode = false;

        }
        //Debug.Log("current phase " + currentPhase.ToString());
        //Debug.Log("phase cooldown " + phaseTimeCooldown);

        switch (currentPhase)
        {
            case bossPhases.moveAndMakeWall:
                MoveAndMakeWallAttack();
                break;
            case bossPhases.spinAttackPhase:
                SpinAttack();

                break;
            default:
                break;
        }
    }
}
