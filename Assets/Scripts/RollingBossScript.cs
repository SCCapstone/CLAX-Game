using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingBossScript : MonoBehaviour
{

    //Vector3 scale;
    float lastExpandedTime;
    public float expandFrequency = .5f;
    public float maxYSize = 10.5f;
    public float expansionAmount = .5f;


    public GameObject wallPrefab;

    bool hasActiveWall = false;
    GameObject madeWall;


    // Start is called before the first frame update
    void Start()
    {
        //scale = gameObject.transform.localScale;
        lastExpandedTime = Time.time;
    }

    void ballExpand()
    {

        if (Time.time - lastExpandedTime >= expandFrequency)
        {
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
                    madeWall = Instantiate(wallPrefab);
                }
            }

        }

    }




    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        ballExpand();
        //wallExpand();


    }
}
