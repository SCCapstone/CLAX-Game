using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingBossScript : MonoBehaviour
{

    //Vector3 scale;
    float lastExpandedTime;
    float expandFrequency = .05f;
    float maxYSize = 4.0f;
    float normalSize;
    float expansionAmount = .15f;
    float distanceToPlayerForAttack = 8.0f;
    float rollPower = 40.0f;

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


    enum bossPhases
    {
        moveAndMakeWall = 0, spinAttackPhase
    }

    bossPhases currentPhase = bossPhases.moveAndMakeWall;


    // Start is called before the first frame update
    void Start()
    {
        //scale = gameObject.transform.localScale;
        lastExpandedTime = Time.time;
        body = GetComponent<Rigidbody>();
        normalSize = transform.localScale.y;

        InvokeRepeating("cooldowns", .5f, cooldownUpdateTime);

    }

    // Cleanup before destruction
    private void OnDestroy()
    {
        if (madeWall)
        {
            Destroy(madeWall);
        }
    }

    void cooldowns()
    {
        phaseTimeCooldown = Mathf.Max(phaseTimeCooldown - cooldownUpdateTime, 0);

    }

    bool changeSize()
    {
        if (isGrowing)
            expansionAmount = Mathf.Abs(expansionAmount);
        else
            expansionAmount = -1 * Mathf.Abs(expansionAmount);

        if (Time.time - lastExpandedTime >= expandFrequency)
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

    void ballExpandAndMakeWall(bool lockSize)
    {
        if (madeWall != null && madeWall.GetComponent<expandingWallScript>().isDone)
        {
            Destroy(madeWall);
            isGrowing = false;
        }
        bool changedSize = changeSize();
        if (changedSize == false)
        {
            //if the ball has expanded to the correct size, then make the new wall
            if (madeWall == null && body.velocity == Vector3.zero)
            {
                Debug.Log("Made wall");

                makeWall(lockSize);
            }
        }

    }

    void makeWall(bool lockSize)
    {
        madeWall = Instantiate(wallPrefab, this.transform.position,
                    this.transform.rotation);
        madeWall.transform.Rotate(new Vector3(90, 90, 0));
        madeWall.GetComponent<expandingWallScript>().lockWallSize = lockSize;
    }

    void moveTowardPoint(float powerMuliplier = 1)
    {
        transform.LookAt(new Vector3(goalTransform.position.x, transform.position.y, goalTransform.position.z),
            new Vector3(1, 0, 0));

        Vector3 distance = (goalTransform.position - this.transform.position) * powerMuliplier * rollPower;


        body.AddForce(distance);
        //body.velocity = (goalTransform.position - this.transform.position);
        //Debug.Log("moved " + distance);



    }

    void stopMomentum()
    {
        if (Time.time - startMovingTime > rollTime)
        {
            body.velocity = Vector3.zero;

        }
    }

    void spinAttack()
    {
        //Debug.Log("Changed lock to true");
        //if (madeWall == null)
        //    makeWall();
        //if (isDoneWithCurrentPhase)
        //    return;

        ballExpandAndMakeWall(true);

        if (hasDoneOneTimePhaseCode == false)
        {
            if (madeWall != null)
            {
                madeWall.GetComponent<expandingWallScript>().lockWallSize = true;
                Debug.Log("locked wall size");

                hasDoneOneTimePhaseCode = true;
                phaseTimeCooldown = 5f;
            }
            return;
        }
        //Debug.Log(madeWall);


        float rotateAmount = .4f;
        transform.Rotate(new Vector3(0, 0, rotateAmount));
        if (madeWall != null)
        {
            madeWall.transform.Rotate(-rotateAmount, 0, 0);
            madeWall.GetComponent<expandingWallScript>().lockWallSize = true;
        }
        //Debug.Log("phase cooldown " + phaseTimeCooldown);

        if (phaseTimeCooldown <= 0)
        {
            if (madeWall != null)
            {
                madeWall.GetComponent<expandingWallScript>().lockWallSize = false;
                Debug.Log("set to false");
                if (madeWall.GetComponent<expandingWallScript>().isDone)
                {

                    Destroy(madeWall);
                    isDoneWithCurrentPhase = true;
                }
            }

        }
        //mad.GetComponent<Transform>().Rotate(new Vector3(0, 0, 2));


    }

    void moveAndMakeWallAttack()
    {
        if (hasDoneOneTimePhaseCode == false)
        {
            hasDoneOneTimePhaseCode = true;
            phaseTimeCooldown = 10f;
        }




        if (Vector3.Distance(transform.position, goalTransform.position) < distanceToPlayerForAttack)
        {
            body.velocity = Vector3.zero;
            isGrowing = true;
        }

        if (hasStartedMoveAndWall == false)
        {
            moveTowardPoint();
            startMovingTime = Time.time;
            hasStartedMoveAndWall = true;
            //isGrowing = true;
            lastExpandedTime = Time.time;
        }
        else
        {
            stopMomentum();
            //isGrowing = true;
            ballExpandAndMakeWall(false);
        }

        if (phaseTimeCooldown <= 0)
        {
            if (madeWall != null)
            {

                if (madeWall.GetComponent<expandingWallScript>().isDone)
                {

                    Destroy(madeWall);
                    isDoneWithCurrentPhase = true;
                }
            }

        }

    }

    void choosePhase()
    {
        if (phaseTimeCooldown <= 0 && isDoneWithCurrentPhase)
        {
            //get the total number of phases the boss can have
            int totalPhases = System.Enum.GetNames(typeof(bossPhases)).Length;
            currentPhase = (bossPhases)(currentPhaseNum % totalPhases);
            currentPhaseNum++;
            Debug.Log("Changed phase");
            isDoneWithCurrentPhase = false;
            hasDoneOneTimePhaseCode = false;

        }
        Debug.Log("current phase " + currentPhase.ToString());
        Debug.Log("phase cooldown " + phaseTimeCooldown);

        switch (currentPhase)
        {
            case bossPhases.moveAndMakeWall:
                moveAndMakeWallAttack();
                break;
            case bossPhases.spinAttackPhase:
                spinAttack();

                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        choosePhase();

        //if (Input.GetKeyDown(KeyCode.R))
        //moveAndMakeWallAttack();
        //spinAttack();
        //wallExpand();


    }
}
