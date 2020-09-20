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

    bool hasActiveWall = false;

    bool isMoving = false;

    GameObject madeWall;
    bool isFacingTarget = false;

    public Transform goalTransform;
    float startMovingTime;
    float rollTime = 2;
    Rigidbody body;


    // Start is called before the first frame update
    void Start()
    {
        //scale = gameObject.transform.localScale;
        lastExpandedTime = Time.time;
        body = GetComponent<Rigidbody>();
        normalSize = transform.localScale.y;
    }

    void ballExpand()
    {

        if (Time.time - lastExpandedTime >= expandFrequency)
        {
            //Debug.Log(transform.localScale.y + expansionAmount);
            //Debug.Log(maxYSize);

            if (transform.localScale.y + expansionAmount < maxYSize)
            {

                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + expansionAmount,
                    transform.localScale.z);
                lastExpandedTime = Time.time;
            }
            else
            {
                //if the ball has expanded to the correct size, then make the new wall
                if (madeWall == null)
                {
                    madeWall = Instantiate(wallPrefab, transform.position,
                        transform.rotation);
                    //var curRotatation = madeWall.transform.rotation;
                    //madeWall.transform.rotation = new Quaternion(curRotatation.x, curRotatation.y, curRotatation.z + 90, curRotatation.w);
                    madeWall.transform.Rotate(new Vector3(90, 90, 0));


                    //madeWall.transform.RotateArou
                }
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
