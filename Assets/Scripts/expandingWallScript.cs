using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandingWallScript : MonoBehaviour
{

    float lastWallExpandTime;
    float maxWallHeight = 20;
    //public float wallExpandFrequency = .15f;
    float maxHorizontalLength = 60;
    float growAmount = 50f;

    public bool isDone = false;
    bool isGrowing = true;

    public bool lockWallSize = false;

    GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        lockWallSize = false;
        //Debug.Log("lock wall is " + lockWallSize);
        boss = GameObject.FindGameObjectWithTag("Boss");


    }

    void wallExpand()
    {
        if (isGrowing && transform.localScale.x >= maxHorizontalLength)
        {
            isGrowing = false;
            //return;
        }
        if (!isGrowing && transform.localScale.x < 3)
        {
            isDone = true;
            return;
        }

        int direction = 1;
        if (!isGrowing)
            direction = -1;

        if (isGrowing == false && lockWallSize)
        {
            direction = 0;
        }
        //Debug.Log("direction is " + direction);


        //only expand every so often
        //if (Time.time - lastWallExpandTime >= wallExpandFrequency)
        //{
        var curPos = transform.position;
        var curScale = transform.localScale;

        var bossAliveComponent = boss.GetComponent<AliveObject>();
        var bossAlivePercent = bossAliveComponent.health / bossAliveComponent.maxHealth;

        Vector3 scale = new Vector3(curScale.x + growAmount * direction * Time.fixedDeltaTime,
               0, .2f);
        if (bossAlivePercent > .67f)
        {
            //low wall
            scale.Set(scale.x,
                Mathf.Min(Mathf.Max(curScale.y + .5f * direction * (maxWallHeight / maxHorizontalLength / 4), .2f),
                maxWallHeight), scale.z);
        }
        else if (bossAlivePercent > .33f)
        {
            //medium wall to out run
            scale.Set(scale.x,
                Mathf.Min(Mathf.Max(curScale.y + .5f * direction * (maxWallHeight / maxHorizontalLength / 2), .2f),
                maxWallHeight), scale.z);
        }
        else
        {
            //higher wall
            scale.Set(scale.x,
                Mathf.Min(Mathf.Max(curScale.y + .5f * direction * (maxWallHeight / maxHorizontalLength), .2f),
                maxWallHeight), scale.z);
        }

        transform.localScale = scale;



        transform.position = new Vector3(curPos.x, 0, curPos.z);

        //lastWallExpandTime = Time.time;
        //}
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        wallExpand();
    }
}
