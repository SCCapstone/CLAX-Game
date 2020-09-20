using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingBossScript : MonoBehaviour
{

    //Vector3 scale;
    float lastExpandedTime;
    float expandFrequency = .3f;
    float maxYSize = 4.8f;
    float normalSize;
    float expansionAmount = .3f;

    public GameObject wallPrefab;

    bool isMoving = false;
    bool isGrowing = true;

    GameObject madeWall;
    bool isFacingTarget = false;

    public Transform goalTransform;
    float startMovingTime;
    float rollTime = 2;
    Rigidbody body;


    enum bossPhases
    {
        spinAttack, moveAndMakeWall
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
            //the ball is already the correct size
            return false;

        }
        //did not attempt to change size
        return true;

    }

    void ballExpand()
    {
        bool changedSize = changeSize();
        if (changedSize == false)
        {
            //if the ball has expanded to the correct size, then make the new wall
            if (madeWall == null)
            {
                madeWall = Instantiate(wallPrefab, transform.position,
                    transform.rotation);
                madeWall.transform.Rotate(new Vector3(90, 90, 0));
            }
        }

    }

    void faceTarget()
    {
        if (isFacingTarget == false)
        {
            transform.LookAt(goalTransform, new Vector3(1, 0, 0));
            isFacingTarget = true;
        }
    }

    void moveTowardPoint()
    {
        faceTarget();
        if (isMoving == false)
        {
            int power = 15;
            Vector3 distance = (goalTransform.position - this.transform.position) * power;

            body.AddForce(distance);
            isMoving = true;
            startMovingTime = Time.time;
        }
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

    void choosePhase()
    {

    }

    void moveMakeWallAttack()
    {
        //ballExpand()
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        ballExpand();
        moveTowardPoint();
        stopMomentum();
        updateWallPos();
        //wallExpand();


    }
}
