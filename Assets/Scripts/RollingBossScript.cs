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
    bool doneWithCurrentPhase = true;

    public Transform goalTransform;
    float startMovingTime;
    float rollTime = 2;

    bool hasStartedMoveAndWall = false;


    enum bossPhases
    {
        moveAndMakeWall = 0, spinAttack
    }

    bossPhases currentPhase = bossPhases.moveAndMakeWall;


    // Start is called before the first frame update
    void Start()
    {
        //scale = gameObject.transform.localScale;
        lastExpandedTime = Time.time;
        body = GetComponent<Rigidbody>();
        normalSize = transform.localScale.y;
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

    void ballExpand()
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
                madeWall = Instantiate(wallPrefab, this.transform.position,
                    this.transform.rotation);
                madeWall.transform.Rotate(new Vector3(90, 90, 0));
            }
        }

    }

    void moveTowardPoint(float powerMuliplier = 1)
    {
        transform.LookAt(new Vector3(goalTransform.position.x, transform.position.y, goalTransform.position.z),
            new Vector3(1, 0, 0));

        Vector3 distance = (goalTransform.position - this.transform.position) * powerMuliplier * rollPower;


        body.AddForce(distance);
        //body.velocity = (goalTransform.position - this.transform.position);
        Debug.Log("moved " + distance);



    }

    void updateWallPos()
    {
        //if (madeWall)
        //{
        //    madeWall.transform.position = transform.position;
        //}
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
        wallPrefab.GetComponent<expandingWallScript>().lockWallSize = true;
        ballExpand();
        float rotateAmount = .4f;
        transform.Rotate(new Vector3(0, 0, rotateAmount));
        if (madeWall != null)
        {
            madeWall.transform.Rotate(-rotateAmount, 0, 0);


        }
        //mad.GetComponent<Transform>().Rotate(new Vector3(0, 0, 2));


    }

    void choosePhase()
    {
        if (doneWithCurrentPhase)
        {
            //get the total number of phases the boss can have
            int totalPhases = System.Enum.GetNames(typeof(bossPhases)).Length;
            currentPhase = (bossPhases)(currentPhaseNum % totalPhases);
            currentPhaseNum++;
            doneWithCurrentPhase = false;
        }
        switch (currentPhase)
        {
            case bossPhases.moveAndMakeWall:
                moveAndMakeWallAttack();
                break;
            case bossPhases.spinAttack:
                break;
            default:
                break;
        }
    }

    void moveAndMakeWallAttack()
    {
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
            ballExpand();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //moveAndMakeWallAttack();
        spinAttack();
        //wallExpand();


    }
}
